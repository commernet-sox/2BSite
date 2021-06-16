using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.DTO.WX
{
    public class HistoryDTO:BaseDTO
    {
        public int Id { get; set; }
        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        [MappingExpression(PropertyName = "CreateTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        /// <summary>
        /// 记录
        /// </summary>
        public string QuestionList { get; set; }
        /// <summary>
        /// 套题名称
        /// </summary>

        public string QuestionMenu { get; set; }
        /// <summary>
        /// 套题Id
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        public object QuestionListObj { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public int AverageScore { get; set; }
        /// <summary>
        /// 击败多少人
        /// </summary>
        public int BeatNum { get; set; }
        public string sessionId { get; set; }
    }
}
