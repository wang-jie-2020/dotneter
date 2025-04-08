using System.Threading.Tasks;

namespace Wechat
{
    /// <summary>
    /// 微信认证
    /// </summary>
    public interface IWechatAuthorizeService
    {
        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="openId">openId</param>
        /// <param name="phone">phone</param>
        /// <returns>access token</returns>
        Task<string> Authorize(string openId, string phone = null);
    }
}
