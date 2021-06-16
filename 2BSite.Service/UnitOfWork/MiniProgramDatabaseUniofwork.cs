using Core.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WXSite.Database;

namespace _2BSite.Service.UnitOfWork
{
    public class MiniProgramDatabaseUniofwork : UnitOfWork<WXContext>
    {
        public MiniProgramDatabaseUniofwork(ILoggerFactory logger, IVPD<WXContext> virtualPrivateDatabase)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WXContext>();
            optionsBuilder.UseLoggerFactory(logger);
            optionsBuilder.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(optionsBuilder.Options.ContextType.ToString()));//默认设置为主，查询的时候会自动转到从
            //optionsBuilder.EnableSensitiveDataLogging();

            optionsBuilder.UseNoLockSqlGenerator();
            base.DbContext = new WXContext(optionsBuilder.Options);
            virtualPrivateDatabase?.SetVPD(base.DbContext);
        }
    }
}
