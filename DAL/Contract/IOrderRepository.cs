using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contract
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetOrdersAll();
        Task<Order> GetOrderById(int OrderId);
        Task<Order> GetOrderByIdForOrderDetail(OrderDetail orderDetail);
        Task AddOrder(Order order);
        Task SaveOrder();

        Task DeleteOrder(int id);
    }
}
