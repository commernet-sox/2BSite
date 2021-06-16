using _2BSite.Service.DTO.WX;
using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Model;
using Core.WebServices.Service;
using System;
using System.Collections.Generic;
using System.Text;
using WXSite.Database;
using WXSite.Database.Entities;

namespace _2BSite.Service.Service.WX
{
    public class UserService : BaseService<User, WXContext, UserDTO, int>, Interface.WX.IUserService
    {
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="mapper"></param>
        public UserService(IRepository<User, WXContext> Repository, IMapper mapper,
           IServiceProvider serviceProvider) : base(Repository, mapper)
        {
            _serviceProvider = serviceProvider;
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
