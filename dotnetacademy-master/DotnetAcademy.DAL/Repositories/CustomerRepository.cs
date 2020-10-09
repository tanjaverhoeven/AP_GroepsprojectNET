using DotnetAcademy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DotnetAcademy.DAL.Repositories {
    public class CustomerRepository : ICustomerRepository {
        private readonly MainDbContext _context;

        public CustomerRepository(MainDbContext context) {
            _context = context;
        }

        public void Create(Customer customer, ApplicationUser user) {
            try {
                _context.Entry(user).State = EntityState.Unchanged;
                _context.Customers.Add(customer);
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public void Delete(Customer t) {
            try {
                _context.Customers.Remove(t);
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public Customer FindById(int? id) {
            try {
                return _context.Customers
                    .Include(c => c.ApplicationUser)
                    .FirstOrDefault(c => c.Id == id);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void Update(Customer t) {
            try {
                _context.Customers.AddOrUpdate(t);
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public List<Customer> GetAll() {
            try {
                return _context.Customers
                    .Include(c => c.ApplicationUser)
                    .ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Customer>> GetAllAsync() {
            try {
                return await _context.Customers
                    .Include(c => c.ApplicationUser)
                    .ToListAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}