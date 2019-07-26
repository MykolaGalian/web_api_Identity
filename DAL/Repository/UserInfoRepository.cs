using DAL.Contract;
using DAL.EF;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserInfoRepository : IUserInfoRepository
    {

        private readonly ProdDBContext _db;

        public UserInfoRepository(ProdDBContext db)
        {
            _db = db;
        }

        public async Task DeleteUserInfo(int id)
        {
            var deleted = await SelectById(id);
            if (deleted != null)
                _db.Set<UserInfo>().Remove(deleted);
            else throw new ArgumentException("Not found object id.");
            await SaveUserInfo();
        }


        public async Task SaveUserInfo()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserInfo>> SelectAll()
        {
            return await _db.Set<UserInfo>().ToListAsync();
        }

        public async Task<IEnumerable<UserInfo>> SelectAll(Expression<Func<UserInfo, bool>> predicate)
        {
            return await _db.Set<UserInfo>().Where(predicate).ToListAsync();
        }

        public async Task<UserInfo> SelectById(int id)
        {
            return await _db.Set<UserInfo>().FindAsync(id);
        }

        public async Task UpdateUserInfo(UserInfo obj)
        {


            var local = _db.Set<UserInfo>().Local.FirstOrDefault(f => f.Id == obj.Id); 
            if (local != null)
            {
                _db.Entry(local).State = EntityState.Detached;
            }
            else
            {
                throw new ArgumentException("Not found");
            }

            _db.Entry(obj).State = EntityState.Modified;
            await SaveUserInfo();
        }       

    }
}
