using DotnetAcademy.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.BLL.Interfaces
{
    public interface IInvoiceBusinessLogic<T>
    {
        void Create(T t);
        void Update(T t);
        void Delete(T t);
        T FindById(int? id);
        List<T> GetAll();
        List<T> GetAllByCustomerId(int? id);
        void SetInvoiceInactive(int? id);
        bool HasDetailLines(T t);
        decimal GetTotalAmount(List<DetailLineViewModel> detailLinesDTO);
        decimal GetDiscount(List<DetailLineViewModel> detailLinesDTO);
        decimal GetVAT(List<DetailLineViewModel> detailLinesDTO);
        decimal GetTotalPrice(List<DetailLineViewModel> detailLinesDTO);
        int GetCounter(DateTime date);
        bool HasDifferentMonth(T oldInvoiceDTO, T newInvoiceDTO);
        string GetCode(DateTime date);
        InvoiceDetailViewModel GetDetail(int? id);
        List<ProductPerInvoiceViewModel> GetCurrentUserProductsPerInvoice(string userId);
        List<SoldProductViewModel> GetTotalSoldProducts();

    }
}
