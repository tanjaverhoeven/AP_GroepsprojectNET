using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace DotnetAcademy.BLL {
    public class CustomerBusinessLogic : ICustomerBusinessLogic<CustomerViewModel> {
        private IUnitOfWork _uow;
        private Mapper mapper = new Mapper();

        public CustomerBusinessLogic(UnitOfWork uow) {
            _uow = uow;
        }

        public void Delete(CustomerViewModel customerDTO) {
            try {
                Customer customer = mapper.MapDTO(customerDTO);
                _uow.CustomerRepository.Delete(customer);

                _uow.SaveChanges();
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
            }
        }

        public CustomerViewModel FindById(int? id) {
            try {
                Customer customer = _uow.CustomerRepository.FindById(id);
                return mapper.MapModel(customer);
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
                throw;
            }
        }

        public void Update(CustomerViewModel customerDTO) {
            try {
                Customer customer = mapper.MapDTO(customerDTO);
                _uow.CustomerRepository.Update(customer);

                _uow.SaveChanges();
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
            }
        }

        public bool CheckEmail(CustomerViewModel customerDTO) {
            try {
                Customer customer = mapper.MapDTO(customerDTO);
                return Convert.ToBoolean(_uow.CustomerRepository.GetAll().Where(c => c.Email == customer.Email));
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
                throw;
            }
        }

        public void SetInactive(int? id) {
            try {
                Customer customer = _uow.CustomerRepository.FindById(id);
                customer.Deleted = true;
                _uow.CustomerRepository.Update(customer);

                _uow.SaveChanges();
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public CustomerViewModel FindByApplicationUserId(string id) {
            try {
                Customer customer = _uow.CustomerRepository
                    .GetAll()
                    .FirstOrDefault(c => c.ApplicationUser.Id == id);

                return mapper.MapModel(customer);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<IEnumerable<UserCustomerViewModel>> GetAllUserCustomers() {
            List<Customer> customers = await _uow.CustomerRepository.GetAllAsync();

            List<Customer> activeCustomers = customers
                .Where(uc => uc.Deleted == false)
                .ToList();

            List<UserCustomerViewModel> userCustomers = new List<UserCustomerViewModel>();
            foreach (Customer customer in activeCustomers) {
                userCustomers.Add(FillUserCustomerViewModel(customer));
            }

            return userCustomers;
        }

        // Get the currently logged in user as UserCustomerViewModel by their applicationUserId
        public UserCustomerViewModel GetCurrentUserCustomer(string applicationUserId) {
            Customer customer = _uow.CustomerRepository
                .GetAll()
                .FirstOrDefault(c => c.ApplicationUser.Id == applicationUserId);

            return FillUserCustomerViewModel(customer);
        }

        public UserCustomerViewModel GetUserCustomer(int? id) {
            Customer customer = _uow.CustomerRepository.FindById(id);

            return FillUserCustomerViewModel(customer);
        }

        public List<CustomerViewModel> GetAll() {
            try {
                List<Customer> customers = _uow.CustomerRepository.GetAll()
                    .Where(c => c.Deleted == false).ToList();
                return mapper.MapModelList(customers);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return null;
            }
        }

        private UserCustomerViewModel FillUserCustomerViewModel(Customer customer) {
            UserCustomerViewModel model = new UserCustomerViewModel() {
                Id = customer.Id,
                Username = customer.ApplicationUser.UserName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                CompanyName = customer.CompanyName,
                City = customer.City,
                Street = customer.Street,
                Postal = customer.Postal,
                VatNumber = customer.VatNumber,
                EmailConfirmed = customer.ApplicationUser.EmailConfirmed,
                ApplicationUserId = customer.ApplicationUser.Id
            };

            return model;
        }
    }
}