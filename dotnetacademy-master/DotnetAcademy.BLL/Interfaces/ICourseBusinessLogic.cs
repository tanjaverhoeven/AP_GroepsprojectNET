using DotnetAcademy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;

namespace DotnetAcademy.BLL.Interfaces
{
    public interface ICourseBusinessLogic<T>
    {
        void Create(T t);
        void Update(T t);
        void Delete(T t);
        T FindById(int? id);
        List<T> GetAll();
        List<T> FindByProduct(ProductViewModel t);
    }
}
