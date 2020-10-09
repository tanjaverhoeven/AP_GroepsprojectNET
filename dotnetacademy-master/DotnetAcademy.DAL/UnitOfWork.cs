using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.DAL.Repositories;
using DotnetAcademy.DAL.Repositories.Identity;

namespace DotnetAcademy.DAL {
    public class UnitOfWork : IUnitOfWork {
        private MainDbContext _context = new MainDbContext();
        private AccountRepository _accountRepo;
        private ManageRepository _manageRepo;
        private CustomerRepository _customerRepo;
        private DetailLineRepository _detailLineRepo;
        private InvoiceRepository _invoiceRepo;
        private ProductRepository _productRepo;

        private bool _disposed = false;

        public void SaveChanges() {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing) {
            if (!this._disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public AccountRepository AccountRepository {
            get {
                if (_accountRepo == null) {
                    _accountRepo = new AccountRepository(_context);
                }

                return _accountRepo;
            }
        }

        public ManageRepository ManageRepository {
            get {
                if (_manageRepo == null) {
                    _manageRepo = new ManageRepository(_context);
                }

                return _manageRepo;
            }
        }

        public CustomerRepository CustomerRepository {
            get {
                if (_customerRepo == null) {
                    _customerRepo = new CustomerRepository(_context);
                }

                return _customerRepo;
            }
        }

        public DetailLineRepository DetailLineRepository {
            get {
                if (_detailLineRepo == null) {
                    _detailLineRepo = new DetailLineRepository(_context);
                }

                return _detailLineRepo;
            }
        }

        public InvoiceRepository InvoiceRepository {
            get {
                if (_invoiceRepo == null) {
                    _invoiceRepo = new InvoiceRepository(_context);
                }

                return _invoiceRepo;
            }
        }

        public ProductRepository ProductRepository {
            get {
                if (_productRepo == null) {
                    _productRepo = new ProductRepository(_context);
                }

                return _productRepo;
            }
        }

    }
}