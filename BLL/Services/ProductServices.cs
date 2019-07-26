using BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using DAL.Model;
using DAL.Contract;

namespace BLL.Services
{
    public class ProductServices : IProductService
    {
        private IUnitOfWork _uow;

        public ProductServices(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public List<ProductDTO> GetProducts()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            return mapper.Map<List<Product>, List<ProductDTO>>(_uow.productRepository.GetProductsAll().ToList());
        }

        public async Task<ProductDTO> GetProduct(int ProdId)
        {
            var pr = await _uow.productRepository.GetProductById(ProdId);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            return mapper.Map<Product, ProductDTO>(pr);
        }

        public async Task PostProduct(ProductDTO product)
        {

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, Product>()).CreateMapper();
            var prod = mapper.Map<ProductDTO, Product>(product);

            await _uow.productRepository.AddProduct(prod);
        }
    }
}
