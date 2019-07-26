using AutoMapper;
using BLL.Contracts;
using BLL.DTO;
using BLL.Infostructure;
using DAL.Contract;
using DAL.Identity;
using DAL.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserManagerService : IUserManagerService
    {

        private readonly IUnitOfWork _uow;
        public UserManagerService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<string> CheckUserByLoginPas(DTOUser dtouser)
        {
            ApplicationUser user = await _uow.UserManager.FindAsync(dtouser.Login, dtouser.Password); 

            return user.UserInfo.Login;
        }

        public async Task<OperationDetails> Create(DTOUser dtouser)
        {
            ApplicationUser user = await _uow.UserManager.FindByEmailAsync(dtouser.Email); //check email
            if (user == null)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOUser, ApplicationUser>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(y => y.Login))
                .ForMember(x => x.Roles, opt => opt.Ignore())).CreateMapper(); 
                
                user = mapper.Map<DTOUser, ApplicationUser>(dtouser);

                var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<DTOUser, UserInfo>()).CreateMapper();  
                
                user.UserInfo = mapper2.Map<DTOUser, UserInfo>(dtouser);

                var result = await _uow.UserManager.CreateAsync(user, dtouser.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await _uow.UserManager.AddToRoleAsync(user.Id, dtouser.Roles[0]);   // only User type "user"
                await _uow.Save();
                return new OperationDetails(true, "Registration success", "");
            }
            else
            {
                return new OperationDetails(false, "User with this email exist", "email");
            }
        }

        public async Task<ClaimsIdentity> GetClaim(string username, string password)
        {
            var user = await _uow.UserManager.FindAsync(username, password);

            if (user != null)
            {
                ClaimsIdentity claim = await _uow.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ExternalBearer); //UseOAuthBearerTokens method for DefaultAuthentication
                return claim;
            }
            else return null;
        }

        public async Task<DTOUser> GetUserByLogin(string login)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, DTOUser>()).CreateMapper();
            return mapper.Map<ApplicationUser, DTOUser>(await _uow.UserManager.Users.FirstOrDefaultAsync(x => x.UserName == login));
        }

        public async Task<bool> IsUserInRoleAdmin(int userId)
        {
            return await _uow.UserManager.IsInRoleAsync(userId, "admin");
        }
    }
}
