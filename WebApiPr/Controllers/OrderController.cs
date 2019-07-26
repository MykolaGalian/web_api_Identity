using BLL.Contracts;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiPr.Controllers
{
    [Authorize(Roles = "admin,user")]
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        private IDTOUnitOfWork _unitOfWork;

        public OrderController(IDTOUnitOfWork uow)
        {
            _unitOfWork = uow;
        }


        // GET: api/Order/GetOrders - просмотр всех товаров - getOrdersAll
        [HttpGet]
        [Route("GetOrders")]
        public HttpResponseMessage GetOrders()
        {

            var orders = this._unitOfWork.orderServices.GetOrders();

            if (orders == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, orders);
        }

        // GET: api/Order/GetOrder/5 -  просмотр выбранного заказа - getOrderById
        [HttpGet]
        [Route("GetOrder/{id}")]
        public async Task<HttpResponseMessage> GetOrder([FromUri]int id)
        {
            var order = await _unitOfWork.orderServices.GetOrder(id);
            if (order == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, order);
        }

        // POST: api/Order/PostOrder  -создание заказа  - AddOrder
        [HttpPost]
        [Route("PostOrder")]
        public async Task<HttpResponseMessage> PostOrder([FromBody] OrderDTO orderDto)
        {

            if (ModelState.IsValid)
            {
                await _unitOfWork.orderServices.PostOrder(orderDto);

                return this.Request.CreateResponse(HttpStatusCode.Created, orderDto);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        //GET /api/order/{id}/products - просмотр списка товаров выбранного заказа - GetProductsByOrderId
        [HttpGet]
        [Route("api/order/{id}/products")]
        public async Task<HttpResponseMessage> GetProductsByOrderId(int id)
        {
            var order = await _unitOfWork.orderServices.GetProductsByOrderId(id);
            if (order == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, order);
        }


        //PUT /api/order/{Id}/products - добавление товара к выбранному заказу - AddProductsToOrderById
        [HttpPut]
        [Route("api/order/{id}/products")]
        public async Task<HttpResponseMessage> PutProductForOrder(int id, [FromBody] OrderDetailDTO orderDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (id != orderDetailDto.OrderId)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            await _unitOfWork.orderServices.PutProductForOrder(id, orderDetailDto);
            return this.Request.CreateResponse(HttpStatusCode.OK, orderDetailDto);
        }
    }
}
