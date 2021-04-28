using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Identity.Database.Entities
{
    /// <summary>
    /// 系统
    /// </summary>
    public class Systems : BaseEntity<int>
    {
        /// <summary>
        /// 系统名
        /// </summary>
        [MaxLength(150)]
        [Column(Order = 5)]
        public string Name { get; set; }

        /// <summary>
        /// 系统备注
        /// </summary>
        [MaxLength(500)]
        [Column(Order = 6)]
        public string Remarks { get; set; }
    }
}
