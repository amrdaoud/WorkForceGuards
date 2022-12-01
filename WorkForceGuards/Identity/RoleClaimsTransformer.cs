using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Identity;

namespace WorkForceManagementV0.Identity
{
    public class RoleClaimsTransformer: IClaimsTransformation
    {
        private readonly IConfiguration _config;
        
        public RoleClaimsTransformer(IConfiguration config)
        {
            _config = config;
        }
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            
            var adminGroups = _config.GetValue<string>("WindowsToRoles:Groups:Admin");
            var userGroups = _config.GetValue<string>("WindowsToRoles:Groups:User");
            var hosGroups = _config.GetValue<string>("WindowsToRoles:Groups:Hos");
            var superGroups = _config.GetValue<string>("SuperUserWindows:Groups");

            var adminUsers = _config.GetValue<string>("WindowsToRoles:Users:Admin");
            var userUsers = _config.GetValue<string>("WindowsToRoles:Users:User");
            var hosUsers = _config.GetValue<string>("WindowsToRoles:Users:Hos");
            var superUsers = _config.GetValue<string>("SuperUserWindows:Users");

            var userIdentity = (ClaimsIdentity)principal.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(x =>
                new System.Security.Principal.SecurityIdentifier(x.Value).Translate(
                typeof(System.Security.Principal.NTAccount)).ToString().ToLower()
                ).ToList();
            var slashIndex = userIdentity.Name.IndexOf("\\");
            var userName = slashIndex > -1 ? userIdentity.Name.Substring(slashIndex + 1) : userIdentity.Name.Substring(0, userIdentity.Name.IndexOf("@"));
            if(adminGroups.Split(',').FirstOrDefault(x => roles.Contains(x.Trim().ToLower())) != null ||
                adminUsers.Split(',').FirstOrDefault(x => userName.ToLower() == x.Trim().ToLower()) != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(
               new Claim("SystemRole", "Admin"));
            }
            if(userGroups.Split(',').FirstOrDefault(x => roles.Contains(x.Trim().ToLower())) != null ||
                userUsers.Split(',').FirstOrDefault(x => userName.ToLower() == x.Trim().ToLower()) != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(
               new Claim("SystemRole", "User"));
            }
            if(hosGroups.Split(',').FirstOrDefault(x => roles.Contains(x.Trim().ToLower())) != null ||
                hosUsers.Split(',').FirstOrDefault(x => userName.ToLower() == x.Trim().ToLower()) != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(
               new Claim("SystemRole", "Hos"));
            }
            if (superGroups.Split(',').FirstOrDefault(x => roles.Contains(x.Trim().ToLower())) != null ||
                superUsers.Split(',').FirstOrDefault(x => userName.ToLower() == x.Trim().ToLower()) != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(
               new Claim("SystemRole", "SuperUser"));
            }

            return Task.FromResult(principal);
        }
    }
}
