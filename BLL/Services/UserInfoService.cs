using AutoMapper;
using BLL.DTO;
using DAL.Contract;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Contracts;

namespace BLL.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IUnitOfWork _uow;
        public UserInfoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<DTOUser>> GetAllUserInfo()
        {          
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserInfo, DTOUser>()).CreateMapper();
            return mapper.Map<IEnumerable<UserInfo>, IEnumerable<DTOUser>>(await _uow.userInfoRepository.SelectAll());
        }

        public async Task Update(DTOUser user)
        {
            if (user != null && user.Id > 0  && user.Name.Length > 1)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOUser, UserInfo>()).CreateMapper();
                await _uow.userInfoRepository.UpdateUserInfo(mapper.Map<DTOUser, UserInfo>(user));
            }
            else throw new ArgumentException("Wrong user data");
        }

        public async Task Delete(int userId)
        {

            List<OrderDetail> orderDetail = new List<OrderDetail>();
            orderDetail.AddRange(await _uow.orederDetailRepository.SelectAll(x => x.UserInfoId == userId));
            foreach (var i in orderDetail)
            {
                await _uow.orederDetailRepository.DeleteOrederDetai(i.OrderDetailId);
            }

            await _uow.userInfoRepository.DeleteUserInfo(userId);

        }
             

        public async Task<bool> CheckLoginExist(string login)
        {

            return (await _uow.userInfoRepository.SelectAll(x => x.Login == login)).Any();
        }

        public async Task<DTOUser> GetUserById(int id)
        {
             var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserInfo, DTOUser>()).CreateMapper();
            return mapper.Map<UserInfo, DTOUser>(await _uow.userInfoRepository.SelectById(id));
        }

        public async Task<DTOUser> GetUserByLogin(string login)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserInfo, DTOUser>()).CreateMapper();
            return mapper.Map<UserInfo, DTOUser>((await _uow.userInfoRepository.SelectAll(x => x.Login == login)).FirstOrDefault());
        }

       


        public void Dispose()
        {
            _uow?.Dispose();
        }
    }
}
