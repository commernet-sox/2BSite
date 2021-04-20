using _2BSite.Service.Model;
using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Model;
using Core.WebServices.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2BSite.Service.Service
{
    public class MasterDetailService<TEntity, TDbContext, DTO, Tkey> : BaseService<TEntity, TDbContext, DTO, Tkey>
        where TEntity : class
        where TDbContext : DbContext
        where DTO : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="mapper"></param>
        public MasterDetailService(IRepository<TEntity, TDbContext> Repository, IMapper mapper) : base(Repository, mapper)
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

        public virtual Seed GetSequenceNumber(string sequenceName)
        {
            var seed = base.Repository.MasterUnitOfWork.ExecuteQuery<Seed>($"SELECT NEXT VALUE FOR [{sequenceName}] seed").FirstOrDefault();
            return seed;
        }

    }
}
