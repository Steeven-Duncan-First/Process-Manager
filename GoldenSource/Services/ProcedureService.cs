using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoldenSource.Data;
using GoldenSource.Models;
using Tesseract;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace GoldenSource.Services
{
    public interface IProcedureService
    {
        Task<Procedure> CreateProcedureAsync(Procedure procedure, string userId);
        Task<Procedure> UpdateProcedureAsync(Procedure procedure, string userId);
        Task<bool> ArchiveProcedureAsync(int procedureId);
        Task<IEnumerable<Procedure>> SearchProceduresAsync(string query, string userId);
        Task<Procedure> GetProcedureByIdAsync(int id, string userId);
        Task LogAccessAsync(int procedureId, string userId, string action);
        Task<bool> HasAccessAsync(string userId, int procedureId);
    }

    public class ProcedureService : IProcedureService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _indexPath;
        private readonly string _tesseractDataPath;
        private readonly FSDirectory _indexDirectory;
        private readonly StandardAnalyzer _analyzer;

        public ProcedureService(ApplicationDbContext context, string indexPath, string tesseractDataPath)
        {
            _context = context;
            _indexPath = indexPath;
            _tesseractDataPath = tesseractDataPath;
            _indexDirectory = FSDirectory.Open(_indexPath);
            _analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            InitializeIndex();
        }

        private void InitializeIndex()
        {
            if (!IndexReader.IndexExists(_indexDirectory))
            {
                using var writer = new IndexWriter(_indexDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48, _analyzer));
                var procedures = _context.Procedures.ToList();
                foreach (var procedure in procedures)
                {
                    var doc = new Document();
                    doc.Add(new StringField("id", procedure.Id.ToString(), Field.Store.YES));
                    doc.Add(new TextField("title", procedure.Title, Field.Store.YES));
                    doc.Add(new TextField("department", procedure.Department ?? "", Field.Store.YES));
                    doc.Add(new TextField("affectedServices", procedure.AffectedServices ?? "", Field.Store.YES));
                    doc.Add(new TextField("recommendations", procedure.Recommendations ?? "", Field.Store.YES));
                    doc.Add(new Int32Field("accessLevel", procedure.AccessLevel, Field.Store.YES));
                    writer.AddDocument(doc);
                }
                writer.Commit();
            }
        }

        public async Task<Procedure> CreateProcedureAsync(Procedure procedure, string userId)
        {
            procedure.CreationDate = DateTime.UtcNow;
            procedure.ExpirationDate = DateTime.UtcNow.AddYears(3);
            procedure.CreatedBy = userId;
            procedure.LastModifiedBy = userId;
            procedure.LastModifiedDate = DateTime.UtcNow;

            _context.Procedures.Add(procedure);
            await _context.SaveChangesAsync();

            await IndexProcedureAsync(procedure);
            return procedure;
        }

        public async Task<Procedure> UpdateProcedureAsync(Procedure procedure, string userId)
        {
            var existingProcedure = await _context.Procedures.FindAsync(procedure.Id);
            if (existingProcedure == null)
                throw new KeyNotFoundException("Procédure non trouvée");

            existingProcedure.Title = procedure.Title;
            existingProcedure.DocumentPath = procedure.DocumentPath;
            existingProcedure.AccessLevel = procedure.AccessLevel;
            existingProcedure.Department = procedure.Department;
            existingProcedure.AffectedServices = procedure.AffectedServices;
            existingProcedure.Recommendations = procedure.Recommendations;
            existingProcedure.LastModifiedBy = userId;
            existingProcedure.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await IndexProcedureAsync(existingProcedure);
            return existingProcedure;
        }

        public async Task<bool> ArchiveProcedureAsync(int procedureId)
        {
            var procedure = await _context.Procedures.FindAsync(procedureId);
            if (procedure == null)
                return false;

            procedure.IsArchived = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Procedure>> SearchProceduresAsync(string query, string userId)
        {
            using var reader = DirectoryReader.Open(_indexDirectory);
            var searcher = new IndexSearcher(reader);
            var parser = new QueryParser(LuceneVersion.LUCENE_48, "title", _analyzer);
            var searchQuery = parser.Parse(query);

            var hits = searcher.Search(searchQuery, 10);
            var procedures = new List<Procedure>();

            foreach (var hit in hits.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);
                var procedureId = int.Parse(doc.Get("id"));
                
                if (await HasAccessAsync(userId, procedureId))
                {
                    var procedure = await _context.Procedures.FindAsync(procedureId);
                    if (procedure != null && !procedure.IsArchived)
                    {
                        procedures.Add(procedure);
                    }
                }
            }

            return procedures;
        }

        public async Task<Procedure> GetProcedureByIdAsync(int id, string userId)
        {
            if (await HasAccessAsync(userId, id))
            {
                return await _context.Procedures.FindAsync(id);
            }
            return null;
        }

        public async Task LogAccessAsync(int procedureId, string userId, string action)
        {
            var log = new ProcedureAccessLog
            {
                ProcedureId = procedureId,
                UserId = userId,
                AccessDate = DateTime.UtcNow,
                Action = action
            };

            _context.AccessLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        private async Task IndexProcedureAsync(Procedure procedure)
        {
            using (var directory = FSDirectory.Open(_indexPath))
            using (var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48))
            using (var writer = new IndexWriter(directory, new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer)))
            {
                var doc = new Document();
                doc.Add(new StringField("id", procedure.Id.ToString(), Field.Store.YES));
                doc.Add(new TextField("title", procedure.Title, Field.Store.YES));
                doc.Add(new TextField("content", await ExtractTextFromDocumentAsync(procedure.DocumentPath), Field.Store.NO));

                writer.UpdateDocument(new Term("id", procedure.Id.ToString()), doc);
                writer.Commit();
            }
        }

        private async Task<string> ExtractTextFromDocumentAsync(string documentPath)
        {
            // Implémentation de l'extraction de texte avec Tesseract OCR
            using (var engine = new TesseractEngine(_tesseractDataPath, "fra", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(documentPath))
                {
                    var result = engine.Process(img);
                    return result.GetText();
                }
            }
        }

        public async Task<bool> HasAccessAsync(string userId, int procedureId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            var procedure = await _context.Procedures.FindAsync(procedureId);
            if (procedure == null) return false;

            var userMaxAccessLevel = user.UserRoles
                .Where(r => r.ExpirationDate == null || r.ExpirationDate > DateTime.UtcNow)
                .Max(r => r.MaxAccessLevel);

            return userMaxAccessLevel >= procedure.AccessLevel;
        }
    }
} 