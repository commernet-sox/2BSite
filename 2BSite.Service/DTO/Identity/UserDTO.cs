using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _2BSite.Service.DTO.Identity
{
    /// <summary>
    /// 账号信息
    /// </summary>
    public class UserDTO: BaseDTO//DTO定义父类不要有任何与数据库相关的字段，不然生成的QUERY会有问题
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        [Display(Name = "账号ID")]
        public int Id { get; set; }

        /// <summary>
        /// 账号别名
        /// </summary>
        [MaxLength(150)]
        [Display(Name = "账号别名")]
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        public string AliasName { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        [MaxLength(150)]
        [Display(Name = "登录名")]
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(150)]
        [Display(Name = "密码")]
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [Display(Name = "是否禁用")]
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [Display(Name = "最后登陆时间")]
        public Nullable<System.DateTime> LastLoginDatetime { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        [Display(Name = "登录次数")]
        public Nullable<int> LoginNumber { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        [Display(Name = "是否管理员")]
        public bool IsAdmin { get; set; }
        
        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        [AutoMapper.IgnoreMap]
        [Display(Name = "用户角色")]
        public IEnumerable<UserRoleDTO> UserRole { get; set; }

        [AutoMapper.IgnoreMap]
        [Display(Name = "用户系统")]
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        public IEnumerable<UserSystemDTO> UserSystem { get; set; }

        /// <summary>
        /// 用户所属角色ID
        /// </summary>
        public int UserRoleId { get; set; }
        /// <summary>
        /// 系统id
        /// </summary>
        public int UserSystemId { get; set; }
        /// <summary>
        /// 用户所属部门ID
        /// </summary>
        public int? UserDepartmentId { get; set; }

        /// <summary>
        /// 用户所属公司ID
        /// </summary>
        [Display(Name = "所属公司ID")]
        public int? CompanyId { get; set; }
        /// <summary>
        /// 用户所属公司名称
        /// </summary>
        [MaxLength(150)]
        [Display(Name = "所属公司名称")]
        public string Company { get; set; }

        /// <summary>
        /// 用户所拥有的公司权限（查看，编辑，修改，导入等）
        /// </summary>
        [AutoMapper.IgnoreMap]
        public List<int> UserCompany { get; set; }
        [AutoMapper.IgnoreMap]
        public string oper { get; set; }
    }
}
