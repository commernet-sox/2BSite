using _2BSite.Service.DTO.Identity;
using Core.WebServices.Interface;
using Identity.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.Interface.Identity
{
    public interface IUserRoleService : IBase<UserRole, UserRoleDTO, int>, IDatatable
    {
    }
}
