using DotnetAcademy.Common.DTO;
using DotnetAcademy.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace DotnetAcademy.UI.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        // GET: Invoice
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            IEnumerable<InvoiceViewModel> invoices = await this.SetInvoiceSorting(sortOrder, searchString, currentFilter, page);

            return View(invoices);
        }

        //GET: Invoice/Customer/5
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Customer(int id)
        {
            IEnumerable<InvoiceViewModel> invoices = await ApiService<IEnumerable<InvoiceViewModel>>.GetApi($"/api/invoice/getbycustomerid/{id}", User.Identity.Name);
            CustomerViewModel customer = await ApiService<CustomerViewModel>.GetApi($"/api/Customer/Get/{id}", User.Identity.Name);
            ViewBag.CustomerName = $"{customer.FirstName} {customer.LastName}";

            return View(invoices);
        }

        // GET: Invoice/Details/5
        public async Task<ActionResult> Details(int id)
        {
            InvoiceDetailViewModel invoiceDetail = await ApiService<InvoiceDetailViewModel>.GetApi($"/api/invoice/detail/{id}", User.Identity.Name);
            return View(invoiceDetail);
        }

        // GET: Invoice/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            InvoiceViewModel invoice = await ApiService<InvoiceViewModel>.GetApi($"/api/invoice/get/{id}", User.Identity.Name);
            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete([Bind(Include = "Id, Code, Date, Deleted, DeleteMessage, Id")] InvoiceViewModel invoice)
        {
            if (ModelState.IsValid)
            {
                await ApiService<InvoiceViewModel>.PostApi($"/api/invoice/delete", invoice, User.Identity.Name);
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        #region
        public async Task<IEnumerable<InvoiceViewModel>> SetInvoiceSorting(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CustomerSortParm = String.IsNullOrEmpty(sortOrder) ? "Customer_desc" : "";
            ViewBag.CodeSortParm = sortOrder == "Code" ? "Code_desc" : "Code";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IEnumerable<InvoiceViewModel> invoices = await ApiService<IEnumerable<InvoiceViewModel>>.GetApi("/api/invoice/index", User.Identity.Name);

            if (invoices != null)
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    invoices = invoices.Where(i =>
                        i.Customer.FirstName?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Code?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Date.ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                switch (sortOrder)
                {
                    case "Customer_desc":
                        invoices = invoices.OrderByDescending(i => i.Customer.FirstName).ToList();
                        break;
                    case "Code":
                        invoices = invoices.OrderBy(i => i.Code).ToList();
                        break;
                    case "Code_desc":
                        invoices = invoices.OrderByDescending(i => i.Code).ToList();
                        break;
                    case "Date":
                        invoices = invoices.OrderBy(i => i.Date).ToList();
                        break;
                    case "Date_desc":
                        invoices = invoices.OrderByDescending(i => i.Date).ToList();
                        break;
                    default:
                        invoices = invoices.OrderBy(i => i.Customer.FirstName).ToList();
                        break;
                }
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return invoices.ToPagedList(pageNumber, pageSize);
        }
        #endregion
    }

}
