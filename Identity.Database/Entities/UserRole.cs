using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Identity.Database.Entities
{
    /// <summary>
    /// 系统管理:账号角色管理
    /// </summary>
    public class UserRole : BaseEntity<int>
    {
        /// <summary>
        /// 账号Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }
    }
}
