using _2BSite.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace _2BSite.Database.Entities
{
    public class CodeMaster : BaseEntity<int>, IAuditable
    {
        [MaxLength(20), Required]
        public string CodeGroup { get; set; }

        [MaxLength(20), Required]
        public string CodeId { get; set; }

        [MaxLength(50)]
        public string CodeName { get; set; }

        [Column(TypeName = "char(1)"), MaxLength(1)]
        public string IsActive { get; set; }

        [MaxLength(200)]
        public string Remarks { get; set; }

        [MaxLength(200)]
        public string HUDF_01 { get; set; }
    }
}
