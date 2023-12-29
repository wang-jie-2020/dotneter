using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Helpers;

namespace Wechat
{
    [Route("api/[controller]/[action]")]
    public class WechatController : Controller
    {
        //与微信公众账号后台的Token设置保持一致，区分大小写。
        protected readonly string Token = Config.SenparcWeixinSetting.MpSetting.Token;

        //与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        protected readonly string EncodingAesKey = Config.SenparcWeixinSetting.MpSetting.EncodingAESKey;

        //与微信公众账号后台的AppId设置保持一致，区分大小写。
        protected readonly string AppId = Config.SenparcWeixinSetting.MpSetting.WeixinAppId;

        //与微信公众账号后台的AppId设置保持一致，区分大小写。
        protected readonly string AppSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

        private readonly IWechatAuthorizeService _wechatAuthorizeService;

        public WechatController(IWechatAuthorizeService wechatAuthorizeService)
        {
            _wechatAuthorizeService = wechatAuthorizeService;
        }

        /// <summary>
        /// 微信后台验证地址（使用Get）
        /// 微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echo)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echo); //返回随机字符串则表示验证通过
            }

            return Content("failed:" +
                           postModel.Signature + "," +
                           Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                           "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
        }

        /// <summary>
        /// 获取给UI使用的JsSDK信息包
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetJsSdkUiPackage")]
        public async Task<JsonResult> GetJsSdkUiPackage(string url)
        {
            var jsSdkUiPackage = await JSSDKHelper.GetJsSdkUiPackageAsync(AppId, AppSecret, url);
            return Json(jsSdkUiPackage);
        }

        /// <summary>
        /// 微信授权的Code登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Login")]
        public async Task<ActionResult> Login(string code)
        {
            var result = await OAuthApi.GetAccessTokenAsync(AppId, AppSecret, code);
            if (result == null || result.errcode != ReturnCode.请求成功)
            {
                throw new Exception(result?.errmsg);
            }

            var token = await _wechatAuthorizeService.Authorize(result.openid);
            return Json(new
            {
                AccessToken = token
            });
        }
    }
}
