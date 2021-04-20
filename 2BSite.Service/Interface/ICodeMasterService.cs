using _2BSite.Database.Entities;
using _2BSite.Service.DTO;
using Core.Database.Repository;
using Core.WebServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.Interface
{
    public interface ICodeMasterService : IBase<CodeMaster, CodeMasterDTO, int>, IDatatable
    {
        DBResult SubmitCodeMaster(CodeMasterDTO codeMasterDTO);
        CodeMasterDTO GetCodeMasterInfo(int Id);
    }
}
