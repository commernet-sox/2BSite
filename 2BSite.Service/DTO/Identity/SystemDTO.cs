using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _2BSite.Service.DTO.Identity
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class SystemDTO : BaseDTO//DTO定义父类不要有任何与数据库相关的字段，不然生成的QUERY会有问题
    {
        /// <summary>
        /// 系统ID
        /// </summary>
        [Display(Name = "系统ID")]
        public int Id { get; set; }
        /// <summary>
        /// 账号Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 系统名
        /// </summary>
        [MaxLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// 系统备注
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }

        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

    }
}
