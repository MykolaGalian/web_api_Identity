using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contract;
using DAL.EF;
using DAL.Model;

namespace DAL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProdDBContext _db;

        public ProductRepository(ProdDBContext db)
        {
            _db = db;
        }
        public  IQueryable<Product> GetProductsAll()
        {
            return  _db.Products_;
        }

        public async Task<Product> GetProductById(int ProdId)
        {
            return await _db.Products_.FirstOrDefaultAsync(p => p.ProductId == ProdId); //
        }

        public async Task<Product> GetProductByIdForOrderDetail(OrderDetail orderDetail)
        {
            return await _db.Products_.FirstOrDefaultAsync(o => o.ProductId == orderDetail.ProductId);
        }

        public async Task AddProduct(Product product)
        {
             _db.Products_.Add(product);
            await SaveProduct();
        }

        public async Task SaveProduct()
        {
            await _db.SaveChangesAsync();
        }
    }
}
