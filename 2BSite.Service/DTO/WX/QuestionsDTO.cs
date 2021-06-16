using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.DTO.WX
{
    public class QuestionsDTO: BaseDTO
    {
        public int Id { get; set; }
        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        [MappingExpression(PropertyName = "CreateTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }

        /// <summary>
        /// 标题
        /// </summary>

        public string Title { get; set; }
        /// <summary>
        /// 解析
        /// </summary>

        public string Help { get; set; }
        /// <summary>
        /// 题目类型 1单选题  2多选题  3判断题
        /// </summary>

        public string Type { get; set; }
        /// <summary>
        /// 选项,包含正确答案
        /// </summary>

        public string ChoseList { get; set; }
        /// <summary>
        /// 对应的套题
        /// </summary>

        public int MenuId { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>

        public string PicUrl { get; set; }

        /// <summary>
        /// 选项,包含正确答案 序列化成对象
        /// </summary>
        public object ChoseListObj { get; set; }
    }
}
