using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL;

namespace DotnetAcademy.BLL {
    public class DetailLineBusinessLogic : IDetailLineBusinessLogic<DetailLineViewModel> {
        private IUnitOfWork _uow;
        private Mapper mapper = new Mapper();

        public DetailLineBusinessLogic(UnitOfWork uow) {
            _uow = uow;
        }

        public void Create(DetailLineViewModel detailLineDTO)
        {
            try
            {
                DetailLine detailLine = mapper.MapDTO(detailLineDTO);
                _uow.DetailLineRepository.Create(detailLine);

                _uow.SaveChanges();
            }
            catch (Exception ex )
            {
                new LogWriter(ex.ToString());
            }

        }

        public void Update(DetailLineViewModel detailLineDTO)
        {
            try
            {
                DetailLine detailLine = mapper.MapDTO(detailLineDTO);
                _uow.DetailLineRepository.Update(detailLine);

                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString());
            }
        }

        public void Delete(DetailLineViewModel detailLineDTO)
        {
            try
            {
                DetailLine detailLine = mapper.MapDTO(detailLineDTO);
                _uow.DetailLineRepository.Delete(detailLine);

                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString());
            }
        }

        public DetailLineViewModel FindById(int? id)
        {
            try
            {
                DetailLine detailLine = _uow.DetailLineRepository.FindById(id);
                detailLine.Product = _uow.ProductRepository.FindById(detailLine.ProductId);
                return mapper.MapModel(detailLine);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString());
                throw;
            }
        }

        public List<DetailLineViewModel> GetAll()
        {
            try
            {
                List<DetailLine> detailLine = _uow.DetailLineRepository.GetAll().ToList();
                return mapper.MapModelList(detailLine);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString());
                throw;
            }
        }

        public List<DetailLineViewModel> FindByInvoice(InvoiceViewModel invoiceDTO)
        {
            try
            {
                Invoice invoice = mapper.MapDTO(invoiceDTO);
                int invoiceId = invoice.Id;
                List<DetailLine> detailLines = _uow.DetailLineRepository.GetAll().Where(i => i.InvoiceId == invoiceId).ToList();
                foreach (DetailLine detailLine in detailLines)
                {
                    detailLine.Product = _uow.ProductRepository.FindById(detailLine.ProductId);
                }

                return mapper.MapModelList(detailLines);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString());
                throw;
            }
        }
    }
}
