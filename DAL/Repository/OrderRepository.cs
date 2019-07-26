using DAL.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using DAL.EF;

namespace DAL.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ProdDBContext _db;

        public OrderRepository(ProdDBContext db)
        {
            _db = db;
        }

        public IQueryable<Order> GetOrdersAll()
        {
            // Include - загрузка связанных сущностей
            return _db.Orders_.Include("OrderDetails.Product");
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _db.Orders_.Include("OrderDetails.Product").FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> GetOrderByIdForOrderDetail(OrderDetail orderDetail)
        {
            return await _db.Orders_.FirstOrDefaultAsync(o => o.OrderId == orderDetail.OrderId);
        }

        public async Task AddOrder(Order order)
        {
            _db.Orders_.Add(order);
            await SaveOrder();
        }

        public async Task SaveOrder()
        {
            await _db.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var deleted = await GetOrderById(id);
            if (deleted != null)
                _db.Set<Order>().Remove(deleted);
            else throw new ArgumentException("Not found object id.");
            await SaveOrder();
        }

        
    }
}
