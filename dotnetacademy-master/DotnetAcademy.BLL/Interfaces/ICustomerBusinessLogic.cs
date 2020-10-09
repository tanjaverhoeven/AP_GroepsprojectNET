using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;

namespace DotnetAcademy.BLL.Interfaces
{
    public interface ICustomerBusinessLogic<T>
    {
        void Update(T t);
        void Delete(T t);
        T FindById(int? id);
        List<T> GetAll();
        bool CheckEmail(T t);
        void SetInactive(int? id);
        CustomerViewModel FindByApplicationUserId(string id);
        Task<IEnumerable<UserCustomerViewModel>> GetAllUserCustomers();
        UserCustomerViewModel GetUserCustomer(int? id);
        UserCustomerViewModel GetCurrentUserCustomer(string applicationUserId);
    }
}
