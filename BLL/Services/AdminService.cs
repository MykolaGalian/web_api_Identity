using BLL.Contracts;
using DAL.Contract;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    class AdminService : IAdminService
    {
        private readonly IUnitOfWork _uow;
        public AdminService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task BlockAccount(int userId, string aLogin)
        {
            if (userId > 0 && aLogin != null && aLogin.Length > 3)
            {
                UserInfo user = await _uow.userInfoRepository.SelectById(userId);
                user.IsBlocked = true;
                await _uow.userInfoRepository.UpdateUserInfo(user);
            }
            else throw new ArgumentException("Wrong data");
        }

        public async Task UnblockAccount(int userId)
        {
            if (userId > 0)
            {
                UserInfo user = await _uow.userInfoRepository.SelectById(userId);
                user.IsBlocked = false;
                await _uow.userInfoRepository.UpdateUserInfo(user);
            }
            else throw new ArgumentException("Wrong data");
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }
    }
}
