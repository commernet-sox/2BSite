using Core.Database.Repository;
using Identity.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.UnitOfWork
{
    public class IdentityDatabaseUniofwork : UnitOfWork<IdentityDataContext>
    {
        public IdentityDatabaseUniofwork(ILoggerFactory logger, IVPD<IdentityDataContext> virtualPrivateDatabase)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDataContext>();
            optionsBuilder.UseLoggerFactory(logger);
            optionsBuilder.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(optionsBuilder.Options.ContextType.ToString()));//默认设置为主，查询的时候会自动转到从
            //optionsBuilder.EnableSensitiveDataLogging();

            optionsBuilder.UseNoLockSqlGenerator();
            base.DbContext = new IdentityDataContext(optionsBuilder.Options);
            virtualPrivateDatabase?.SetVPD(base.DbContext);
        }
    }
}
