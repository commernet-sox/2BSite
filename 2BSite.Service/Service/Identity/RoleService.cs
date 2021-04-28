using AutoMapper;
using Core.Database.Repository;
using Core.Infrastructure;
using Core.Infrastructure.DataTables;
using Core.WebServices.Model;
using Core.WebServices.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;

namespace _2BSite.Service.Service
{
    public class RoleService : BaseService<Identity.Database.Entities.Role, Identity.Database.IdentityDataContext, RoleDTO, int>, IRoleService
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="IRolePermissionService"></param>
        /// <param name="IPermissionService"></param>
        /// <param name="companyService"></param>
        /// <param name="mapper"></param>
        public RoleService(IRepository<Identity.Database.Entities.Role, Identity.Database.IdentityDataContext> Repository,
            IMapper mapper) : base(Repository, mapper)
        {
        }

        protected override CoreResponse Create(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Edit(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Remove(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Upload(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }
    }
}
