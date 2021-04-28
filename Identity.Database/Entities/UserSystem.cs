using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Identity.Database.Entities
{
    /// <summary>
    /// 系统管理:帐号能登录的系统
    /// </summary>
    public class UserSystem : BaseEntity<int>
    {
        /// <summary>
        /// 账号Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 系统Id
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }
    }
}
