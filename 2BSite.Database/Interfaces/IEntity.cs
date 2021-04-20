using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Database.Interfaces
{
    interface IEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}
