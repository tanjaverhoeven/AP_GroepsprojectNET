using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using DotnetAcademy.DAL.Repositories.Identity;
using DotnetAcademy.DAL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DotnetAcademy.BLL.Identity {
    public class AccountBusinessLogic : IAccountBusinessLogic {
        private IUnitOfWork _uow;
        private Mapper mapper = new Mapper();

        public AccountBusinessLogic(UnitOfWork uow) {
            _uow = uow;
        }

        public ApplicationUser FindById(string id) {
            return _uow.AccountRepository.FindById(id);
        }

        public ApplicationUser FindByEmail(string email) {
            return _uow.AccountRepository.FindByEmail(email);
        }

        public async Task<ApplicationUserViewModel> FindByEmailAsync(string email) {
            return await _uow.AccountRepository.FindByEmailAsync(email);
        }

        // Create an ApplicationUser and Customer that are linked to each other
        public bool Register(RegisterViewModel model) {
            ApplicationUser user = new ApplicationUser {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = model.EmailConfirmed,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            bool succeeded = _uow.AccountRepository.Create(user, model.Password, new List<string>() {"klant"});
            if (succeeded) {
                try {
                    Customer customer = new Customer() {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        ApplicationUser = _uow.AccountRepository.FindById(user.Id),
                        Deleted = false,
                        Email = model.Email,
                        CompanyName = model.CompanyName,
                        City = model.City,
                        Street = model.Street,
                        Postal = model.Postal,
                        VatNumber = model.VatNumber
                    };

                    _uow.CustomerRepository.Create(customer, user);
                    _uow.SaveChanges();
                } catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                }

                return true;
            }

            return false;
        }

        public bool Update(UserCustomerViewModel model) {
            ApplicationUser user = this.GetUpdatedUser(model);

            bool succeeded = _uow.AccountRepository.Update(user);
            if (succeeded) {
                try {
                    Customer customer = this.GetUpdatedCustomer(model);

                    _uow.CustomerRepository.Update(customer);
                    _uow.SaveChanges();
                    return true;
                } catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                    return false;
                }
            }

            return false;
        }

        public void Delete(UserCustomerViewModel model) {
            Customer customer = _uow.CustomerRepository.FindById(model.Id);
            customer.Deleted = true;

            try {
                _uow.CustomerRepository.Update(customer);
                _uow.SaveChanges();
            } catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public string GetRoles(string id) {
            List<string> roles = _uow.AccountRepository.GetRoles(id);

            return String.Join(",", roles);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string id) {
            return await _uow.AccountRepository.GenerateEmailConfirmationTokenAsync(id);
        }

        public async Task SendEmailAsync(string id, string subject, string body) {
            await _uow.AccountRepository.SendEmailAsync(id, subject, body);
        }

        public async Task<bool> ConfirmEmailAsync(string id, string code) {
            IdentityResult result = await _uow.AccountRepository.ConfirmEmailAsync(id, code);

            return result.Succeeded;
        }

        public async Task<bool> EmailConfirmed(string username) {
            var result = await _uow.AccountRepository.FindByUsernameAsync(username);

            return result.EmailConfirmed;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string id) {
            return await _uow.AccountRepository.GeneratePasswordResetTokenAsync(id);
        }

        public async Task<bool> ResetPasswordAsync(string id, ResetPasswordViewModel model) {
            IdentityResult result = await _uow.AccountRepository.ResetPasswordAsync(id, model);

            return result.Succeeded;
        }

        public bool UserExists(string username) {
            return _uow.AccountRepository.UserExists(username);
        }

        public bool EmailExists(string email) {
            return _uow.AccountRepository.FindByEmail(email) != null;
        }

        public bool IsDeleted(string username) {
            string applicationUserId = _uow.AccountRepository.FindByUsername(username).Id;

            try {
                Customer customer = _uow.CustomerRepository
                    .GetAll()
                    .FirstOrDefault(c => c.ApplicationUser.Id == applicationUserId);

                return customer.Deleted;
            } catch (NullReferenceException ex) {
                Console.WriteLine(ex.StackTrace);
            }

            return false;
        }

        #region Helpers

        // Get user, then update its values so we don't get primary key conflicts when updating
        private ApplicationUser GetUpdatedUser(UserCustomerViewModel model) {
            ApplicationUser user = FindById(model.ApplicationUserId);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            return user;
        }

        private Customer GetUpdatedCustomer(UserCustomerViewModel model) {
            Customer customer = _uow.CustomerRepository.FindById(model.Id);
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Deleted = model.Deleted;
            customer.CompanyName = model.CompanyName;
            customer.City = model.City;
            customer.Street = model.Street;
            customer.Postal = model.Postal;
            customer.VatNumber = model.VatNumber;

            return customer;
        }

        #endregion
    }
}