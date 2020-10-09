using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using DotnetAcademy.DAL;

namespace DotnetAcademy.BLL {
    public class InvoiceBusinessLogic : IInvoiceBusinessLogic<InvoiceViewModel> {
        private IUnitOfWork _uow;
        private IDetailLineBusinessLogic<DetailLineViewModel> _detailLineBusinessLogic;
        private ICustomerBusinessLogic<CustomerViewModel> _customerBusinessLogic;
        private IProductBusinessLogic<ProductViewModel> _productBusinessLogic;
        private Mapper mapper = new Mapper();

        public InvoiceBusinessLogic(UnitOfWork uow, DetailLineBusinessLogic detailLineBusinessLogic,
            CustomerBusinessLogic customerBusinessLogic, ProductBusinessLogic productBusinessLogic) {
            _uow = uow;
            _detailLineBusinessLogic = detailLineBusinessLogic;
            _customerBusinessLogic = customerBusinessLogic;
            _productBusinessLogic = productBusinessLogic;
        }

        public void Create(InvoiceViewModel invoiceDTO) {
            Invoice invoice = mapper.MapDTO(invoiceDTO);
            Customer customer = _uow.CustomerRepository.FindById(invoice.CustomerId);
            invoice.Code = GetCode(invoice.Date);
            invoice.Deleted = false;
            _uow.InvoiceRepository.Create(invoice);

            _uow.SaveChanges();
        }

        public void Update(InvoiceViewModel invoiceDTO) {
            Invoice invoice = mapper.MapDTO(invoiceDTO);
            _uow.InvoiceRepository.Update(invoice);

            _uow.SaveChanges();
        }

        public void Delete(InvoiceViewModel invoiceDTO) {
            Invoice invoice = mapper.MapDTO(invoiceDTO);
            _uow.InvoiceRepository.Delete(invoice);

            _uow.SaveChanges();
        }

        public InvoiceViewModel FindById(int? id) {
            Invoice invoice = _uow.InvoiceRepository.FindById(id);

            InvoiceViewModel invoiceDTO = mapper.MapModel(invoice);
            invoiceDTO.DetailLines = _detailLineBusinessLogic.FindByInvoice(invoiceDTO);
            return invoiceDTO;
        }

        public List<InvoiceViewModel> GetAll() {
            List<Invoice> invoices = _uow.InvoiceRepository.GetAll().Where(i => i.Deleted == false).ToList();
            return mapper.MapModelList(invoices);
        }

        public List<InvoiceViewModel> GetAllByCustomerId(int? id) {
            return GetAll().Where(i => i.CustomerId == id).ToList();
        }

        public InvoiceDetailViewModel GetDetail(int? id) {
            InvoiceViewModel invoice = FindById(id);
            List<DetailLineViewModel> detailLines = _detailLineBusinessLogic.FindByInvoice(invoice);
            InvoiceDetailViewModel invoiceDetail = new InvoiceDetailViewModel() {
                Invoice = invoice,
                DetailLines = detailLines,
                TotalAmount = GetTotalAmount(detailLines),
                Discount = GetDiscount(detailLines),
                VAT = GetVAT(detailLines),
                FinalTotal = GetTotalPrice(detailLines)
            };
            invoiceDetail.Customer = _customerBusinessLogic.FindById(invoice.CustomerId);
            return invoiceDetail;
        }

