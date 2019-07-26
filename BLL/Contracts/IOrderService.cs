using BLL.DTO;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IOrderService
    {
        IQueryable<OrderDTO> GetOrders();
        Task<OrderDTO> GetOrder(int id);
        Task PostOrder(OrderDTO orderDto);
        Task PutProductForOrder(int id, OrderDetailDTO orderDetailDto);
        Task<OrderDTO> GetProductsByOrderId(int id);
    }
}
