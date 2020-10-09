using AutoMapper;
using DotnetAcademy.Common;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DotnetAcademy.BLL {
    class Mapper {
        private readonly IMapper mapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => {
            //Customer
            cfg.CreateMap<Customer, CustomerViewModel>();
            cfg.CreateMap<CustomerViewModel, Customer>();

            //Invoice
            cfg.CreateMap<Invoice, InvoiceViewModel>();
            cfg.CreateMap<InvoiceViewModel, Invoice>();

            //DetailLine
            cfg.CreateMap<DetailLine, DetailLineViewModel>();
            cfg.CreateMap<DetailLineViewModel, DetailLine>();

            //Product
            cfg.CreateMap<Product, ProductViewModel>();
            cfg.CreateMap<ProductViewModel, Product>();

        }));

        //Customer
        public CustomerViewModel MapModel(Customer customer) {
            return mapper.Map<CustomerViewModel>(customer);
        }

        public Customer MapDTO(CustomerViewModel customerView) {
            return mapper.Map<Customer>(customerView);
        }

        public List<CustomerViewModel> MapModelList(List<Customer> customers) {
            return mapper.Map<List<Customer>, List<CustomerViewModel>>(customers);
        }

        public List<Customer> MapDTOList(List<CustomerViewModel> customerViews) {
            return mapper.Map<List<CustomerViewModel>, List<Customer>>(customerViews);
        }

        //Invoice
        public InvoiceViewModel MapModel(Invoice invoice) {
            return mapper.Map<InvoiceViewModel>(invoice);
        }

        public Invoice MapDTO(InvoiceViewModel invoiceView) {
            return mapper.Map<Invoice>(invoiceView);
        }

        public List<InvoiceViewModel> MapModelList(List<Invoice> invoices) {
            return mapper.Map<List<Invoice>, List<InvoiceViewModel>>(invoices);
        }

        public List<Invoice> MapDTOList(List<InvoiceViewModel> invoiceViews) {
            return mapper.Map<List<InvoiceViewModel>, List<Invoice>>(invoiceViews);
        }

        //DetailLine
        public DetailLineViewModel MapModel(DetailLine detailLine) {
            return mapper.Map<DetailLineViewModel>(detailLine);
        }

        public DetailLine MapDTO(DetailLineViewModel detailLineView) {
            return mapper.Map<DetailLine>(detailLineView);
        }

        public List<DetailLineViewModel> MapModelList(List<DetailLine> detailLines) {
            return mapper.Map<List<DetailLine>, List<DetailLineViewModel>>(detailLines);
        }

        public List<DetailLine> MapDTOList(List<DetailLineViewModel> detailLineViews) {
            return mapper.Map<List<DetailLineViewModel>, List<DetailLine>>(detailLineViews);
        }

        //Product
        public ProductViewModel MapModel(Product product) {
            return mapper.Map<ProductViewModel>(product);
        }

        public Product MapDTO(ProductViewModel productView) {
            return mapper.Map<Product>(productView);
        }

        public List<ProductViewModel> MapModelList(List<Product> products) {
            return mapper.Map<List<Product>, List<ProductViewModel>>(products);
        }

        public List<Product> MapDTOList(List<ProductViewModel> productViews) {
            return mapper.Map<List<ProductViewModel>, List<Product>>(productViews);
        }

        //ApplicationUser
        public ApplicationUserViewModel MapModel(IdentityUser user) {
            return mapper.Map<ApplicationUserViewModel>(user);
        }

        public ApplicationUser MapDTO(ApplicationUserViewModel userView) {
            return mapper.Map<ApplicationUser>(userView);
        }

    }
}