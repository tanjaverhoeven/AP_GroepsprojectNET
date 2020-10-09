using DotnetAcademy.BLL;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DotnetAcademy.API.Controllers
{
    [Authorize]
    public class DetailLineController : ApiController
    {
        private IDetailLineBusinessLogic<DetailLineViewModel> _detailLineLogic;

        public DetailLineController(DetailLineBusinessLogic detailLineLogic)
        {
            _detailLineLogic = detailLineLogic;
        }

        [HttpGet]
        public List<DetailLineViewModel> Index()
        {
            return _detailLineLogic.GetAll();
        }

        [HttpGet]
        public DetailLineViewModel Detail(int? id)
        {
            return _detailLineLogic.FindById(id);
        }

        [HttpDelete]
        [Authorize(Roles = "administrator")]
        public void Delete(int id, string message)
        {
            DetailLineViewModel delDetailLine = _detailLineLogic.FindById(id);
            delDetailLine.Deleted = true;
            delDetailLine.DeleteMessage = message;
            _detailLineLogic.Update(delDetailLine);
        }
    }
}
