using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL.Models;

namespace DotnetAcademy.DAL.EntityConfigurations {
    class InvoiceEntityConfiguration: EntityTypeConfiguration<Invoice> {
        public InvoiceEntityConfiguration() {
            this.Map(m => m.ToTable("Invoice"))
                .HasKey(i => i.Id)
                .HasMany(i => i.DetailLines)
                .WithRequired(d => d.Invoice)
                .HasForeignKey(d => d.InvoiceId);

            this.Property(i => i.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            this.Property(i => i.Code)
                .HasColumnName("Code")
                .HasMaxLength(20)
                .IsRequired();

            this.Property(i => i.Date)
                .IsRequired();

            this.Property(i => i.Deleted)
                .HasColumnName("Deleted")
                .HasColumnType("bit")
                .IsRequired();

            this.Property(i => i.DeleteMessage)
                .HasColumnName("DeleteMessage")
                .HasColumnType("nvarchar")
                .HasMaxLength(300)
                .IsOptional();
        }
    }
}
