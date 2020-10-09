using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotnetAcademy.Common;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.UI.Services;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace DotnetAcademy.UI.Controllers {
    public class CartController : Controller {

        // http://learningprogramming.net/net/asp-net-mvc/build-shopping-cart-with-session-in-asp-net-mvc/
        // GET: Cart
        public ActionResult Index() {
            IEnumerable<ShoppingCartItemViewModel> cart = (List<ShoppingCartItemViewModel>)Session["cart"];

            return View(cart);
        }

        // GET: Cart/Buy/5
        public async Task<ActionResult> Buy(int? id, bool inShoppingCart) {
            ProductViewModel product = await ApiService<ProductViewModel>.GetApi($"/api/product/get/{id}");

            if (product != null) {
                if (Session["cart"] == null) {
                    List<ShoppingCartItemViewModel> cart = new List<ShoppingCartItemViewModel>();
                    cart.Add(new ShoppingCartItemViewModel() { ProductViewModel = product, Quantity = 1 });
                    
                    Session["cart"] = cart;
                } else {
                    List<ShoppingCartItemViewModel> cart = (List<ShoppingCartItemViewModel>)Session["cart"];

                    int index = ItemExists(id);
                    if (index != -1) {
                        cart[index].Quantity++;
                    } else {
                        cart.Add(new ShoppingCartItemViewModel() { ProductViewModel = product, Quantity = 1 });
                    }

                    Session["cart"] = cart;
                }
            }

            // If we are in the shoppingcart view, return to the shopping cart, otherwise
            // just redirect to product list
            if (inShoppingCart) {
                return RedirectToAction("Index", "Cart");
            }

            return RedirectToAction("Index", "Product");
        }

        // GET: Cart/Remove/5
        public ActionResult Remove(int? id) {
            List<ShoppingCartItemViewModel> cart = (List<ShoppingCartItemViewModel>)Session["cart"];

            int index = ItemExists(id);

            if (cart[index].Quantity > 1) {
                cart[index].Quantity--;
            } else {
                cart.RemoveAt(index);
            }
            Session["cart"] = cart;

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> FinishOrder() {
            if (User.Identity.IsAuthenticated) {
                IEnumerable<ShoppingCartItemViewModel> cart = (IEnumerable<ShoppingCartItemViewModel>)Session["cart"];

                if (cart == null || cart.Count() == 0) {
                    TempData["EmptyCartMessage"] = "Uw winkelmand is leeg, gelieve eerst iets toe te voegen.";
                    return RedirectToAction("Index");
                }

                CustomerViewModel customer =
                    await ApiService<CustomerViewModel>.GetApi($"/api/Customer/GetCurrent",
                        User.Identity.Name);
                
                InvoiceViewModel invoice = new InvoiceViewModel() {
                    CustomerId = customer.Id,
                };

                List<DetailLineViewModel> detailLines = createDetailLinesFromcart(cart, invoice);
                invoice.DetailLines = detailLines;
                invoice.Date = DateTime.Today;

                await ApiService<InvoiceViewModel>.PostApi("/api/Invoice/Create", invoice, User.Identity.Name);
                Session.Remove("cart");

                TempData["FinishOrderSucceeded"] = "Uw bestelling is geplaatst en toegevoegd aan uw facturen.";
                return View("Index");
            }

            TempData["FinishOrderLogin"] = "Gelieve eerst in te loggen of te registreren om een aankoop te doen.";
            return RedirectToAction("Login", "Account");
        }

        #region Helpers

        private int ItemExists(int? id) {
            List<ShoppingCartItemViewModel> cart = (List<ShoppingCartItemViewModel>)Session["cart"];

            for (int i = 0; i < cart.Count; i++)
                if (cart[i].ProductViewModel.Id.Equals(id))
                    return i;
            return -1;
        }

        private List<DetailLineViewModel> createDetailLinesFromcart(IEnumerable<ShoppingCartItemViewModel> items, InvoiceViewModel invoice) {
            List<DetailLineViewModel> detailLines = new List<DetailLineViewModel>();

            foreach (ShoppingCartItemViewModel item in items) {
                DetailLineViewModel detailLine = new DetailLineViewModel() {
                    ProductId = item.ProductViewModel.Id,
                    Amount = item.Quantity,
                    Deleted = false,
                    Invoice = invoice,
                };

                detailLines.Add(detailLine);
            }

            return detailLines;
        }
        
        #endregion
    }
}