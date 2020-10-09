using DotnetAcademy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.DAL.Repositories {
    public class DetailLineRepository : IDbRepository<DetailLine> {
        private MainDbContext _context;

        public DetailLineRepository(MainDbContext context) {
            _context = context;
        }

        public void Create(DetailLine detailLine)
        {
            try
            {
                _context.DetailLines.Add(detailLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Delete(DetailLine detailLine)
        {
            try
            {
                _context.DetailLines.Remove(detailLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public DetailLine FindById(int? id)
        {
            try
            {
                return _context.DetailLines.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void Update(DetailLine detailLine)
        {
            try
            {
                _context.DetailLines.AddOrUpdate(detailLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public List<DetailLine> GetAll()
        {
            try
            {
                return _context.DetailLines.Include(c => c.Invoice).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;

            }
        }
    }
}
