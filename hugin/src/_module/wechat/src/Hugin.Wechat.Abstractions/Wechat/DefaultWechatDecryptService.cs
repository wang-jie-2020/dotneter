using System.Threading.Tasks;

namespace Wechat
{
    public class DefaultWechatDecryptService : IWechatDecryptService
    {
        public async Task DecryptUser(string data)
        {
            await Task.CompletedTask;
        }

        public async Task DecryptRunData(string identifier, string data)
        {
            await Task.CompletedTask;
        }
    }
}
