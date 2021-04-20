using _2BSite.Database;
using _2BSite.Database.Entities;
using _2BSite.Service.DTO;
using _2BSite.Service.Interface;
using AutoMapper;
using Core.Database.Repository;
using Core.Infrastructure.DataTables;
using Core.WebServices.Model;
using Core.WebServices.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2BSite.Service.Service
{
    public class CodeMasterService : BaseService<CodeMaster, BSiteContext, CodeMasterDTO, int>, ICodeMasterService
    {
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="mapper"></param>
        public CodeMasterService(IRepository<CodeMaster, BSiteContext> Repository, IMapper mapper,
           IServiceProvider serviceProvider) : base(Repository, mapper)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 查询明细
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CodeMasterDTO GetCodeMasterInfo(int Id)
        {
            var res = this.GetByID(Id);
            res.DX_Status = "edit";
            return res;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="codeMasterDTO"></param>
        /// <returns></returns>
        public DBResult SubmitCodeMaster(CodeMasterDTO codeMasterDTO)
        {
            DBResult dBResult = new DBResult();
            if (codeMasterDTO.DX_Status == "create")
            {
                codeMasterDTO.Id = 0;
                codeMasterDTO.Creator = "administrator";
                codeMasterDTO.CreateTime = DateTime.Now;
                dBResult = this.Add(codeMasterDTO);
                if (dBResult.Code != 0)
                {
                    return dBResult;
                }
            }
            else if (codeMasterDTO.DX_Status == "edit")
            {
                codeMasterDTO.Modifier = "administrator";
                codeMasterDTO.ModifyTime = DateTime.Now;
                dBResult = this.Update(codeMasterDTO);
                if (dBResult.Code != 0)
                {
                    return dBResult;
                }
            }
            else
            {
                dBResult.Code = 4001;
                dBResult.ErrMsg = "无效的DX_Status";
            }
            return dBResult;
        }

        protected override CoreResponse Create(CoreRequest core_request)
        {
            CoreResponse core_response = new CoreResponse(core_request);

            foreach (var item in core_request.DtRequest.Data)
            {
                string key = item.Key;
                List<Dictionary<string, object>> list_pair = new List<Dictionary<string, object>>();
                var pair = item.Value as Dictionary<string, object>;
                CodeMasterDTO newCodemaster = new CodeMasterDTO();
                base.ConvertDictionaryToObject(newCodemaster, pair, core_response.DtResponse.fieldErrors);
                if (core_response.DtResponse.fieldErrors != null && core_response.DtResponse.fieldErrors.Count > 0)
                    return core_response;
                var existCodemaster = this.GetAll()
                    .Where(codemaster => codemaster.CodeId == newCodemaster.CodeId
                    && codemaster.CodeName == newCodemaster.CodeName
                    && codemaster.CodeGroup == newCodemaster.CodeGroup
                    ).FirstOrDefault();
                if (existCodemaster != null)
                {
                    DtResponse.FieldError fe = new DtResponse.FieldError();
                    fe.name = "CodeName";
                    fe.status = "该参数名称已经存在";
                    core_response.DtResponse.fieldErrors.Add(fe);
                    return core_response;
                }
                DBResult dbresult;
                newCodemaster.CreateTime = DateTime.Now;
                newCodemaster.Creator = "administrator";
                dbresult = this.Add(newCodemaster);
                if (dbresult.Code != 0)
                {
                    core_response.DtResponse.error += dbresult.ErrMsg;
                }
            }
            return core_response;
        }

        protected override CoreResponse Edit(CoreRequest core_request)
        {

            CoreResponse core_response = new CoreResponse(core_request);
            foreach (var item in core_request.DtRequest.Data)
            {
                string key = item.Key;
                List<Dictionary<string, object>> listPair = new List<Dictionary<string, object>>();
                var pair = item.Value as Dictionary<string, object>;
                CodeMasterDTO updateCodemaster, originRelease;
                updateCodemaster = this.GetAll().Where(c => c.Id == Convert.ToInt32(key)).FirstOrDefault();
                originRelease = (CodeMasterDTO)updateCodemaster.Clone();
                base.ConvertDictionaryToObject(updateCodemaster, pair, core_response.DtResponse.fieldErrors);

                Dictionary<string, string> oridic = new Dictionary<string, string>();
                Dictionary<string, string> upddic = new Dictionary<string, string>();


                if (core_response.DtResponse.fieldErrors != null && core_response.DtResponse.fieldErrors.Count > 0)
                    return core_response;
                updateCodemaster.Modifier = "administrator";
                updateCodemaster.ModifyTime = DateTime.Now;
                DBResult dbresult;
                dbresult = this.Merge(originRelease, updateCodemaster);
                if (dbresult.Code != 0)
                {
                    core_response.DtResponse.error += dbresult.ErrMsg;
                }

            }
            return core_response;
        }

        protected override CoreResponse Remove(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Upload(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }
    }
}
