using DotnetAcademy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.DAL
{
    public interface IMainDbContext
    {
        IQueryable<Customer> Customers { get; }
        IQueryable<Invoice> Invoices { get; }
        IQueryable<DetailLine> DetailLines { get; }
        IQueryable<Product> Products { get; }
    }
}
