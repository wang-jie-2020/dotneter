using System.ComponentModel;

namespace Sms.ChinaMobile.Internal.Model
{
    public class ChinaMobileHttpRsp
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public string Rspcod { get; set; }

        /// <summary>
        /// 消息批次号，由云MAS平台生成，用于验证短信提交报告和状态报告的一致性（取值msgGroup）注:如果数据验证不通过msgGroup为空
        /// </summary>
        public string MsgGroup { get; set; }

        public bool Success { get; set; }
    }

    public enum ChinaMobileHttpRspCode
    {
        [Description("无效mac")]
        IllegalMac = -1,
        [Description("非法消息")]
        InvalidMessage = -2,
        [Description("非法用户名或密码")]
        InvalidUsrOrPwd = -3,
        [Description("未找到签名")]
        NoSignId = -4,
        [Description("无效的签名")]
        IllegalSignId = -5,
        [Description("成功")]
        Success = 1,
        [Description("手机号超出最大上限（5000）")]
        TooManyMobiles = -6,
        [Description("非法的手机号")]
        InvalidMobile = -7,
        [Description("重复的手机号")]
        DupMobile = -8,
    }
}
