using _2BSite.Service.DTO.WX;
using _2BSite.Service.Interface.WX;
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
    public class FeedBackService : BaseService<FeedBack, WXContext, FeedBackDTO, int>, IFeedBackService
    {
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="mapper"></param>
        public FeedBackService(IRepository<FeedBack, WXContext> Repository, IMapper mapper,
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
