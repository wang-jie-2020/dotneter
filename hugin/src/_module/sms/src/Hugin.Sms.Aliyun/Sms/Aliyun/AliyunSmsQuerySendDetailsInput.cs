using System;

namespace Hugin.Sms.Aliyun
{
    public class AliyunSmsQuerySendDetailsInput
    {
        public string Mobile { get; set; }

        /// <summary>
        /// 短信发送日期，支持查询最近30天的记录。
        /// 格式为yyyyMMdd，例如20181225。
        /// </summary>
        public DateTime SendDate { get; set; }

        /// <summary>
        /// 分页查看发送记录，指定每页显示的短信记录数量。
        /// 取值范围为1 ~50。
        /// </summary>
        public int PageSize { get; set; } = 50;

        /// <summary>
        /// 分页查看发送记录，指定发送记录的的当前页码。
        /// </summary>
        public int CurrentPage { get; set; } = 1;
    }
}
