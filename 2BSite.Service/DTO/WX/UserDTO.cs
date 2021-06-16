using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.DTO.WX
{
    public class UserDTO:BaseDTO
    {
        public int Id { get; set; }
        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        [MappingExpression(PropertyName = "CreateTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }


        public string UserName { get; set; }

        public string Password { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>

        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>

        public string NickName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>

        public string AvatarUrl { get; set; }

        public string Email { get; set; }
        /// <summary>
        /// 登录验证信息
        /// </summary>

        public string AuthData { get; set; }
    }
}
