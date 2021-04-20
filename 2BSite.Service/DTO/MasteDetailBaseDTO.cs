using Core.Database.Repository;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Core.Infrastructure.DataTables.DtResponse;

namespace _2BSite.Service.DTO
{
    public class MasteDetailBaseDTO : BaseDTO
    {
        public int DX_ID { get; set; }

        public string DX_Status { get; set; }



        public virtual DBResult Validation()
        {
            DBResult dBResult = new DBResult();
            List<FieldError> fieldErrors = new List<FieldError>();

            Type type = this.GetType();
            foreach (var tpi in type.GetProperties())
            {
                var value = tpi.GetValue(this);
                if (tpi.IsDefined(typeof(Core.Infrastructure.DataTables.Attributes.RequireAttribute)))
                {
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                    {
                        var err = tpi.GetCustomAttribute<Core.Infrastructure.DataTables.Attributes.RequireAttribute>();
                        Core.Infrastructure.DataTables.DtResponse.FieldError fe = new Core.Infrastructure.DataTables.DtResponse.FieldError();
                        fe.name = tpi.Name;
                        fe.status = err.Msg;
                        fieldErrors.Add(fe);
                        continue;
                    }
                }
                if (tpi.IsDefined(typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute)))
                {
                    if (value != null || !string.IsNullOrEmpty(value.ToString()))
                    {

                        var max = tpi.GetCustomAttribute<System.ComponentModel.DataAnnotations.MaxLengthAttribute>();
                        string result = value.ToString();
                        if (result.Length > max.Length)
                        {
                            Core.Infrastructure.DataTables.DtResponse.FieldError fe = new Core.Infrastructure.DataTables.DtResponse.FieldError();
                            fe.name = tpi.Name;
                            fe.status = "最大长度定义是：" + max.Length.ToString() + "，字段超出长度定义";
                            fieldErrors.Add(fe);
                            continue;
                        }
                    }
                }
                if (tpi.IsDefined(typeof(System.ComponentModel.DataAnnotations.MinLengthAttribute)))
                {
                    if (value != null || !string.IsNullOrEmpty(value.ToString()))
                    {

                        var min = tpi.GetCustomAttribute<System.ComponentModel.DataAnnotations.MinLengthAttribute>();
                        string result = value.ToString();
                        if (result.Length < min.Length)
                        {
                            Core.Infrastructure.DataTables.DtResponse.FieldError fe = new Core.Infrastructure.DataTables.DtResponse.FieldError();
                            fe.name = tpi.Name;
                            fe.status = "最小长度定义是：" + min.Length.ToString() + "，字段未达到指定长度";
                            fieldErrors.Add(fe);
                            continue;
                        }
                    }
                }
            }

            if (fieldErrors.Count > 0)
            {
                dBResult.Code = 4000;
                dBResult.Result = fieldErrors;
            }

            return dBResult;
        }
    }
}
