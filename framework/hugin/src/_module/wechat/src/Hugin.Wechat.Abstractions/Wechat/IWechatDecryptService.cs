using System.Threading.Tasks;

namespace Wechat
{
    public interface IWechatDecryptService
    {
        /// <summary>
        /// UserInfo
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task DecryptUser(string data);

        /// <summary>
        ///  RunInfo
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task DecryptRunData(string identifier, string data);
    }
}
