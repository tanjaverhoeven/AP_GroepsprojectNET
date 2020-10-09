using DotnetAcademy.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.BLL.Interfaces
{
    public interface IDetailLineBusinessLogic<T>
    {
        void Create(T t);
        void Update(T t);
        void Delete(T t);
        T FindById(int? id);
        List<T> GetAll();
        List<T> FindByInvoice(InvoiceViewModel t);
    }
}
