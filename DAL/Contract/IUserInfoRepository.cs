using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contract
{
    public interface IUserInfoRepository
    {

        Task<IEnumerable<UserInfo>> SelectAll();
        Task<IEnumerable<UserInfo>> SelectAll(Expression<Func<UserInfo, bool>> predicate);
        Task<UserInfo> SelectById(int id);        
        Task UpdateUserInfo(UserInfo obj);
        Task DeleteUserInfo(int id);
        Task SaveUserInfo();
        
    }
}
