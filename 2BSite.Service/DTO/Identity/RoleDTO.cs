using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace _2BSite.Service.DTO.Identity
{
    public class RoleDTO : BaseDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        [MaxLength(150)]
        [Core.Infrastructure.DataTables.Attributes.RequireAttribute("不能为空")]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        [AutoMapper.IgnoreMap]
        public IEnumerable<RolePermissionDTO> RolePermission { get; set; }

        public int? SystemID { get; set; }

        public string[] Permissions { get; set; }
    }
}
