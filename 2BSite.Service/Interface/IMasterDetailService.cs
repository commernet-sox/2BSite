using _2BSite.Service.DTO;
using Core.Database.Repository;
using Core.WebServices.DTO;
using Core.WebServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.Interface
{
    public interface IMasterDetailService<TEntity, T, Tkey> : IBase<TEntity, T, Tkey> where T : MasteDetailBaseDTO
    {
        T GetMaserDetailsInfo(Tkey id);

        DBResult SubmitMasterDetail(T DTO);

        DBResult Merge(T persisted, T current, bool isSubmit = true);

    }
}
