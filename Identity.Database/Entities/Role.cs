using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Identity.Database.Entities
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class Role:BaseEntity<int>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [MaxLength(150)]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        public int? SystemID { get; set; }
    }
}
