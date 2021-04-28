
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;
using _2BSite.Service.Model;

namespace _2BSite.Middleware
{
    /// <summary>
    /// 权限中间件
    /// </summary>
    public class PermissionMiddleware
    {
        /// <summary>
        /// 管道代理对象
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 权限中间件的配置选项
        /// </summary>
        private readonly PermissionMiddlewareOption _option;

        /// <summary>
        /// 权限中间件构造
        /// </summary>
        /// <param name="next">管道代理对象</param>
        /// <param name="permissionResitory">权限仓储对象</param>
        /// <param name="option">权限中间件配置选项</param>
        public PermissionMiddleware(RequestDelegate next, PermissionMiddlewareOption option)
        {
            _option = option;
            _next = next;
        }

        /// <summary>
        /// 调用管道
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext context, IServiceProvider provider)
        {
            //请求Url
            var host = (context.Request.IsHttps ? "https://" : "http://") + context.Request.Host.Value+ context.Request.PathBase.Value;
            var questUrl = context.Request.Path.Value.ToLower();
            if (questUrl == _option.DeniedUrl)
                return this._next(context);
            if (_option.AllowAnonymousUrls.Where(p => questUrl.StartsWith(p)).Count() > 0)
                return this._next(context);

            //是否经过验证
            Microsoft.AspNetCore.Authentication.AuthenticateResult result;
            try
            {
                result = Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.AuthenticateAsync(context, CookieAuthenticationDefaults.AuthenticationScheme).Result;

            }
            catch
            {
                context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //context.Response.Redirect(_option.LoginUrl);
                return this._next(context);
            }
            if (result.Principal == null)
            {
                //if (questUrl != _option.LoginUrl)
                //{
                //    context.Response.Redirect(_option.LoginUrl);
                //}
                return this._next(context);
            }
            bool valid = true;
            var timespan = result.Properties.ExpiresUtc.Value - DateTime.UtcNow;
            if (timespan.TotalSeconds < 0)
            {
                valid = false;
            }
            if (!result.Succeeded || !result.Principal.Identity.IsAuthenticated)
            {
                valid = false;
            }
            if (valid)
            {

                if (_option.NoPermissionUrls.Where(p => questUrl.StartsWith(p)).Count() > 0)
                    return this._next(context);


                //用户名
                var userName = result.Principal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid).Value;
                UserDTO m_userdto = null;
                if (string.IsNullOrEmpty(context.Session.GetString("User")))
                {
                    var m_userservice = provider.GetService(typeof(IUserService)) as IUserService;
                    //如果是cookie过来验证
                    m_userdto = m_userservice.ValidateUserByName(userName);
                    if (m_userdto == null)
                    {
                        //无权限跳转到拒绝页面
                        context.Response.Redirect(host + _option.DeniedUrl);
                    }
                    else
                    {

                        var user_permissions = m_userservice.GetUserPermission(userName);

                        context.Session.SetString("User", Newtonsoft.Json.JsonConvert.SerializeObject(m_userdto));

                        context.Session.SetString("UserPermissions", Newtonsoft.Json.JsonConvert.SerializeObject(user_permissions));
                    }
                }
                else
                {
                    m_userdto = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(context.Session.GetString("User"));
                }
                if (m_userdto.IsDisabled)
                {
                    context.Response.Redirect(host + _option.DeniedUrl);
                }
                else
                {
                    if (!m_userdto.IsAdmin)
                    {
                        var userPermissions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserPermission>>(context.Session.GetString("UserPermissions"));
                        if (userPermissions != null && userPermissions.Count > 0)
                        {
                            if (questUrl == "/"|| questUrl=="")
                                questUrl = _option.HomeUrl;
                            if (userPermissions.Where(w => w.UserName == userName
                            && (w.Url.ToLower() == questUrl || w.Url2.ToLower() == questUrl
                            || w.Url3.ToLower() == questUrl)).Count() > 0)
                            {
                                return this._next(context);
                            }
                            else
                            {
                                if (questUrl == _option.HomeUrl)
                                {
                                    context.Response.Redirect(host + _option.WelcomeUrl);
                                }
                                else if (questUrl == _option.WelcomeUrl)
                                {
                                    return this._next(context);
                                }
                                else
                                {
                                    //无权限跳转到拒绝页面
                                    context.Response.Redirect(host + _option.DeniedUrl);
                                }
                            }
                        }
                        else
                        {
                            //无权限跳转到拒绝页面
                            context.Response.Redirect(host + _option.DeniedUrl);
                        }
                    }
                }
            }

            return this._next(context);
        }
    }
}
