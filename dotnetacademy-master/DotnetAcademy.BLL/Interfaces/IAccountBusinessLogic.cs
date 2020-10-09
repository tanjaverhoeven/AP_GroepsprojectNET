using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using Microsoft.AspNet.Identity;

namespace DotnetAcademy.BLL.Interfaces {
    public interface IAccountBusinessLogic {
        ApplicationUser FindById(string id);
        ApplicationUser FindByEmail(string email);
        Task<ApplicationUserViewModel> FindByEmailAsync(string email);
        bool Register(RegisterViewModel model);
        bool Update(UserCustomerViewModel model);
        void Delete(UserCustomerViewModel model);
        string GetRoles(string id);
        Task<string> GenerateEmailConfirmationTokenAsync(string id);
        Task SendEmailAsync(string id, string subject, string body);
        Task<bool> ConfirmEmailAsync(string id, string code);
        Task<bool> EmailConfirmed(string username);
        Task<string> GeneratePasswordResetTokenAsync(string id);
        Task<bool> ResetPasswordAsync(string id, ResetPasswordViewModel model);
        bool UserExists(string username);
        bool EmailExists(string email);
        bool IsDeleted(string username);
    }
}
