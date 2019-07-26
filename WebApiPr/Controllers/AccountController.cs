using AutoMapper;
using BLL.Contracts;
using BLL.DTO;
using BLL.Infostructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApiPr.Models.account;

namespace WebApiPr.Controllers
{
    [Authorize]
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {

        private IAuthenticationManager _authenticationManager
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        private readonly IDTOUnitOfWork _uow;

        public AccountController(IDTOUnitOfWork uow)
        {
            _uow = uow;
        }


        //Post api/accounts/CheckLogin
        [HttpPost]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]        
        [Route("CheckLogin")]
        public async Task<IHttpActionResult> CheckLogin([FromBody]LoginViewModel model)   // only check login/password
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return BadRequest("Please first logout");
            }

            DTOUser user = new DTOUser { Login = model.Login, Password = model.Password};
            string userLogin = await _uow.userManagerService.CheckUserByLoginPas(user);

            if (userLogin == null)
            {
                ModelState.AddModelError("Login", "Wrong login.");
                ModelState.AddModelError("Password", "Wrong password.");
            }

            if (ModelState.IsValid)
            {
                return this.Ok(userLogin.ToString() + " login/Password ok.");
            }
            else return BadRequest(ModelState);

        }


      
        [HttpGet]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            //cancellation any claims identity associated the the caller
            _authenticationManager.SignOut();
            return Ok(User.Identity.Name + " logged out");
        }


        // Post api/accounts/Register
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest(User.Identity.Name + " already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data.");
            }

            if (await _uow.userInfoService.CheckLoginExist(model.Login))
            {
                ModelState.AddModelError("Login", "Login is already taken.");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RegisterViewModel, DTOUser>()).CreateMapper();
            DTOUser user = mapper.Map<RegisterViewModel, DTOUser>(model);
            user.Roles = new List<string> { "user" };  // only User type
            user.DateRegistration = DateTime.Now;

            OperationDetails operationDetails = await _uow.userManagerService.Create(user);
            if (!operationDetails.Success)
            {
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                return this.BadRequest(operationDetails.Message);
            }
            else
            {
                return this.Ok(user.Login + " user registered ok.");
            }
        }

    }

    
}
