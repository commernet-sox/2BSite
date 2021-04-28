using Core.WebServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Identity.Database.Entities;
using _2BSite.Service.DTO.Identity;

namespace _2BSite.Service.Interface.Identity
{
    public interface IRoleService : IBase<Role, RoleDTO, int>, IDatatable
    {
    }
}
