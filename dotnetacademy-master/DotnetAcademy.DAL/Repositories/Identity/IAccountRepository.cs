using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using Microsoft.AspNet.Identity;

namespace DotnetAcademy.DAL.Repositories.Identity {
    public interface IAccountRepository {
        bool Create(ApplicationUser u, string password, List<string> roles);
        bool Update(ApplicationUser u);
        void Delete(ApplicationUser u);
        ApplicationUser FindById(string id);
        ApplicationUser FindByEmail(string email);
        Task<ApplicationUserViewModel> FindByEmailAsync(string email);
        List<string> GetRoles(string id);
        Task<string> GenerateEmailConfirmationTokenAsync(string id);
        Task SendEmailAsync(string id, string subject, string body);
        Task<IdentityResult> ConfirmEmailAsync(string id, string code);
        ApplicationUser FindByUsername(string username);
        Task<ApplicationUser> FindByUsernameAsync(string username);
        Task<string> GeneratePasswordResetTokenAsync(string id);
        Task<IdentityResult> ResetPasswordAsync(string id, ResetPasswordViewModel model);
        bool UserExists(string username);
    }
}
