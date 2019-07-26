using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IUserInfoService
    {
        Task<IEnumerable<DTOUser>> GetAllUserInfo();
        Task Update(DTOUser user);        
        Task Delete(int userId);
        Task<bool> CheckLoginExist(string login);
        Task<DTOUser> GetUserById(int id);
        Task<DTOUser> GetUserByLogin(string login);
       
        
    }
}
