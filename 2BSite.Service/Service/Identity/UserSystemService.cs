using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;
using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Model;
using Core.WebServices.Service;
using System;

namespace _2BSite.Service.Service
{
	public class UserSystemService : BaseService<Identity.Database.Entities.UserSystem, Identity.Database.IdentityDataContext, UserSystemDTO, int>, IUserSystemService
    {
        
        public UserSystemService (IRepository<Identity.Database.Entities.UserSystem, Identity.Database.IdentityDataContext> Repository, IMapper mapper) : base(Repository, mapper)
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
