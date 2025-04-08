using System.Threading.Tasks;

namespace Wechat
{
    public class DefaultWechatAuthorizeService : IWechatAuthorizeService
    {
        public async Task<string> Authorize(string openId, string phone = null)
        {
            return await Task.FromResult(string.Empty);
        }
    }
}
