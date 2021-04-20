using System;
using System.Collections.Generic;
using System.Text;

namespace Npoi.Report.CustomDynamicColumns
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TitleAttribute : Attribute
    {
        public string Name { get; set; }
        public TitleAttribute(string name)
        {
            Name = name;
        }
    }
}

