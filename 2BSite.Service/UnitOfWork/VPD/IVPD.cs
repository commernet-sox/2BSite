using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.UnitOfWork
{
    public interface IVPD<T> where T : Microsoft.EntityFrameworkCore.DbContext
    {
        void SetVPD(T dbcotext);
    }
}
