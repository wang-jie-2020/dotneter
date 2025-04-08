using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Entities.Request;
using Senparc.Weixin.WxOpen.Helpers;

namespace Wechat
{
    [Route("api/[controller]/[action]")]
    public class WechatOpenController : Controller
    {
        //与微信小程序后台的Token设置保持一致，区分大小写。
        protected readonly string Token = Config.SenparcWeixinSetting.WxOpenToken;

        //与微信小程序后台的EncodingAESKey设置保持一致，区分大小写。
        protected readonly string EncodingAesKey = Config.SenparcWeixinSetting.WxOpenEncodingAESKey;

        //与微信小程序后台的AppId设置保持一致，区分大小写。
        protected readonly string WxOpenAppId = Config.SenparcWeixinSetting.WxOpenAppId;

        //与微信小程序账号后台的AppId设置保持一致，区分大小写。
        protected readonly string WxOpenAppSecret = Config.SenparcWeixinSetting.WxOpenAppSecret;

        private readonly IWechatAuthorizeService _wechatAuthorizeService;

        public WechatOpenController(IWechatAuthorizeService wechatAuthorizeService)
        {
            _wechatAuthorizeService = wechatAuthorizeService;
        }

        /// <summary>
        /// GET请求用于处理微信小程序后台的URL验证
        /// </summary>
        /// <returns></returns>
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
                           CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                           "如果你在浏览器中看到这句话，说明此地址可以被作为微信小程序后台的Url，请注意保持Token一致。");
        }

        /// <summary>
        /// wx.login登陆成功之后发送的请求
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnLogin(string code)
        {
            try
            {
                var jsonResult = SnsApi.JsCode2Json(WxOpenAppId, WxOpenAppSecret, code);
                if (jsonResult.errcode == ReturnCode.请求成功)
                {
                    var unionId = "";
                    var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, unionId);

                    return Json(new { success = true, msg = "OK", sessionId = sessionBag.Key });
                }

                return Json(new { success = false, msg = jsonResult.errmsg });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

        }

        [HttpPost]
        [ActionName("CheckWxOpenSignature")]
        public ActionResult CheckWxOpenSignature(string sessionId, string rawData, string signature)
        {
            try
            {
                var checkSuccess = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.CheckSignature(sessionId, rawData, signature);
                return Json(new { success = checkSuccess, msg = checkSuccess ? "签名校验成功" : "签名校验失败" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        /// <summary>
        /// 微信小程序登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Login")]
        public async Task<ActionResult> Login(string code)
        {
            JsCode2JsonResult result = await Senparc.Weixin.WxOpen.AdvancedAPIs.Sns.SnsApi.JsCode2JsonAsync(WxOpenAppId, WxOpenAppSecret, code);
            if (result == null || result.errcode != ReturnCode.请求成功)
            {
                throw new Exception(result?.errmsg);
            }

            var unionId = "";
            var sessionBag = await SessionContainer.UpdateSessionAsync(null, result.openid, result.session_key, unionId);

            var token = await _wechatAuthorizeService.Authorize(result.openid);

            return Json(new
            {
                sessionId = sessionBag.Key,
                AccessToken = token
            });
        }

        /// <summary>
        /// 微信加密手机登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("LoginByPhone")]
        public async Task<ActionResult> LoginByPhone(WeChatEncryptedData encrypted)
        {
            var sessionBag = await SessionContainer.GetSessionAsync(encrypted.SessionId);
            var data = EncryptHelper.DecryptPhoneNumber(encrypted.SessionId,
                encrypted.EncryptedData, encrypted.Iv);

            var token = await _wechatAuthorizeService.Authorize(sessionBag.OpenId, data.phoneNumber);

            return Json(new
            {
                sessionId = sessionBag.Key,
                AccessToken = token
            });
        }
    }
}