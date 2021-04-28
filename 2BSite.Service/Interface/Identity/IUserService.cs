using Core.WebServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Identity.Database.Entities;
using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Model;

namespace _2BSite.Service.Interface.Identity
{
    public interface IUserService : IBase<User, UserDTO, int>, IDatatable
    {
        /// <summary>
        ///  修改用户登录次数
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserDTO ValidateUser(string username, string password);
        /// <summary>
        /// 根据用户名更新用户登录次数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserDTO ValidateUserByName(string username);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        bool ResetPassword(int id, string newpassword);
        /// <summary>
        /// 得到权限
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        List<UserPermission> GetUserPermission(string username);

        Dictionary<string, object> UserSearch(int rows, int page);
    }
}
