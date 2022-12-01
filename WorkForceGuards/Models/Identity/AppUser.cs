using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Identity
{
    public class AppUser
    {
        public string UserName { get; set; }
        public string Alias { get; set; }
        public ICollection<string> Roles { get; set; }
        public string AvatarImgUrl { get; set; }
        public string LargeImgUrl { get; set; }
    }
    public class RoleMapping
    {
        public string Name { get; set; }
        public List<string> WindowsGroups { get; set; }
    }
}
