using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;

namespace DotnetAcademy.DAL.Repositories {
    public interface ICustomerRepository {
        void Create(Customer c, ApplicationUser u);
        void Update(Customer c);
        void Delete(Customer c);
        Customer FindById(int? id);
        List<Customer> GetAll();
        Task<List<Customer>> GetAllAsync();
    }
}
