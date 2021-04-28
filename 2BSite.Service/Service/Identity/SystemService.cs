using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Model;
using Core.WebServices.Service;

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Identity.Database.Entities;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;

namespace _2BSite.Service.Service
{
    public class SystemService : BaseService<Systems, Identity.Database.IdentityDataContext, SystemDTO, int>,ISystemService
    {
        
        public SystemService(IRepository<Identity.Database.Entities.Systems, Identity.Database.IdentityDataContext> Repository, IMapper mapper) : base(Repository, mapper)
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
