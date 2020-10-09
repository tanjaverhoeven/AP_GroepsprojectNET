using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL.Models;

namespace DotnetAcademy.DAL.EntityConfigurations {
    public class DetailLineEntityConfiguration: EntityTypeConfiguration<DetailLine> {
        public DetailLineEntityConfiguration() {
            this.Map(m => m.ToTable("DetailLine"))
                .HasKey(d => d.Id);

            this.Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Id")
                .IsRequired();

            this.Property(d => d.Discount)
                .IsRequired();

            this.Property(d => d.Amount)
                .IsRequired();

        }
    }
}
