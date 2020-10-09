using DotnetAcademy.BLL;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DotnetAcademy.DAL.Models;
using Microsoft.AspNet.Identity;

namespace DotnetAcademy.API.Controllers
{
    public class CustomerController : ApiController {
        private ICustomerBusinessLogic<CustomerViewModel> _customerLogic;
        private IInvoiceBusinessLogic<InvoiceViewModel> _invoiceLogic;

        public CustomerController(CustomerBusinessLogic customerLogic, InvoiceBusinessLogic invoiceLogic) {
            _customerLogic = customerLogic;
            _invoiceLogic = invoiceLogic;
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public async Task<IEnumerable<UserCustomerViewModel>> GetAllUserCustomers() {
            return await _customerLogic.GetAllUserCustomers();
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public List<CustomerViewModel> All() {
            return _customerLogic.GetAll();
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public UserCustomerViewModel Get([FromUri] int id) {
            return _customerLogic.GetUserCustomer(id);
        }

        // GET: api/Customer/GetCurrentUserCustomer
        [Authorize]
        public UserCustomerViewModel GetCurrentUserCustomer() {
            string userId = HttpContext.Current.User.Identity.GetUserId();

            return _customerLogic.GetCurrentUserCustomer(userId);
        }

        // GET: api/Customer/GetCurrent
        [Authorize]
        public CustomerViewModel GetCurrent() {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            CustomerViewModel customer = _customerLogic.FindByApplicationUserId(userId);

            return customer;
        }

        [HttpGet]
        public CustomerDetailViewModel Detail(int? id) {
            CustomerViewModel customer = _customerLogic.FindById(id);
            List<InvoiceViewModel> invoices = _invoiceLogic.GetAll();
            CustomerDetailViewModel customerDetail = new CustomerDetailViewModel() {
                Customer = customer,
                Invoices = invoices
            };
            return customerDetail;
        }

    }
}
