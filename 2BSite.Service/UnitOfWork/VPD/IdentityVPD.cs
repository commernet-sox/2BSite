using _2BSite.Database;
using _2BSite.Service.DTO.Identity;
using Core.Database.Extension;
using Core.Database.Repository;
using Core.Redis;
using Identity.Database;
using Identity.Database.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace _2BSite.Service.UnitOfWork.VPD
{
    /// <summary>
    /// 权限VPD
    /// </summary>
    public class IdentityVPD : IVPD<IdentityDataContext>
    {
        private IHttpContextAccessor _contentAccessor;
        private ICacheClient _cacheClient;
        private IUnitOfWork<BSiteContext> _biDatabaseUniofwork;

        public IdentityVPD(IHttpContextAccessor contentAccessor, ICacheClient cacheClient, IUnitOfWork<BSiteContext> biDatabaseUniofwork)
        {
            _contentAccessor = contentAccessor;
            _cacheClient = cacheClient;
            _biDatabaseUniofwork = biDatabaseUniofwork;
        }

        public void SetVPD(IdentityDataContext dbContext)
        {
            //string json = _contentAccessor.HttpContext.Session.GetString("User");
            //UserDTO usetdto = null;
            //if (!string.IsNullOrEmpty(json))
            //{
            //    usetdto = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(json);
            //}
            //else
            //{
            //    try
            //    {
            //        var authenticateInfo = await AuthenticationHttpContextExtensions.AuthenticateAsync(_contentAccessor.HttpContext, "Bearer");
            //        if (authenticateInfo.Principal != null)
            //        {
            //            var userClaim = authenticateInfo.Principal.FindFirst(ClaimTypes.NameIdentifier);
            //            if (userClaim != null)
            //                usetdto = _cacheClient.Get<UserDTO>("jwt:" + userClaim.Value);
            //        }
            //    }
            //    catch
            //    {
            //        return;
            //    }
            //}

            //if (usetdto != null)
            //{
            //    QueryFilterManager.Filter<Permission>("bi_global" + "_Permission", q => q.Where(x => x.SystemID == 2));
            //    QueryFilterManager.Filter<Role>("bi_global" + "_Role", q => q.Where(x => x.SystemID == 2));
            //    QueryFilterManager.Filter<Identity.Database.Entities.Systems>("bi_global" + "_System", q => q.Where(x => x.Id == 2));
            //    QueryFilterManager.InitilizeGlobalFilter(dbContext, p => (p.Key.ToString().StartsWith("bi_global_")));
            //    dbContext.AuditCreateBy = usetdto.LoginName;
            //    if (usetdto.IsAdmin)
            //        return;
            //    string key = "global";//这个用于数据权限的区分，目前统一都是一个数据权限  SystemId == 2   BI系统
            //    if (!QueryFilterManager.GlobalFilters.ContainsKey(key + "_User"))
            //    {

            //        QueryFilterManager.Filter<User>(key + "_User", q => (from user in q
            //                                                             join usersystem in dbContext.UserSystems on user.Id equals usersystem.UserId
            //                                                             where usersystem.SystemId == 2
            //                                                             select user).Distinct());
            //    }

            //    //初始化当前部门的VPD数据，并且ENABLE
            //    QueryFilterManager.InitilizeGlobalFilter(dbContext, p => (p.Key.ToString().StartsWith(key + "_")));

            //}
        }
    }
}
