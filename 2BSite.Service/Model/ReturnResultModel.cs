using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.Model
{
    public class ReturnResultModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
