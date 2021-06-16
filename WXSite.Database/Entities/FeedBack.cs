using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WXSite.Database.Interfaces;

namespace WXSite.Database.Entities
{
    public class FeedBack : BaseEntity<int>, IAuditable
    {
        /// <summary>
        /// 反馈内容
        /// </summary>
        [MaxLength(200)]
        public string Content { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(20)]
        public string Phone { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }
}
