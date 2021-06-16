using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WXSite.Database.Interfaces;

namespace WXSite.Database.Entities
{
    /// <summary>
    /// 基础类
    /// </summary>
    /// <typeparam name="TIdentifier">泛型标识符</typeparam>
    public abstract class BaseEntity<TIdentifier> : IEntity<TIdentifier>
    {
        /// <summary>
        /// 各表Id(自增主键)
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TIdentifier Id { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column(Order = 1)]
        public System.DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [Column(Order = 2)]
        [MaxLength(150)]
        public string Modifier { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Order = 3)]
        public System.DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column(Order = 4)]
        [MaxLength(150)]
        public string Creator { get; set; }
    }
}
