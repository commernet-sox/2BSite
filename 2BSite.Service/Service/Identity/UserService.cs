using AutoMapper;
using Core.Database.Repository;
using Core.Infrastructure;
using Core.Infrastructure.DataTables;
using Core.WebServices.Model;
using Core.WebServices.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Core.Database.Extension;
using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;
using _2BSite.Service.Model;
using Identity.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace _2BSite.Service.Service
{
    public class UserService : BaseService<User, Identity.Database.IdentityDataContext, UserDTO, int>, IUserService
    {
        private IUserRoleService _IUserRoleService;
        private IRoleService _IRoleService;

        private IUserSystemService _IUserSystemService;
        private ISystemService _ISystemService;

        private IServiceProvider _IServiceProvider;
        private IRolePermissionService _rolePermissionService;
        private IPermissionService _permissionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="mapper"></param>
        /// <param name="IUserRoleService"></param>
        /// <param name="IRoleService"></param>
        /// <param name="companyService"></param>
        public UserService(IRepository<User, Identity.Database.IdentityDataContext> Repository, IMapper mapper,
            IUserRoleService IUserRoleService,
            IRoleService IRoleService,
            IServiceProvider IServiceProvider,
            IUserSystemService IUserSystemService,
            ISystemService ISystemService,
            IRolePermissionService rolePermissionService,
            IPermissionService permissionService) : base(Repository, mapper)
        {
            _IUserRoleService = IUserRoleService;
            _IRoleService = IRoleService;
            _IUserSystemService = IUserSystemService;
            _ISystemService = ISystemService;
            _IServiceProvider = IServiceProvider;
            _rolePermissionService = rolePermissionService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// 修改用户登录次数
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserDTO ValidateUser(string username, string password)
        {
            var result = this.GetAll().Where(p => p.LoginName == username && p.Password == password && p.IsDisabled == false).FirstOrDefault();

            //增加登录次数和修改最后登录时间
            if (result != null)
            {
                if (result.LoginNumber.HasValue)
                {
                    result.LoginNumber++;
                }
                else
                {
                    result.LoginNumber = 1;
                }
                result.LastLoginDatetime = DateTime.Now;
                this.Update(result);
                result.UserRole = this._IUserRoleService.GetAll().Where(w => w.UserId == result.Id).Future();
                result.UserSystem = this._IUserSystemService.GetAll().Where(w => w.UserId == result.Id).ToList();

            }
            return result;
        }

        /// <summary>
        /// 根据用户名更新用户登录次数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserDTO ValidateUserByName(string username)
        {
            var result = this.GetAll().Where(p => p.LoginName == username && p.IsDisabled == false).FirstOrDefault();
            if (result != null)
            {
                if (result.LoginNumber.HasValue)
                {
                    result.LoginNumber++;
                }
                else
                {
                    result.LoginNumber = 1;
                }
                result.LastLoginDatetime = DateTime.Now;

                this.Update(result);
                result.UserRole = this._IUserRoleService.GetAll().Where(w => w.UserId == result.Id).Future();
                result.UserSystem = this._IUserSystemService.GetAll().Where(w => w.UserId == result.Id).ToList();

            }
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        public bool ResetPassword(int id, string newpassword)
        {
            var item = base.GetByID(id);
            item.Password = newpassword.ToMD5String();
            var dbresult = base.Update(item);
            if (dbresult.Code != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取账号对应权限信息方法
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<UserPermission> GetUserPermission(string username)
        {
            //var db = this.Repository.SlaveUnitOfWork.DbContext;
            var result = (from u in this.GetAll()
                          join ur in _IUserRoleService.GetAll() on u.Id equals ur.UserId
                          join rp in _rolePermissionService.GetAll() on ur.RoleId equals rp.RoleId
                          join p in _permissionService.GetAll() on rp.PermissionId equals p.Id
                          where u.LoginName == username
                          group u by new
                          {
                              u.LoginName,
                              p.Action,
                              p.Control,
                              p.Area
                          } into gp

                          select new UserPermission
                          {
                              UserName = gp.Key.LoginName,
                              Action = gp.Key.Action,
                              Control = gp.Key.Control,
                              Area = gp.Key.Area
                          }).ToList();
            return result;
        }

        protected override CoreResponse Remove(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Create(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Edit(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Upload(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> UserSearch(int rows, int page)
        {
            var total = this.GetAll().Count();
            var data = this.GetAll().Skip((page - 1) * rows).Take(rows).ToList();
            var userRole = _IUserRoleService.GetAll().Where(t => data.Select(u => u.Id).Contains(t.Id)).ToList();
            var userSystem = _IUserSystemService.GetAll().Where(t => data.Select(u => u.Id).Contains(t.Id)).ToList();
            data.ForEach(t =>
            {
                t.UserRoleId = userRole.Where(u => u.UserId == t.Id).Select(u => u.RoleId).FirstOrDefault();
                t.UserSystemId = userSystem.Where(u => u.UserId == t.Id).Select(u => u.SystemId).FirstOrDefault();
            });
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", total % rows == 0 ? total / rows : total / rows + 1);
            dic.Add("page", page);
            dic.Add("records", data.Count());
            dic.Add("pageSize", page);
            dic.Add("rows", data);
            return dic;
        }
    }
}
