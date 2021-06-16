using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.DTO.WX
{
    public class FeedBackDTO:BaseDTO
    {
        public int Id { get; set; }
        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        [MappingExpression(PropertyName = "CreateTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>

        public string Phone { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }
}
