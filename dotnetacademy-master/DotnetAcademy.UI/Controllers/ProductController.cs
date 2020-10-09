using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotnetAcademy.Common;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.UI.Services;
using PagedList;

namespace DotnetAcademy.UI.Controllers {
    public class ProductController : Controller {
        // GET: Product
        public async Task<ActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page) {
            IEnumerable<ProductViewModel> products =
                await this.SetProductSorting(sortOrder, searchString, currentFilter, page);

            return View((IPagedList<ProductViewModel>) products);
        }

        // GET: Product/Create
        [Authorize(Roles = "administrator")]
        public ActionResult Create() {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id, Name, Level, Type, Category, Description, Price, VatPercentage, IsActive")]
            ProductViewModel model) {
            if (ModelState.IsValid) {
                model.IsActive = true;

                await ApiService<ProductViewModel>.PostApi("/api/product/create", model, User.Identity.Name);

                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: Product/Edit/5
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductViewModel model = await ApiService<ProductViewModel>.GetApi($"/api/product/get/{id}");

            if (model == null) {
                return HttpNotFound();
            }

            if (!model.IsActive) {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id, Name, Level, Type, Category, Description, Price, VatPercentage, IsActive")]
            ProductViewModel updatedProduct) {
            if (ModelState.IsValid) {
                await ApiService<ProductViewModel>.PostApi($"/api/product/update", updatedProduct, User.Identity.Name);

                return RedirectToAction("Index");
            }

            return View(updatedProduct);
        }

        // GET: Product/Details/5
        public async Task<ActionResult> Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductViewModel model = await ApiService<ProductViewModel>.GetApi($"/api/product/get/{id}");

            if (model == null) {
                return HttpNotFound();
            }

            if (!model.IsActive) {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductViewModel model = await ApiService<ProductViewModel>.GetApi($"/api/product/get/{id}");

            if (model == null) {
                return HttpNotFound();
            }

            if (!model.IsActive) {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: Product/Delete/5
        [Authorize(Roles = "administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(ProductViewModel model) {
            await ApiService<ProductViewModel>.PostApi($"/api/product/delete", model, User.Identity.Name);

            return RedirectToAction("Index");
        }

        // GET: Product/SoldProducts
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> SoldProducts() {
            List<SoldProductViewModel> soldProducts =
                await ApiService<List<SoldProductViewModel>>.GetApi("/api/invoice/GetTotalSoldProducts", User.Identity.Name);

            return View(soldProducts);
        }


        #region Helpers

        // Set all the sorting, searching and paging logic
        public async Task<IEnumerable<ProductViewModel>> SetProductSorting(string sortOrder, string searchString,
            string currentFilter, int? page) {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.LevelSortParm = sortOrder == "Level" ? "Level_desc" : "Level";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.VatPercentageSortParm = sortOrder == "VatPercentage" ? "VatPercentage_desc" : "VatPercentage";
            ViewBag.IsActiveSortParm = sortOrder == "IsActive" ? "IsActive_desc" : "IsActive";

            if (searchString != null) {
                page = 1;
            } else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IEnumerable<ProductViewModel> products =
                await ApiService<IEnumerable<ProductViewModel>>.GetApi("/api/product/All");

            if (products != null) {
                if (!String.IsNullOrEmpty(searchString)) {
                    products = products.Where(i =>
                        i.Name?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Level?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Type?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Category?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Description?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Price.ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.VatPercentage.ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.IsActive.ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                switch (sortOrder) {
                    case "Name_desc":
                        products = products.OrderByDescending(i => i.Name).ToList();
                        break;
                    case "Level":
                        products = products.OrderBy(i => i.Level).ToList();
                        break;
                    case "Level_desc":
                        products = products.OrderByDescending(i => i.Level).ToList();
                        break;
                    case "Type":
                        products = products.OrderBy(i => i.Type).ToList();
                        break;
                    case "Type_desc":
                        products = products.OrderByDescending(i => i.Type).ToList();
                        break;
                    case "Category":
                        products = products.OrderBy(i => i.Category).ToList();
                        break;
                    case "Category_desc":
                        products = products.OrderByDescending(i => i.Category).ToList();
                        break;
                    case "Description":
                        products = products.OrderBy(i => i.Description).ToList();
                        break;
                    case "Description_desc":
                        products = products.OrderByDescending(i => i.Description).ToList();
                        break;
                    case "Price_desc":
                        products = products.OrderByDescending(i => i.Price).ToList();
                        break;
                    case "Price":
                        products = products.OrderBy(i => i.Price).ToList();
                        break;
                    case "VatPercentage_desc":
                        products = products.OrderByDescending(i => i.VatPercentage).ToList();
                        break;
                    case "VatPercentage":
                        products = products.OrderBy(i => i.VatPercentage).ToList();
                        break;
                    case "IsActive_desc":
                        products = products.OrderByDescending(i => i.IsActive).ToList();
                        break;
                    case "IsActive":
                        products = products.OrderBy(i => i.IsActive).ToList();
                        break;
                    default:
                        products = products.OrderBy(i => i.Name).ToList();
                        break;
                }
            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return products.ToPagedList(pageNumber, pageSize);
        }

        #endregion
    }
}