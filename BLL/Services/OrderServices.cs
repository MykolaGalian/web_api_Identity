using DAL.Contract;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Model;
using BLL.DTO;
using AutoMapper;
using System.Data.Entity.Infrastructure;

namespace BLL.Services
{
    public class OrderServices : IOrderService
    {
        private IUnitOfWork _uow;

        public OrderServices(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<OrderDTO> GetOrders()
        {
            var orders = _uow.orderRepository.GetOrdersAll();
        
            var or = from o in orders
                select new OrderDTO()
                {
                    Details = from d in o.OrderDetails
                        select new OrderDTO.Detail()
                        {
                            OdrerId = o.OrderId,
                            OrderDate = d.Order.OrderDate,
                            ProductId = d.Product.ProductId,
                            Product = d.Product.ProductName,
                            Price = d.Product.ProductPrice,
                            Quantity = d.Quantity,
                            UserInfoId = d.UserInfoId
                        }
                };
            return or;
        }

        public async Task<OrderDTO> GetOrder(int id)
        {
            var order =  await _uow.orderRepository.GetOrderById(id);
        
            OrderDTO or = new OrderDTO()
            {
                Details = from d in order.OrderDetails
                    select new OrderDTO.Detail()
                    {
                        OdrerId = order.OrderId,
                        OrderDate = d.Order.OrderDate,
                        ProductId = d.Product.ProductId,
                        Product = d.Product.ProductName,
                        Price = d.Product.ProductPrice,
                        Quantity = d.Quantity,
                        UserInfoId = d.UserInfoId
                    }
            };
            return or;
        }

        public async Task PostOrder(OrderDTO orderDto)
        {
            DateTime dt = DateTime.Now;
            var order = new Order()
            {
                OrderDate = dt.Date,
                OrderDetails = (
                        from item in orderDto.Details
                        select new OrderDetail() {ProductId = item.ProductId, Quantity = item.Quantity})
                    .ToList()
            };
            await _uow.orderRepository.AddOrder(order);
        }

        public async Task<OrderDTO> GetProductsByOrderId(int id)
        {
            var order = await _uow.orderRepository.GetOrderById(id);

            OrderDTO or = new OrderDTO()
            {
                Details = from d in order.OrderDetails
                    select new OrderDTO.Detail()
                    {
                        ProductId = d.Product.ProductId,
                        Product = d.Product.ProductName,
                        Price = d.Product.ProductPrice,
                        Quantity = d.Quantity,                       
                        UserInfoId = d.UserInfoId
                    }
            };
            return or;
        }
        public async Task PutProductForOrder(int id, OrderDetailDTO orderDetailDto)
        {

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetailDTO, OrderDetail>()).CreateMapper();
            var ordet = mapper.Map<OrderDetailDTO, OrderDetail>(orderDetailDto);
            
            
            var ord = new OrderDetail()
            {
                ProductId = ordet.ProductId,
                OrderId = ordet.OrderId,
                Quantity = ordet.Quantity,
                Product = await _uow.productRepository.GetProductByIdForOrderDetail(ordet),
                Order = await _uow.orderRepository.GetOrderByIdForOrderDetail(ordet),
                UserInfoId = ordet.UserInfoId
            };

            await _uow.orederDetailRepository.AddOrderDetail(ord);
            
        }
        
    }
}
