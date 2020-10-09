using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL.Models;

namespace DotnetAcademy.DAL.EntityConfigurations {
    public class ProductEntityConfiguration: EntityTypeConfiguration<Product> {
        public ProductEntityConfiguration() {
            this.Map(m => m.ToTable("Product"))
                .HasKey(p => p.Id)
                .HasMany(p => p.DetailLines)
                .WithRequired(d => d.Product)
                .HasForeignKey(d => d.ProductId);

            this.Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Id")
                .IsRequired();

            this.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            this.Property(p => p.Level)
                .HasMaxLength(25)
                .IsRequired();

            this.Property(p => p.Type)
                .IsRequired();

            this.Property(p => p.Category)
                .IsRequired();

            this.Property(p => p.Description)
                .IsRequired();

            this.Property(p => p.Price)
                .IsRequired();

            this.Property(d => d.VatPercentage)
                .IsRequired();

            this.Property(i => i.IsActive)
                .HasColumnName("Active")
                .HasColumnType("bit")
                .IsRequired();

        }
    }
}
