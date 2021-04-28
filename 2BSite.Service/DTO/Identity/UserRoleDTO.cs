using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _2BSite.Service.DTO.Identity
{
    public class UserRoleDTO : BaseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        [AutoMapper.IgnoreMap]
        public string RoleName { get; set; }
    }
}
