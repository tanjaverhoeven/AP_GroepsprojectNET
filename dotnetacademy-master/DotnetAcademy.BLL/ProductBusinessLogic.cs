using System;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL;

namespace DotnetAcademy.BLL {
    public class ProductBusinessLogic : IProductBusinessLogic<ProductViewModel> {
        private IUnitOfWork _uow;
        private Mapper mapper = new Mapper();

        public ProductBusinessLogic(UnitOfWork uow) {
            _uow = uow;
        }

        public void Create(ProductViewModel productDTO) {
            try {
                Product product = mapper.MapDTO(productDTO);
                _uow.ProductRepository.Create(product);

                _uow.SaveChanges();
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
            }
        }

        public void Update(ProductViewModel productDTO) {
            try {
                Product product = mapper.MapDTO(productDTO);
                _uow.ProductRepository.Update(product);

                _uow.SaveChanges();
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
            }
        }

        public void Delete(ProductViewModel productDTO) {
            try {
                Product product = _uow.ProductRepository.FindById(productDTO.Id);
                product.IsActive = false;

                _uow.ProductRepository.Update(product);
                _uow.SaveChanges();
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
            }
        }

        public ProductViewModel FindById(int? id) {
            try {
                Product product = _uow.ProductRepository.FindById(id);
                ProductViewModel productDTO = mapper.MapModel(product);
                return productDTO;
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
                throw;
            }
        }

        public List<ProductViewModel> GetAll() {
            try {
                List<Product> products = _uow.ProductRepository.GetAll().Where(i => i.IsActive).ToList();
                List<ProductViewModel> productDTOs = mapper.MapModelList(products);
                return productDTOs;
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
                throw;
            }
        }

        public void SetInactive(int? id) {
            try {
                Product product = _uow.ProductRepository.FindById(id);
                product.IsActive = false;
                _uow.ProductRepository.Update(product);

                _uow.SaveChanges();
            } catch (Exception ex) {
                new LogWriter(ex.ToString());
            }
        }
    }
}