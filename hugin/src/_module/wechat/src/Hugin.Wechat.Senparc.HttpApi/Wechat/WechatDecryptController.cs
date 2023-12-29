using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Senparc.Weixin.Annotations;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Helpers;

namespace Wechat
{
    [Route("api/[controller]/[action]")]
    public class WechatDecryptController : Controller
    {
        private readonly IWechatDecryptService _wechatDecryptService;

        public WechatDecryptController(IWechatDecryptService wechatDecryptService)
        {
            _wechatDecryptService = wechatDecryptService;
        }

        /// <summary>
        /// 解密姓名等信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("DecryptUserInfo")]
        public async Task DecryptUserInfo(WeChatEncryptedData encrypted)
        {
            var sessionBag = await SessionContainer.GetSessionAsync(encrypted.SessionId);

            var data = EncryptHelper.DecodeUserInfoBySessionId(encrypted.SessionId, encrypted.EncryptedData, encrypted.Iv);
            await _wechatDecryptService.DecryptUser(JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// 解密运动数据
        /// </summary>
        /// <param name="identifier">数据标识</param>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("DecryptRunData")]
        public async Task DecryptRunData([CanBeNull][FromQuery] string identifier, WeChatEncryptedData encrypted)
        {
            var sessionBag = await SessionContainer.GetSessionAsync(encrypted.SessionId);

            var data = EncryptHelper.DecryptRunData(encrypted.SessionId, encrypted.EncryptedData, encrypted.Iv);
            await _wechatDecryptService.DecryptRunData(identifier, JsonConvert.SerializeObject(data));
        }
    }
}
