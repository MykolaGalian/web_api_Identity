using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiPr.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/admins")]
    public class AdminController : ApiController
    {
        private readonly IDTOUnitOfWork _uow;

        public AdminController(IDTOUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet]
        [Route("block/user/{accountLogin}")]
        public async Task<IHttpActionResult> BlockAccount([FromUri]string accountLogin)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            DTOUser user = await _uow.userManagerService.GetUserByLogin(accountLogin);
            if (user == null)
                return NotFound();

            if (user.IsBlocked)
                return BadRequest("User already blocked.");

            var userId = this.User.Identity.GetUserId();
            if (userId == null || !User.IsInRole("admin"))
                return this.Unauthorized();

            var admin = await _uow.userInfoService.GetUserById(User.Identity.GetUserId<int>());
            if (admin != null)
                await _uow.adminService.BlockAccount(user.Id, admin.Login);
            else return NotFound();

            return Ok("Account blocked");
        }


        [HttpGet]
        [Route("unblock/user/{accountLogin}")]
        public async Task<IHttpActionResult> UnblockAccount([FromUri]string accountLogin)
        {
            DTOUser user = await _uow.userInfoService.GetUserByLogin(accountLogin);
            if (user == null)
                return NotFound();
            if (!user.IsBlocked)
                return BadRequest("User is not blocked.");

            var userId = this.User.Identity.GetUserId();
            if (userId == null || !User.IsInRole("admin"))
                return this.Unauthorized();

            await _uow.adminService.UnblockAccount(user.Id);
            return Ok("Account unblocked");
        }
    }
}
