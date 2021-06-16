using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WXSite.Database.Interfaces;

namespace WXSite.Database.Entities
{
    public class User : BaseEntity<int>, IAuditable
    {
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(20)]
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(50)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [MaxLength(500)]
        public string AvatarUrl { get; set; }
        [MaxLength(20)]
        public string Email { get; set; }
        /// <summary>
        /// 登录验证信息
        /// </summary>
        [MaxLength(100)]
        public string AuthData { get; set; }
    }
}
