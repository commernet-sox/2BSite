using _2BSite.Database;
using Core.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.UnitOfWork
{
    public class DemoDatabseUnitofwork : UnitOfWork<BSiteContext>
    {
        public DemoDatabseUnitofwork(ILoggerFactory logger, IVPD<BSiteContext> virtualPrivateDatabase)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BSiteContext>();
            optionsBuilder.UseLoggerFactory(logger);
            optionsBuilder.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(optionsBuilder.Options.ContextType.ToString()));//默认设置为主，查询的时候会自动转到从
            optionsBuilder.UseNoLockSqlGenerator();
            base.DbContext = new BSiteContext(optionsBuilder.Options);
            virtualPrivateDatabase?.SetVPD(base.DbContext);
        }
    }
}
