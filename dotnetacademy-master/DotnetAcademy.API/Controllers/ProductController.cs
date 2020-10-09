using DotnetAcademy.BLL;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotnetAcademy.Common.DTO;

namespace DotnetAcademy.API.Controllers {
    public class ProductController : ApiController {
        private IProductBusinessLogic<ProductViewModel> _productLogic;

        public ProductController(ProductBusinessLogic productLogic) {
            _productLogic = productLogic;
        }

        // GET: api/Product/All
        [HttpGet]
        public List<ProductViewModel> All() {
            return _productLogic.GetAll();
        }

        // POST: api/Product/Create
        [HttpPost]
        [Authorize(Roles = "administrator")]
        public IHttpActionResult Create(ProductViewModel model) {
            try {
                _productLogic.Create(model);

                return Ok();
            } catch (Exception e) {
                Console.WriteLine(e);
                return InternalServerError();
            }
        }

        // POST: api/Product/Update
        [HttpPost]
        [Authorize(Roles = "administrator")]
        public void Update(ProductViewModel model) {
            _productLogic.Update(model);
        }

        // POST: api/Product/Delete
        [HttpPost]
        [Authorize(Roles = "administrator")]
        public void Delete(ProductViewModel model) {
            _productLogic.Delete(model);
        }

        // GET: api/Product/Get/5
        public ProductViewModel Get(int? id) {
            return _productLogic.FindById(id);
        }

    }
}