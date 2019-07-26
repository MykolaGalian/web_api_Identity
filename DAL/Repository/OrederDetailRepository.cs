using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Contract;
using DAL.EF;
using DAL.Model;

namespace DAL.Repository
{
    public class OrederDetailRepository : IOrederDetailRepository
    {
        private readonly ProdDBContext _db;

        public OrederDetailRepository(ProdDBContext db)
        {
            _db = db;
        }
        public async Task AddOrderDetail(OrderDetail orederDetail)
        {
            _db.OrderDetails_.Add(orederDetail);
            await SaveOrderDetail();
        }

        public async Task SaveOrderDetail()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetail>> SelectAll(Expression<Func<OrderDetail, bool>> predicate)
        {
            return await _db.Set<OrderDetail>().Where(predicate).ToListAsync();
        }

        public async Task DeleteOrederDetai(int? id)
        {
            var deleted = await SelectById(id);
            if (deleted != null)
                _db.Set<OrderDetail>().Remove(deleted);
            else throw new ArgumentException("Not found object id.");
            await SaveOrderDetail();
        }

        public async Task<OrderDetail> SelectById(int? id)
        {
            return await _db.Set<OrderDetail>().FindAsync(id);
        }
    }
}
