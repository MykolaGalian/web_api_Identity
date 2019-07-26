using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApiPr.Models;

namespace WebApiPr.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private IAuthenticationManager _authenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        private readonly IDTOUnitOfWork _unitOfWork;

        public ApplicationOAuthProvider(IDTOUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        //Validates the user object
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {       
            
            //GetClaim with BearerTokens
            ClaimsIdentity claim = await _unitOfWork.userManagerService.GetClaim(context.UserName, context.Password);

            DTOUser user = await _unitOfWork.userInfoService.GetUserByLogin(context.UserName);

            if (claim == null || user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            claim.AddClaim(new Claim("UserName", user.Login));

            if (await _unitOfWork.userManagerService.IsUserInRoleAdmin(user.Id))
            {
                claim.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            }
            //cancellation any claims identity associated the the caller
            _authenticationManager.SignOut();

            //grant a claims-based identity (token response) to the recipient of the response 
            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim); // claim with BearerTokens  
            context.Validated(claim);  

        }   
    }
}