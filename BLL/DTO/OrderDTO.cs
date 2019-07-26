using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL.DTO
{
    public class OrderDTO
    {
        
        public class Detail
        {
            public int ProductId { get; set; }
            public int OdrerId { get; set; }

            public int UserInfoId { get; set; } //
            public string Product { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
            public DateTime OrderDate { get; set; }

        }
        public IEnumerable<Detail> Details { get; set; }
    }
}