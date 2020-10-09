using DotnetAcademy.BLL;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace DotnetAcademy.API.Controllers
{
    [Authorize]
    public class InvoiceController : ApiController {
        private IInvoiceBusinessLogic<InvoiceViewModel> _invoiceLogic;
        private IDetailLineBusinessLogic<DetailLineViewModel> _detailLogic;
        private ICustomerBusinessLogic<CustomerViewModel> _customerLogic;

        public InvoiceController(InvoiceBusinessLogic invoiceLogic, DetailLineBusinessLogic detailLogic,
            CustomerBusinessLogic customerLogic) {
            _invoiceLogic = invoiceLogic;
            _detailLogic = detailLogic;
            _customerLogic = customerLogic;
        }


        [HttpGet]
        [Authorize(Roles = "administrator")]
        public List<InvoiceViewModel> Index() {
            return _invoiceLogic.GetAll();
        }

        [HttpGet]
        public InvoiceViewModel Get(int? id) {
            return _invoiceLogic.FindById(id);
        }

        [HttpGet]
        public List<InvoiceViewModel> GetByCustomerId(int? id) {
            return _invoiceLogic.GetAllByCustomerId(id);
        }

        [HttpGet]
        public InvoiceDetailViewModel Detail(int? id) {
            return _invoiceLogic.GetDetail(id);
        }

        [HttpPost]
        public void Create(InvoiceViewModel invoice) {
            try {
                _invoiceLogic.Create(invoice);
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public void Delete(InvoiceViewModel invoice) {
            invoice.Deleted = true;
            _invoiceLogic.Update(invoice);
        }

        [HttpGet]
        public List<ProductPerInvoiceViewModel> GetCurrentUserProductsPerInvoice() {
            string userId = HttpContext.Current.User.Identity.GetUserId();

            return _invoiceLogic.GetCurrentUserProductsPerInvoice(userId); ;
        }

        [HttpGet]
        public List<InvoiceViewModel> GetCurrentUserInvoices() {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            CustomerViewModel customer = _customerLogic.FindByApplicationUserId(userId);

            return _invoiceLogic.GetAllByCustomerId(customer.Id);
        }

        [HttpGet]
        public List<SoldProductViewModel> GetTotalSoldProducts() {
            return _invoiceLogic.GetTotalSoldProducts();
        }
}
}
