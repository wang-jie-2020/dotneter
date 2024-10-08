﻿namespace Yi.Infra.Account.Options;

public class JwtOptions
{
    public string Issuer { get; set; } = "ccnetcore.com";

    public string Audience { get; set; } = "https//ccnetcore.com";

    public string SecurityKey { get; set; } =
        "892u4j1803qj23jro0fjkf8bmsdf9nb9mf92834u23jdf923jrnmvasbceqwt347562tgdhdnsv9wevbnop";

    public long ExpiresMinuteTime { get; set; } = 120;
}