using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.DTO
{
    public class CodeMasterDTO : MasteDetailBaseDTO
    {
        public int Id { get; set; }
        public System.DateTime? ModifyTime { get; set; }
        public string Modifier { get; set; }
        [MappingExpression(PropertyName = "CreateTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public System.DateTime CreateTime { get; set; }
        public string Creator { get; set; }


        public string CodeGroup { get; set; }


        public string CodeId { get; set; }


        public string CodeName { get; set; }


        public string IsActive { get; set; }


        public string Remarks { get; set; }


        public string HUDF_01 { get; set; }
    }
}
