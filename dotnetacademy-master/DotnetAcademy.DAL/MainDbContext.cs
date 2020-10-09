using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL.EntityConfigurations;
using DotnetAcademy.DAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DotnetAcademy.DAL {
    public class MainDbContext : IdentityDbContext<ApplicationUser>, IMainDbContext {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<DetailLine> DetailLines { get; set; }
        public DbSet<Product> Products { get; set; }

        IQueryable<Customer> IMainDbContext.Customers => Customers;
        IQueryable<Invoice> IMainDbContext.Invoices => Invoices;
        IQueryable<DetailLine> IMainDbContext.DetailLines => DetailLines;
        IQueryable<Product> IMainDbContext.Products => Products;

        public MainDbContext()
            : base("DefaultConnection", false) { }

        public static MainDbContext Create() {
            return new MainDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new CustomerEntityConfiguration());
            modelBuilder.Configurations.Add(new DetailLineEntityConfiguration());
            modelBuilder.Configurations.Add(new InvoiceEntityConfiguration());
            modelBuilder.Configurations.Add(new ProductEntityConfiguration());
        }
    }
}
