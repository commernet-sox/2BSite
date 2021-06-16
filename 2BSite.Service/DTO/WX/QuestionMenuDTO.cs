using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.DTO.WX
{
    public class QuestionMenuDTO:BaseDTO
    {
        public int Id { get; set; }
        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        [MappingExpression(PropertyName = "CreateTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; set; }
        /// <summary>
        /// 套卷题数
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 答题时间
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// 套卷使用人数
        /// </summary>
        public int PeopleNum { get; set; }

        public string Tips { get; set; }
    }
}
