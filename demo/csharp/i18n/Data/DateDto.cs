﻿using Newtonsoft.Json;

namespace i18n.Controllers;

public class DateTimeDto
{
    [JsonProperty(Order = 1)]
    public DateTime DateTimeNow { get; set; }
    
    [JsonProperty(Order = 2)]
    public DateTime DateTimeUtcNow { get; set; }
    
    [JsonProperty(Order = 3)]
    public DateTime DateTimeUnspecified { get; set; }

}

public class InputDateTime
{
    [JsonProperty(Order = 1)]
    public DateTime DateTime1 { get; set; }
    
    [JsonProperty(Order = 2)]
    public DateTime DateTime2 { get; set; }
    
    [JsonProperty(Order = 3)]
    public DateTime DateTime3 { get; set; }

}

public class DateTimeOffsetDto
{
    [JsonProperty(Order = 1)]
    public DateTimeOffset DateTimeOffsetNow { get; set; }
    
    [JsonProperty(Order = 2)]
    public DateTimeOffset DateTimeOffsetUtcNow { get; set; }
}