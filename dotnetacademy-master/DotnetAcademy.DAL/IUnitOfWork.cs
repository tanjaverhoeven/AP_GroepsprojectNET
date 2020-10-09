using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL.Repositories;
using DotnetAcademy.DAL.Repositories.Identity;

namespace DotnetAcademy.DAL {
    public interface IUnitOfWork {
         AccountRepository AccountRepository { get; }
         ManageRepository ManageRepository { get; }
         CustomerRepository CustomerRepository { get; }
         DetailLineRepository DetailLineRepository { get; }
         InvoiceRepository InvoiceRepository { get; }
         ProductRepository ProductRepository { get; }
         void SaveChanges();
         void Dispose();
    }
}
