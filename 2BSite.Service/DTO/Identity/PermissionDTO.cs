using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _2BSite.Service.DTO.Identity
{
    public class PermissionDTO : BaseDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 权限名字
        /// </summary>
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        [MaxLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        [MaxLength(150)]
        public string Control { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        [MaxLength(150)]
        public string Action { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [MaxLength(150)]
        public string Area { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public Nullable<int> OrderIndex { get; set; }

        /// <summary>
        /// 父权限ID
        /// </summary>
        public int? ParentID { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        /// <summary>
        /// 父权限ID
        /// </summary>
        [AutoMapper.IgnoreMap]
        public string ParentName { get; set; }

        public int? SystemID { get; set; }
        public string oper { get; set; }
    }
}
