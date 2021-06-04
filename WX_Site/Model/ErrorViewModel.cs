using System;
using System.Collections.Generic;
using System.Text;

namespace WX_Site.Model
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
