using System;
using System.Collections.Generic;
using System.Text;

namespace Npoi.Report.CustomDynamicColumns
{
    /// <summary>
    /// Specifies to ignore a property for mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {
    }

}
