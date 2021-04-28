using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2BSite.Models
{
    public class MenuModel
    {
        public List<UserPermission> UserPermissions { get; set; }

        public UserDTO User { get; set; }
    }
}
