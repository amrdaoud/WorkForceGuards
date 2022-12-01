using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WorkForceManagementV0.Models.Identity;
namespace WorkForceManagementV0.Repositories.Identity
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public AppUser GetUserInfo(ClaimsPrincipal user)
        {
            if(user != null && user.Identity.IsAuthenticated)
            {
                var h = _httpContextAccessor.HttpContext.Request.Headers;
                var userIdentity = (ClaimsIdentity)user.Identity;
                var claims = userIdentity.Claims;
                var roleClaimType = userIdentity.RoleClaimType;
                var roles = claims.Where(c => c.Type == "SystemRole").Select(x => x.Value).ToList();
                var appUser = new AppUser();
                appUser.Roles = roles;
                appUser.UserName = user.Identity.Name;
                var slashIndex = appUser.UserName.IndexOf("\\");
                if(appUser.Roles.Contains("SuperUser") && h.TryGetValue("pretend", out var pretendName))
                {
                    if (!string.IsNullOrEmpty(pretendName))
                    {
                        appUser.Alias = pretendName;
                    }
                }
                else
                {
                    appUser.Alias = slashIndex > -1 ? appUser.UserName.Substring(slashIndex + 1) : appUser.UserName.Substring(0, appUser.UserName.IndexOf("@"));
                }
                appUser.AvatarImgUrl = $@"http://mysites/User%20Photos/Profile%20Pictures/{appUser.Alias.ToLower()}_SThumb.jpg";
                appUser.LargeImgUrl = $@"http://mysites/User%20Photos/Profile%20Pictures/{appUser.Alias.ToLower()}_LThumb.jpg";
                return appUser;
            }
            return null;
        }
        public string getAvatar(string alias)
        {
            return $@"http://mysites/User%20Photos/Profile%20Pictures/{alias.ToLower()}_SThumb.jpg";
        }
    }
}
