using DotnetAcademy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.DAL.Repositories 
{
    public class InvoiceRepository : IDbRepository<Invoice> {
        private MainDbContext _context;

        public InvoiceRepository(MainDbContext context) {
            _context = context;
        }

        public void Create(Invoice invoice)
        {
            try
            {
                _context.Invoices.Add(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public void Delete(Invoice invoice)
        {
            try
            {
                _context.Invoices.Remove(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public Invoice FindById(int? id)
        {
            try
            {
                return _context.Invoices.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void Update(Invoice invoice)
        {
            try
            {
                _context.Invoices.AddOrUpdate(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public List<Invoice> GetAll()
        {
            try
            {
                return _context.Invoices.Include(c => c.Customer).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
