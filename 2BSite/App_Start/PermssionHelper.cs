using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2BSite.App_Start
{
    /// <summary>
    /// 权限帮助类，用于RAZOR 前端使用
    /// </summary>
    public static class PermssionHelper
    {
        public static bool HasPermission(this HttpContext context, string url, UserDTO dto, List<UserPermission> userPermissions)
        {
            url = url.ToLower();
            url = url.Replace("/hrms", "");
            if (dto.IsAdmin)
                return true;
            if (userPermissions.Where(w => w.Url.ToLower() == url).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasPermission(this HttpContext context, string control, string action, UserDTO dto, List<UserPermission> userPermissions)
        {
            if (string.IsNullOrEmpty(control) || string.IsNullOrEmpty(action))
                return false;
            if (dto.IsAdmin)
                return true;
            if (userPermissions.Where(w => w.Control.ToLower() == control.ToLower() &&
            w.Action.ToLower() == action.ToLower()).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
