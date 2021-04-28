using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;
using _2BSite.Service.Service;
using Identity.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.WebServices;

namespace _2BSite.Controllers
{
    /// <summary>
    /// 系统权限配置
    /// </summary>
    public class SystemRoleController : BaseController
    {
        private IPermissionService _permissionService;
        private IRoleService _roleService;
        private IRolePermissionService _rolePermissionService;
        private IUserService _userService;
        private IUserRoleService _userRoleService;
        private IUserSystemService _userSystemService;
        public SystemRoleController(IPermissionService permissionService, IRoleService roleService, IRolePermissionService rolePermissionService, IUserService userService, IUserRoleService userRoleService, IUserSystemService userSystemService)
        {
            _permissionService = permissionService;
            _roleService = roleService;
            _rolePermissionService = rolePermissionService;
            _userService = userService;
            _userRoleService = userRoleService;
            _userSystemService = userSystemService;
        }
        /// <summary>
        /// 权限分配主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 权限管理  用到的表:Permission
        /// </summary>
        /// <returns></returns>
        public IActionResult PermissionIndex()
        {
            return View();
        }
        /// <summary>
        /// 账号管理  用到的表:User,UserRole,UserSystem
        /// </summary>
        /// <returns></returns>
        public IActionResult UserIndex()
        {
            return View();
        }
        /// <summary>
        /// 角色管理  用到的表:Role,RolePermission,
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleIndex()
        {
            return View();
        }
        /// <summary>
        /// 权限主表操作
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public IActionResult PermissionAdd(PermissionDTO test)
        {
            try
            {
                if (test.oper == "add")
                {
                    test.CreateTime = DateTime.Now;
                    test.Creator = "administrator";
                    var res = _permissionService.Add(test);
                    if (res.Code == 0)
                    {
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                if (test.oper == "edit")
                {
                    test.ModifyTime = DateTime.Now;
                    test.Modifier = "administrator";
                    var res = _permissionService.Update(test);
                    if (res.Code == 0)
                    {
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                if (test.oper == "delete")
                {
                    var res = _permissionService.DeleteByID(test.Id);
                    if (res.Code == 0)
                    {
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                return Ok("success");
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 权限详情操作
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public IActionResult PermissionDetailAdd(PermissionDTO test)
        {
            try
            {
                if (test.oper == "add")
                {
                    test.CreateTime = DateTime.Now;
                    test.Creator = "administrator";
                    var res = _permissionService.Add(test);
                    if (res.Code == 0)
                    {
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                if (test.oper == "edit")
                {
                    test.ModifyTime = DateTime.Now;
                    test.Modifier = "administrator";
                    var res = _permissionService.Update(test);
                    if (res.Code == 0)
                    {
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                if (test.oper == "delete")
                {
                    var res = _permissionService.DeleteByID(test.Id);
                    if (res.Code == 0)
                    {
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                return Ok("success");
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 权限主表查询
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult PermissionSearch(int rows,int page)
        {
            var total = _permissionService.GetAll().Where(t=>t.ParentID==null|| t.ParentID==0).Count();
            var data = _permissionService.GetAll().Where(t => t.ParentID == null || t.ParentID == 0).Skip((page-1)*rows).Take(rows).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", total % rows == 0 ? total / rows : total / rows + 1);
            dic.Add("page",page);
            dic.Add("records",data.Count());
            dic.Add("pageSize",page);
            dic.Add("rows",data);
            return Json(dic);
        }
        /// <summary>
        /// 权限详情查询
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult PermissionDetail(int rows, int page,int? id)
        {
            if (id == 0)
            {
                return Json("");
            }
            var total = _permissionService.GetAll().Where(t=>t.ParentID==id).Count();
            var data = _permissionService.GetAll().Where(t => t.ParentID == id).Skip((page - 1) * rows).Take(rows).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", total % rows == 0 ? total / rows : total / rows + 1);
            dic.Add("page", page);
            dic.Add("records", data.Count());
            dic.Add("pageSize", page);
            dic.Add("rows", data);
            return Json(dic);
        }
        /// <summary>
        /// 权限菜单查询
        /// </summary>
        /// <returns></returns>
        public IActionResult PermissionMenuSearch()
        {
            var permissions = _permissionService.GetAll().Select(t=>new { id=t.Id,parent=t.ParentID==0||t.ParentID==null?"#":t.ParentID.ToString(),text=t.Name}).ToList();
            return Json(permissions);
        }
        /// <summary>
        /// 角色权限主表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult RoleSearch(int rows,int page)
        {
            var total = _permissionService.GetAll().Count();
            var data = _roleService.GetAll().Skip((page - 1) * rows).Take(rows).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", total % rows == 0 ? total / rows : total / rows + 1);
            dic.Add("page", page);
            dic.Add("records", data.Count());
            dic.Add("pageSize", page);
            dic.Add("rows", data);
            return Json(dic);
        }
        /// <summary>
        /// 提交权限配置
        /// </summary>
        /// <param name="roleDTO"></param>
        /// <returns></returns>
        public IActionResult CommitPermissions(RoleDTO roleDTO)
        {
            if (roleDTO == null)
            {
                return NotFound();
            }
            else
            {
                if (roleDTO.Id != 0)
                {
                    roleDTO.ModifyTime = DateTime.Now;
                    roleDTO.Modifier = "administrator";
                    var roleRes = _roleService.Update(roleDTO);
                    List<RolePermissionDTO> rolePermissionDTOs = new List<RolePermissionDTO>();
                    foreach (var item in roleDTO.Permissions)
                    {
                        RolePermissionDTO rolePermissionDTO = new RolePermissionDTO();
                        rolePermissionDTO.RoleId = roleDTO.Id;
                        int.TryParse(item, out var pid);
                        rolePermissionDTO.PermissionId = pid;
                        rolePermissionDTO.Remarks = roleDTO.Remarks;
                        rolePermissionDTO.CreateTime = DateTime.Now;
                        rolePermissionDTO.Creator = "administrator";
                        rolePermissionDTOs.Add(rolePermissionDTO);
                    }
                    var olds = _rolePermissionService.GetAll().Where(t => t.RoleId == roleDTO.Id).Select(t=>t.Id).ToArray();
                    _rolePermissionService.DeleteRange(olds);
                    var permissionRes = _rolePermissionService.AddRange(rolePermissionDTOs.ToArray());
                    if (permissionRes.Code == 0)
                    {
                        return Json("success");
                    }
                    else
                    {
                        return Json("failed");
                    }
                }
                else//新增
                {
                    roleDTO.CreateTime = DateTime.Now;
                    roleDTO.Creator = "administrator";
                    var roleRes = _roleService.Add(roleDTO);
                    Role role = (Role)roleRes.Result;
                    List<RolePermissionDTO> rolePermissionDTOs = new List<RolePermissionDTO>();
                    foreach (var item in roleDTO.Permissions)
                    {
                        RolePermissionDTO rolePermissionDTO = new RolePermissionDTO();
                        rolePermissionDTO.RoleId = role.Id;
                        int.TryParse(item, out var pid);
                        rolePermissionDTO.PermissionId = pid;
                        rolePermissionDTO.Remarks = roleDTO.Remarks;
                        rolePermissionDTO.CreateTime = DateTime.Now;
                        rolePermissionDTO.Creator = "administrator";
                        rolePermissionDTOs.Add(rolePermissionDTO);
                    }
                    var permissionRes = _rolePermissionService.AddRange(rolePermissionDTOs.ToArray());
                    if (permissionRes.Code == 0)
                    {
                        return Json("success");
                    }
                    else
                    {
                        return Json("failed");
                    }
                }
                
            }
        }
        /// <summary>
        /// 获取权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetRolePermissionsId(int id)
        {

            var role = _roleService.GetAll().Where(t => t.Id == id).FirstOrDefault();
            var permissions = _rolePermissionService.GetAll().Where(t => t.RoleId == role.Id).Select(t => t.PermissionId).ToArray();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Name",role.Name);
            dic.Add("SystemID",role.SystemID);
            dic.Add("Remarks",role.Remarks);
            dic.Add("Permissions", permissions);
            return Json(dic);
        }
        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <returns></returns>
        public IActionResult UserSearch(int rows, int page)
        {
            var res = _userService.UserSearch(rows,page);
            return Json(res);
        }
        /// <summary>
        /// 编辑用户账号信息
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public IActionResult UserAdd(UserDTO userDTO)
        {
            try
            {
                if (userDTO.oper == "add")
                {
                    userDTO.CreateTime = DateTime.Now;
                    userDTO.Creator = "administrator";
                    var res = _userService.Add(userDTO);
                    User user = (User)res.Result;
                    List<UserRoleDTO> userRoleDTOs = new List<UserRoleDTO>();
                    List<UserSystemDTO> userSystemDTOs = new List<UserSystemDTO>();
                    if (res.Code == 0)
                    {
                        //foreach (var item in userDTO.UserRole)
                        //{
                        //    item.UserId = user.Id;
                        //    item.CreateTime = DateTime.Now;
                        //    item.Creator = "administrator";
                        //    userRoleDTOs.Add(item);
                        //}
                        //foreach (var item in userDTO.UserSystem)
                        //{
                        //    item.UserId = user.Id;
                        //    item.CreateTime = DateTime.Now;
                        //    item.Creator = "administrator";
                        //    userSystemDTOs.Add(item);
                        //}
                        UserRoleDTO userRoleDTO = new UserRoleDTO();
                        userRoleDTO.UserId = user.Id;
                        userRoleDTO.RoleId = userDTO.UserRoleId;
                        userRoleDTO.CreateTime = DateTime.Now;
                        userRoleDTO.Creator = "administrator";
                        userRoleDTOs.Add(userRoleDTO);

                        UserSystemDTO userSystemDTO = new UserSystemDTO();
                        userSystemDTO.UserId = user.Id;
                        userSystemDTO.SystemId = userDTO.UserSystemId;
                        userSystemDTO.CreateTime = DateTime.Now;
                        userSystemDTO.Creator = "administrator";
                        userSystemDTOs.Add(userSystemDTO);

                        _userRoleService.AddRange(userRoleDTOs.ToArray());
                        _userSystemService.AddRange(userSystemDTOs.ToArray());

                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                if (userDTO.oper == "edit")
                {
                    userDTO.ModifyTime = DateTime.Now;
                    userDTO.Modifier = "administrator";
                    var res = _userService.Update(userDTO);
                    List<UserRoleDTO> userRoleDTOs = new List<UserRoleDTO>();
                    List<UserSystemDTO> userSystemDTOs = new List<UserSystemDTO>();
                    if (res.Code == 0)
                    {
                        //foreach (var item in userDTO.UserRole)
                        //{
                        //    item.UserId = userDTO.Id;
                        //    item.CreateTime = DateTime.Now;
                        //    item.Creator = "administrator";
                        //    userRoleDTOs.Add(item);
                        //}
                        //foreach (var item in userDTO.UserSystem)
                        //{
                        //    item.UserId = userDTO.Id;
                        //    item.CreateTime = DateTime.Now;
                        //    item.Creator = "administrator";
                        //    userSystemDTOs.Add(item);
                        //}
                        UserRoleDTO userRoleDTO = new UserRoleDTO();
                        userRoleDTO.UserId = userDTO.Id;
                        userRoleDTO.RoleId = userDTO.UserRoleId;
                        userRoleDTO.CreateTime = DateTime.Now;
                        userRoleDTO.Creator = "administrator";
                        userRoleDTOs.Add(userRoleDTO);

                        UserSystemDTO userSystemDTO = new UserSystemDTO();
                        userSystemDTO.UserId = userDTO.Id;
                        userSystemDTO.SystemId = userDTO.UserSystemId;
                        userSystemDTO.CreateTime = DateTime.Now;
                        userSystemDTO.Creator = "administrator";
                        userSystemDTOs.Add(userSystemDTO);

                        var oldUserRoles = _userRoleService.GetAll().Where(t => t.UserId == userDTO.Id).ToArray();
                        _userRoleService.DeleteRangeBy(oldUserRoles);

                        var oldUserSystems = _userSystemService.GetAll().Where(t => t.UserId == userDTO.Id).ToArray();
                        _userSystemService.DeleteRangeBy(oldUserSystems);

                        _userRoleService.AddRange(userRoleDTOs.ToArray());
                        _userSystemService.AddRange(userSystemDTOs.ToArray());
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                if (userDTO.oper == "delete")
                {
                    var res = _userService.DeleteByID(userDTO.Id);
                    if (res.Code == 0)
                    {
                        var oldUserRoles = _userRoleService.GetAll().Where(t => t.UserId == userDTO.Id).ToArray();
                        _userRoleService.DeleteRangeBy(oldUserRoles);

                        var oldUserSystems = _userSystemService.GetAll().Where(t => t.UserId == userDTO.Id).ToArray();
                        _userSystemService.DeleteRangeBy(oldUserSystems);
                        return Ok("success");
                    }
                    else
                    {
                        return NotFound("failed");
                    }
                }
                return Ok("success");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class Test
    {
        public string id { get; set; }
        public string invdate { get; set; }
        public string name { get; set; }
        public string amount { get; set; }
        public string tax { get; set; }
        public string total { get; set; }
        public string note { get; set; }
        public string oper { get; set; }
    }
}
