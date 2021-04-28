using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Identity.Database.Entities
{
    /// <summary>
    /// 账号信息
    /// </summary>
    public class User:BaseEntity<int>
    {
        /// <summary>
        /// 别名
        /// </summary>
        [MaxLength(150)]
        [Column(Order = 5)]
        public string AliasName { get; set; }

        /// <summary>
        /// 登录名/账号
        /// </summary>
        [MaxLength(150)]
        [Column(Order = 6)]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(150)]
        [Column(Order = 7)]
        public string Password { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Column(Order = 8)]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Column(Order = 9)]
        public Nullable<global::System.DateTime> LastLoginDatetime { get; set; }

        /// <summary>
        /// 登陆次数
        /// </summary>
        [Column(Order = 10)]
        public Nullable<int> LoginNumber { get; set; }

        [Column(Order = 11)]
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [Column(Order = 13)]
        public int? CompanyId { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        [MaxLength(150)]
        [Column(Order = 12)]
        public string Company { get; set; }
    }
}
