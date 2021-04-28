using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Database.Interfaces
{
    interface IEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}
