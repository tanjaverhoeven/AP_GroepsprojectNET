using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.BLL.Interfaces
{
    public interface IProductBusinessLogic<T>
    {
        void Create(T t);
        void Update(T t);
        void Delete(T t);
        T FindById(int? id);
        List<T> GetAll();
        void SetInactive(int? id);
    }
}
