using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WXSite.Database.Interfaces;

namespace WXSite.Database.Entities
{
    public class Error : BaseEntity<int>, IAuditable
    {
        /// <summary>
        /// 记录
        /// </summary>
        [Required]
        public string QuestionList { get; set; }
        /// <summary>
        /// 套题名称
        /// </summary>
        [MaxLength(20), Required]
        public string QuestionMenu { get; set; }
        /// <summary>
        /// 套题Id
        /// </summary>
        [Required]
        public int MenuId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }
}
