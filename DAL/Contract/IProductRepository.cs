using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contract
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProductsAll();
        Task<Product> GetProductById(int ProdId);
        Task<Product> GetProductByIdForOrderDetail(OrderDetail orderDetail);
        Task AddProduct(Product product);  
        Task SaveProduct();
    }
}
