using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2BSite.Middleware
{
    /// <summary>
    /// 权限中间件选项
    /// </summary>
    public class PermissionMiddlewareOption
    {
        /// <summary>
        /// 登录地址
        /// </summary>
        public string LoginUrl
        { get; set; }
        /// <summary>
        /// 无权限导航action
        /// </summary>
        public string DeniedUrl
        { get; set; }
        /// <summary>
        /// 主页
        /// </summary>
        public string HomeUrl
        { get; set; }
        /// <summary>
        /// 主页无权限默认跳转的页面
        /// </summary>
        public string WelcomeUrl
        { get; set; }

        /// <summary>
        /// 需要验证登录，但不需要权限控制的地址
        /// </summary>
        public List<string> NoPermissionUrls
        { get; set; }

        /// <summary>
        /// 允许匿名访问的地址，或者自己写验证
        /// </summary>
        public List<string> AllowAnonymousUrls
        { get; set; }

    }
}
