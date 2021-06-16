using System;
using System.Collections.Generic;
using System.Text;

namespace WXSite.Database.Interfaces
{
    interface IEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}
