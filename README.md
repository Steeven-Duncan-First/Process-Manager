# Golden Source - Gestion des Procédures Bancaires

Application Web ASP.NET MVC 3.1 pour la gestion centralisée des procédures bancaires.

## Fonctionnalités

- Authentification via LDAP (Active Directory)
- Gestion des procédures (création, modification, archivage)
- Recherche avancée avec indexation de texte et OCR
- Chatbot intelligent pour la recherche
- Gestion des droits d'accès (C1, C2, C3)
- Suivi des consultations et modifications
- Interface responsive avec Bootstrap

## Prérequis

- .NET Core SDK 3.1
- SQL Server 2019 ou supérieur
- Active Directory (LDAP)
- Tesseract OCR
- Visual Studio 2019 ou supérieur

## Installation

1. Cloner le repository :
```bash
git clone https://github.com/votre-organisation/golden-source.git
```

2. Configurer la base de données :
- Créer une base de données SQL Server nommée "GoldenSource"
- Exécuter les migrations Entity Framework :
```bash
dotnet ef database update
```

3. Configurer l'authentification LDAP :
- Modifier le fichier `appsettings.json` avec les informations de votre serveur LDAP :
```json
{
  "Ldap": {
    "Server": "ldap://votre-serveur",
    "Domain": "votre-domaine",
    "Container": "DC=votre-domaine,DC=com"
  }
}
```

4. Configurer le stockage des fichiers :
- Créer les dossiers nécessaires :
  - C:\GoldenSource\Documents
  - C:\GoldenSource\Index
  - C:\GoldenSource\TesseractData

5. Installer Tesseract OCR :
- Télécharger et installer Tesseract OCR
- Copier les fichiers de données de langue dans C:\GoldenSource\TesseractData

## Démarrage

1. Compiler le projet :
```bash
dotnet build
```

2. Lancer l'application :
```bash
dotnet run
```

3. Accéder à l'application :
- Ouvrir un navigateur et aller à https://localhost:5001

## Structure du Projet

```
GoldenSource/
├── Controllers/         # Contrôleurs MVC
├── Models/             # Modèles de données
├── Views/              # Vues Razor
├── Services/           # Services métier
├── Data/              # Contexte de base de données
├── wwwroot/           # Fichiers statiques
└── Migrations/        # Migrations Entity Framework
```

## Sécurité

- Authentification via LDAP
- Gestion des rôles et permissions
- Protection CSRF
- Validation des entrées
- Journalisation des accès

## Maintenance

### Sauvegarde

- Sauvegarder régulièrement la base de données
- Sauvegarder le dossier Documents
- Sauvegarder l'index de recherche

### Nettoyage

- Archiver les procédures obsolètes
- Nettoyer les fichiers temporaires
- Optimiser l'index de recherche

## Support

Pour toute question ou problème :
- Contacter l'équipe IT
- Consulter la documentation technique
- Ouvrir une issue sur GitHub

## Licence

Ce projet est propriétaire et confidentiel. Tous droits réservés. 