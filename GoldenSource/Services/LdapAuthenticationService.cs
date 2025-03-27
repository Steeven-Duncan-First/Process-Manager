using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using GoldenSource.Models;

namespace GoldenSource.Services
{
    public interface ILdapAuthenticationService
    {
        User AuthenticateUser(string employeeCode, string password);
        User GetUserInfo(string employeeCode);
    }

    public class LdapAuthenticationService : ILdapAuthenticationService
    {
        private readonly string _ldapServer;
        private readonly string _ldapDomain;
        private readonly string _ldapContainer;

        public LdapAuthenticationService(IConfiguration configuration)
        {
            _ldapServer = configuration["Ldap:Server"];
            _ldapDomain = configuration["Ldap:Domain"];
            _ldapContainer = configuration["Ldap:Container"];
        }

        public User AuthenticateUser(string employeeCode, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, _ldapDomain, _ldapContainer))
            {
                if (context.ValidateCredentials(employeeCode, password))
                {
                    return GetUserInfo(employeeCode);
                }
            }
            return null;
        }

        public User GetUserInfo(string employeeCode)
        {
            using (var context = new PrincipalContext(ContextType.Domain, _ldapDomain, _ldapContainer))
            {
                var userPrincipal = UserPrincipal.FindByIdentity(context, employeeCode);
                if (userPrincipal != null)
                {
                    return new User
                    {
                        Id = userPrincipal.Guid.ToString(),
                        EmployeeCode = employeeCode,
                        UserName = userPrincipal.SamAccountName,
                        FirstName = userPrincipal.GivenName,
                        LastName = userPrincipal.Surname,
                        Email = userPrincipal.EmailAddress,
                        Department = userPrincipal.Description,
                        Service = userPrincipal.Department,
                        Function = userPrincipal.Title,
                        IsActive = userPrincipal.Enabled ?? true,
                        LastLoginDate = DateTime.UtcNow,
                        MaxAccessLevel = DetermineAccessLevel(userPrincipal)
                    };
                }
            }
            return null;
        }

        private AccessLevel DetermineAccessLevel(UserPrincipal userPrincipal)
        {
            // Logique pour déterminer le niveau d'accès basée sur les groupes AD
            if (userPrincipal.IsMemberOf(context, "C3_Group"))
                return AccessLevel.C3;
            if (userPrincipal.IsMemberOf(context, "C2_Group"))
                return AccessLevel.C2;
            return AccessLevel.C1;
        }
    }
} 