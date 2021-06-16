using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WXSite.Database.Interfaces;

namespace WXSite.Database.Entities
{
    /// <summary>
    /// 答题记录
    /// </summary>
    public class History : BaseEntity<int>, IAuditable
    {
        /// <summary>
        /// 记录
        /// </summary>
        public string QuestionList { get; set; }
        /// <summary>
        /// 套题名称
        /// </summary>
        [MaxLength(20)]
        public string QuestionMenu { get; set; }
        /// <summary>
        /// 套题Id
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }
}
