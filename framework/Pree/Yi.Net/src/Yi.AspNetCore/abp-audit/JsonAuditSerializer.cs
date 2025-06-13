using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing;

public class JsonAuditSerializer : IAuditSerializer, ITransientDependency
{
    protected AbpAuditingOptions Options;

    public JsonAuditSerializer(IOptions<AbpAuditingOptions> options)
    {
        Options = options.Value;
    }

    public string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj, CreateJsonSerializerOptions());
    }

    private static readonly ConcurrentDictionary<string, JsonSerializerOptions> JsonSerializerOptionsCache =
        new ConcurrentDictionary<string, JsonSerializerOptions>();

    protected virtual JsonSerializerOptions CreateJsonSerializerOptions()
    {
        return JsonSerializerOptionsCache.GetOrAdd(nameof(JsonAuditSerializer), _ =>
        {
            var settings = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                {
                }
            };

            return settings;
        });
    }
}
