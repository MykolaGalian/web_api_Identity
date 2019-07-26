using BLL.DTO;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IProductService
    {
        List<ProductDTO> GetProducts();
        Task<ProductDTO> GetProduct(int id);
        Task PostProduct(ProductDTO product);
    }
}
