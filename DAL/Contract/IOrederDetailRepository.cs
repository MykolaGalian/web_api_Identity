using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contract
{
    public interface IOrederDetailRepository
    {
        Task AddOrderDetail (OrderDetail orederDetail);
        Task<IEnumerable<OrderDetail>> SelectAll(Expression<Func<OrderDetail, bool>> predicate);
        Task SaveOrderDetail();

        Task DeleteOrederDetai(int? id);
        Task<OrderDetail> SelectById(int? id);
    }
}
