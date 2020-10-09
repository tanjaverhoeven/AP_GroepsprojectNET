using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Provider;

namespace DotnetAcademy.DAL.Repositories.Identity {
    public class AccountRepository : IAccountRepository {
        private static MainDbContext _context;
        private static UserManager<ApplicationUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;

        public AccountRepository(MainDbContext context) {
            _context = context;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            InitUserManager();
        }

        public void InitUserManager() {
            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            _userManager.PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            _userManager.UserLockoutEnabledByDefault = true;
            _userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            _userManager.MaxFailedAccessAttemptsBeforeLockout = 5;

            _userManager.EmailService = new EmailService();

            var provider = new DpapiDataProtectionProvider("DotnetAcademy");
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(provider.Create("ASP.NET Identity")) {
                    // Reset password link expires after 24 hours
                    TokenLifespan = TimeSpan.FromHours(24)
                };

            _userManager.EmailService = new EmailService();
        }

        public bool Create(ApplicationUser user, string password, List<string> roles) {
            try {
                IdentityResult userCreateResult = _userManager.Create(user, password);

                if (userCreateResult.Succeeded) {
                    foreach (string roleName in roles) {
                        IdentityRole role = _roleManager.FindByName(roleName);
                        _userManager.AddToRole(user.Id, role.Name);
                    }
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
                return false;
            }

            return true;
        }

        public bool Update(ApplicationUser u) {
            try {
                return _userManager.Update(u).Succeeded;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public void Delete(ApplicationUser u) {
            try {
                _userManager.Delete(u);
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public ApplicationUser FindById(string id) {
            return _userManager.FindById(id);
        }

        public ApplicationUser FindByEmail(string email) {
            return _userManager.FindByEmail(email);
        }

        public async Task<ApplicationUserViewModel> FindByEmailAsync(string email) {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            ApplicationUserViewModel model = new ApplicationUserViewModel() {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
            };

            return model;
        }

        public List<string> GetRoles(string id) {
            return _userManager.GetRoles(id).ToList();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string id) {
            try {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(id);
                return token;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SendEmailAsync(string id, string subject, string body) {
            try {
                await _userManager.SendEmailAsync(id, subject, body);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string id, string code) {
            return await _userManager.ConfirmEmailAsync(id, code);
        }

        public ApplicationUser FindByUsername(string username) {
            return _userManager.FindByName(username);
        }

        public async Task<ApplicationUser> FindByUsernameAsync(string username) {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string id) {
            return await _userManager.GeneratePasswordResetTokenAsync(id);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string id, ResetPasswordViewModel model) {
            return await _userManager.ResetPasswordAsync(id, model.Code, model.Password);
        }

        public bool UserExists(string username) {
            return _userManager.FindByName(username) != null;
        }

    }
}
