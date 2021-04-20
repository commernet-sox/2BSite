using _2BSite.Database;
using Core.Redis;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.UnitOfWork
{
    public class DemoDefaultVPD : IVPD<BSiteContext>
    {
        private IHttpContextAccessor _contentAccessor;
        private ICacheClient _cacheClient;

        public DemoDefaultVPD(IHttpContextAccessor contentAccessor, ICacheClient cacheClient)
        {
            _contentAccessor = contentAccessor;
            _cacheClient = cacheClient;
        }

        public void SetVPD(BSiteContext dbcotext)
        {
        }

    }
}
