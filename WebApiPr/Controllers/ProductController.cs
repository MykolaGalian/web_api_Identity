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
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        private IDTOUnitOfWork _unitOfWork;

        public ProductController(IDTOUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        // GET: api/Product/GetProducts - просмотр всех товаров - getProductsAll
        [HttpGet]
        [Route("GetProducts")]
        public HttpResponseMessage GetProducts_()
        {
            var prod = _unitOfWork.productServices.GetProducts();

            if (prod == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, prod);
        }

        // GET: api/Product/GetProduct/5  - просмотр выбранного товара  - getProductById
        [HttpGet]
        [Route("GetProduct/{id}")]
        public async Task<HttpResponseMessage> GetProduct_([FromUri]int id)
        {
            var prod = await _unitOfWork.productServices.GetProduct(id);

            if (prod == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, prod);
        }


        // POST: api/Product/PostProduct -добавление товара в систему - AddProduct
        [HttpPost]
        [Route("PostProduct")]
        public async Task<HttpResponseMessage> PostProduct_([FromBody]ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.productServices.PostProduct(product);

                return this.Request.CreateResponse(HttpStatusCode.Created, product);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