        public void SetInvoiceInactive(int? id) {
            try {
                Invoice invoice = _uow.InvoiceRepository.FindById(id);
                invoice.Deleted = true;
                _uow.InvoiceRepository.Update(invoice);

                _uow.SaveChanges();
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public bool HasDetailLines(InvoiceViewModel invoiceDTO) {
            try {
                bool hasDetailLines;
                List<DetailLineViewModel> detailLines = _detailLineBusinessLogic.FindByInvoice(invoiceDTO);
                if (detailLines.Count > 0) {
                    hasDetailLines = true;
                } else {
                    hasDetailLines = false;
                }

                return hasDetailLines;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        // Get all products a user has bought so far with the corresponding invoice
        public List<ProductPerInvoiceViewModel> GetCurrentUserProductsPerInvoice(string userId) {
            List<ProductPerInvoiceViewModel> productsPerInvoice = new List<ProductPerInvoiceViewModel>();
            CustomerViewModel customer = _customerBusinessLogic.FindByApplicationUserId(userId);
            List<InvoiceViewModel> invoices = this.GetAllByCustomerId(customer.Id);

            foreach (var invoice in invoices) {
                List<DetailLineViewModel> detailLines = _detailLineBusinessLogic.FindByInvoice(invoice);

                foreach (var detailLine in detailLines) {
                    ProductPerInvoiceViewModel productPerInvoice = new ProductPerInvoiceViewModel() {
                        Invoice = invoice,
                        DetailLine = detailLine
                    };
                    productsPerInvoice.Add(productPerInvoice);
                }
            }

            return productsPerInvoice;
        }

        // Get all products and the amount of them that have been sold so far
        public List<SoldProductViewModel> GetTotalSoldProducts() {
            List<SoldProductViewModel> soldProducts = new List<SoldProductViewModel>();
            List<ProductViewModel> products = _productBusinessLogic.GetAll();
            List<InvoiceViewModel> invoices = this.GetAll();

            foreach (var product in products) {
                int productCounter = 0;

                foreach (var invoice in invoices) {
                    List<DetailLineViewModel> detailLines = _detailLineBusinessLogic.FindByInvoice(invoice);

                    foreach (var detailLine in detailLines) {
                        if (detailLine.Product.Name == product.Name) {
                            productCounter += detailLine.Amount;
                        }
                    }
                }
                SoldProductViewModel soldProduct = new SoldProductViewModel() {
                    Product = product,
                    TotalSold = productCounter
                };
                soldProducts.Add(soldProduct);
            }

            return soldProducts;
        }

        public decimal GetTotalAmount(List<DetailLineViewModel> detailLinesDTO) {
            try {
                decimal totalAmount = 0;
                List<DetailLine> detailLines = mapper.MapDTOList(detailLinesDTO);
                foreach (DetailLine detailLine in detailLines) {
                    Product product = _uow.ProductRepository.FindById(detailLine.ProductId);
                    totalAmount += detailLine.Amount * product.Price;
                }

                return totalAmount;
                ;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        public decimal GetDiscount(List<DetailLineViewModel> detailLinesDTO) {
            decimal totalDiscount = 0;
            List<DetailLine> detailLines = mapper.MapDTOList(detailLinesDTO);
            foreach (DetailLine detailLine in detailLines) {
                decimal percentage = detailLine.Discount / 100;
                Product product = _uow.ProductRepository.FindById(detailLine.ProductId);
                totalDiscount += product.Price * detailLine.Amount * percentage;
            }

            return totalDiscount;
        }

        public decimal GetVAT(List<DetailLineViewModel> detailLinesDTO) {
            decimal totalVAT = 0;
            List<DetailLine> detailLines = mapper.MapDTOList(detailLinesDTO);
            foreach (DetailLine detailLine in detailLines) {
                Product product = _uow.ProductRepository.FindById(detailLine.ProductId);
                decimal percentage = product.VatPercentage / 100;
                totalVAT += product.Price * detailLine.Amount * percentage;
            }

            return totalVAT;
        }

        public decimal GetTotalPrice(List<DetailLineViewModel> detailLinesDTO) {
            try {
                decimal totalAmount = this.GetTotalAmount(detailLinesDTO);
                decimal totalDiscount = this.GetDiscount(detailLinesDTO);
                decimal totalVAT = this.GetVAT(detailLinesDTO);

                decimal total = totalAmount - totalDiscount + totalVAT;
                return total;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        //calculate the counter based on the month
        public int GetCounter(DateTime date) {
            try {
                List<Invoice> invoices = this._uow.InvoiceRepository.GetAll().Where(i => i.Date.Month == date.Month)
                    .ToList();
                if (invoices.Count == 0) {
                    return 0;
                } else {
                    return invoices.Max(i => int.Parse(i.Code.Split('-')[2]));
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        //calculate the invoice code
        public string GetCode(DateTime date) {
            try {
                string year = date.Year.ToString();
                string month = date.Month.ToString("D2");
                int counter = GetCounter(date);
                string output = $"{year}-{month}-{++counter:0000}";
                return output;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        //compare months to reset counter after each month
        public bool HasDifferentMonth(InvoiceViewModel oldInvoiceDTO, InvoiceViewModel newInvoiceDTO) {
            Invoice oldInvoice = mapper.MapDTO(oldInvoiceDTO);
            Invoice newInvoice = mapper.MapDTO(newInvoiceDTO);

            return oldInvoice.Date.Month != newInvoice.Date.Month;
        }
    }
}