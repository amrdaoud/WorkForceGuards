using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class User_Service:IUser_Service

    {
        //private readonly ApplicationDbContext _db;
            
        //public User_Service(ApplicationDbContext db)
        //{
        //    _db = db;
        //}
        //public DataWithError GetUserInfo (string User)
        // {
        //    var data = new DataWithError();
        //    var appUser = new AppUser();
        //    if ((User != null) && (User.Identity.IsAuthenticated))
        //    {
        //        RolePrincipal rp = (RolePrincipal)User;
        //        String[] Roles = rp.GetRoles();
        //        var appUser = new Appuser();
        //        appUser.UserName = User.Identity.Name;
        //        appUser.Alias = slashIndex > -1 ? appUser.UserName.Substring(slashIndex + 1)
        //            : appUser.UserName.Substring(0, appUser.UserName.IndexOf("@"));
        //        appUser.Roles = Roles;
        //        appUser.AvatarImgUrl = $@"http://mysites/User%20photos/profile%20pictures/{appUser.Alias.ToLower()} SThumb.jpg";
        //        appUser.LargeImgUrl = $@"http://mysites/User%20photos/profile%20pictures/{appUser.Alias.ToLower()} LThumb.jpg";

        //        data.Result = appUser;
        //        data.ErrorMessage = null;
        //        return data;
        //    }
        //    data.Result = null;
        //    data.ErrorMessage = "your are not authorizes!";
        //    return data;
        // }
    }
}

