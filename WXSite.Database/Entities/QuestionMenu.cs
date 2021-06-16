using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WXSite.Database.Interfaces;

namespace WXSite.Database.Entities
{
    /// <summary>
    /// 套题菜单
    /// </summary>
    public class QuestionMenu : BaseEntity<int>, IAuditable
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(20), Required]
        public string Name { get; set; }
        /// <summary>
        /// 套卷题数
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 答题时间
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// 套卷使用人数
        /// </summary>
        public int PeopleNum { get; set; }
        /// <summary>
        /// 说明文件
        /// </summary>
        [MaxLength(500)]
        public string Tips { get; set; }
    }
}
