using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace WX_CommonService.TemplateMessage.WxOpen
{
    public class WxOpenTemplateMessage_OverdueReminder: TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }

        /// <summary>
        /// 租赁逾期通知模版
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <param name="productName">商品名字</param>
        /// <param name="message">逾期提示</param>
        /// <param name="rentmessage">租赁产品</param>
        /// <param name="url"></param>
        /// <param name="templateId"></param>
        public WxOpenTemplateMessage_OverdueReminder(string orderNumber, string productName,
             string message, string rentmessage,
            string url,
            //根据实际的“模板ID”进行修改
            string templateId = "Y21l1ZtTY2Kd6jxBrp5lkbDxFKB6mIHIk9DVFNS0Mb4")
            : base(templateId, url, "租赁逾期通知")
        {
            /* 
                关键词
                订单编号 {{keyword1.DATA}}
                商品名字 {{keyword2.DATA}}
                逾期提示 {{keyword3.DATA}}
                租赁产品 {{keyword4.DATA}}

                */

            keyword1 = new TemplateDataItem(orderNumber);
            keyword2 = new TemplateDataItem(productName);
            keyword3 = new TemplateDataItem(message);
            keyword4 = new TemplateDataItem(rentmessage);
        }
    }
}
