using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL.Models;

namespace DotnetAcademy.DAL.EntityConfigurations {
    public class CustomerEntityConfiguration: EntityTypeConfiguration<Customer> {
        public CustomerEntityConfiguration() {
            this.Map(m => m.ToTable("Customer"))
                .HasKey(c => c.Id)
                .HasMany(c => c.Invoices)
                .WithRequired(i => i.Customer)
                .HasForeignKey(i => i.CustomerId);

            this.HasRequired(c => c.ApplicationUser);

            this.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            this.Property(c => c.FirstName)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            this.Property(c => c.LastName)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            // Set email field to unique with IndexAnnotation
            this.Property(c => c.Email)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(
                        new IndexAttribute("IX_X_Email") {
                            IsUnique = true
                        }
                    ));

            this.Property(c => c.Deleted)
                .HasColumnType("bit")
                .IsRequired();

            this.Property(c => c.CompanyName)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsOptional();

            this.Property(c => c.City)
                .HasColumnType("nvarchar")
                .IsOptional();

            this.Property(c => c.Street)
                .HasColumnType("nvarchar")
                .IsOptional();

            this.Property(c => c.Postal)
                .HasColumnType("nvarchar")
                .IsOptional();

            this.Property(c => c.VatNumber)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsOptional();
        }
    }
}
