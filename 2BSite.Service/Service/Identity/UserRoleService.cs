using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;
using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Model;
using Core.WebServices.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2BSite.Service.Service
{
    public class UserRoleService : BaseService<Identity.Database.Entities.UserRole, Identity.Database.IdentityDataContext, UserRoleDTO, int>, IUserRoleService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="mapper"></param>
        public UserRoleService(IRepository<Identity.Database.Entities.UserRole, Identity.Database.IdentityDataContext> Repository, IMapper mapper) : base(Repository, mapper)
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
