﻿using Newtonsoft.Json;

namespace Yi.Framework.Authentication.OAuth;

public class AuthticationErrCodeModel
{
    public string error { get; set; }

    public string error_description { get; set; }

    public static void VerifyErrResponse(string content)
    {
        var model = JsonConvert.DeserializeObject<AuthticationErrCodeModel>(content);
        if (model.error != null) throw new Exception($"第三方授权返回错误，错误码：【{model.error}】，错误详情：【{model.error_description}】");
    }
}