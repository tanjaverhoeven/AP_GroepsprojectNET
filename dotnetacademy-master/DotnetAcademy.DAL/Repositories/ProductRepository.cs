using DotnetAcademy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace DotnetAcademy.DAL.Repositories {
    public class ProductRepository : IDbRepository<Product> {
        private MainDbContext _context;

        public ProductRepository(MainDbContext context) {
            _context = context;
        }

        public void Create(Product t)
        {
            try
            {
                _context.Products.Add(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public Product FindById(int? id)
        {
            try
            {
                return _context.Products.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void Update(Product t)
        {
            try
            {
                _context.Products.AddOrUpdate(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public List<Product> GetAll()
        {
            try
            {
                return _context.Products.ToList(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        public void Delete(Product t)
        {
            try
            {
                _context.Products.Remove(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

    }
}
