using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WXSite.Database.Interfaces;

namespace WXSite.Database.Entities
{
    /// <summary>
    /// 题目库
    /// </summary>
    public class Questions : BaseEntity<int>, IAuditable
    {
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(200),Required]
        public string Title { get; set; }
        /// <summary>
        /// 解析
        /// </summary>
        [MaxLength(500)]
        public string Help { get; set; }
        /// <summary>
        /// 题目类型 1单选题  2多选题  3判断题
        /// </summary>
        [MaxLength(10),Required]
        public string Type { get; set; }
        /// <summary>
        /// 选项,包含正确答案
        /// </summary>
        [MaxLength(500),Required]
        public string ChoseList { get; set; }
        /// <summary>
        /// 对应的套题
        /// </summary>
        [Required]
        public int MenuId { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [MaxLength(100)]
        public string PicUrl { get; set; }
    }
}
