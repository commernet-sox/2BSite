using _2BSite.Database.Entities;
using _2BSite.Service.DTO;
using _2BSite.Service.DTO.Identity;
using Identity.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.Profile
{
    public class DTOProfile : AutoMapper.Profile
    {
        public DTOProfile() : base()
        {
            #region Identity

            CreateMap<PermissionDTO, Permission>();
            CreateMap<Permission, PermissionDTO>();

            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();

            CreateMap<RolePermissionDTO, RolePermission>();
            CreateMap<RolePermission, RolePermissionDTO>();

            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<UserRoleDTO, UserRole>();
            CreateMap<UserRole, UserRoleDTO>();


            CreateMap<UserSystemDTO, UserSystem>();
            CreateMap<UserSystem, UserSystemDTO>();

            CreateMap<SystemDTO, Systems>();
            CreateMap<Systems, SystemDTO>();
            #endregion

            #region WX

            #endregion

            CreateMap<CodeMasterDTO, CodeMaster>();
            CreateMap<CodeMaster, CodeMasterDTO>();
        }
    }
}
