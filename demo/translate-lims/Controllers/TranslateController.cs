using Microsoft.AspNetCore.Mvc;

namespace Translate.Controllers
{
    [ApiController]
    [Route("translate")]
    public class TranslateController : ControllerBase
    {
        //private readonly LibreTranslate.Net.LibreTranslate _libreTranslate;

        //public TranslateController(LibreTranslate.Net.LibreTranslate libreTranslate)
        //{
        //    _libreTranslate = libreTranslate;
        //}

        //[HttpGet]
        //public async Task<object> GetSupportedLanguage()
        //{
        //    IEnumerable<SupportedLanguages> supportedLanguages = await _libreTranslate.GetSupportedLanguagesAsync();
        //    return supportedLanguages;
        //}

        //[HttpPost]
        //[Route("test1")]
        //public async Task<object> TranslateTest1()
        //{
        //    var englishText = "Hello World!";
        //    string chineseText = await _libreTranslate.TranslateAsync(new LibreTranslate.Net.Translate()
        //    {
        //        //ApiKey = "MySecretApiKey",
        //        Source = LanguageCode.English,
        //        Target = LanguageCode.Chinese,
        //        Text = englishText
        //    });

        //    return chineseText;
        //}

        //[HttpPost]
        //[Route("test2")]
        //public async Task<object> TranslateTest2()
        //{
        //    var chineseText = "你好";
        //    string englishText = await _libreTranslate.TranslateAsync(new LibreTranslate.Net.Translate()
        //    {
        //        //ApiKey = "MySecretApiKey",
        //        Source = LanguageCode.English,
        //        Target = LanguageCode.Chinese,
        //        Text = chineseText
        //    });

        //    return englishText;
        //}
    }
}
