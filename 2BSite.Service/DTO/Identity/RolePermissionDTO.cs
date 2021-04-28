using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _2BSite.Service.DTO.Identity
{
    /// <summary>
    /// 系统管理:角色权限控制
    /// </summary>
    public class RolePermissionDTO : BaseDTO
    {
        /// <summary>
        /// 角色权限关联Id(主键)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 权限Id
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }

        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }
       
    }
}
