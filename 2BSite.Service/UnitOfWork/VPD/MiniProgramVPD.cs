using _2BSite.Database;
using Core.Redis;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WXSite.Database;

namespace _2BSite.Service.UnitOfWork.VPD
{
    /// <summary>
    /// 小程序VPD设置
    /// </summary>
    public class MiniProgramVPD : IVPD<WXContext>
    {

        private IHttpContextAccessor _contentAccessor;
        private ICacheClient _cacheClient;

        public MiniProgramVPD(IHttpContextAccessor contentAccessor, ICacheClient cacheClient)
        {
            _contentAccessor = contentAccessor;
            _cacheClient = cacheClient;
        }

        public void SetVPD(WXContext dbcotext)
        {
        }
    }
}
