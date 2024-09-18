using Mapster;
using Volo.Abp.ObjectMapping;

namespace Yi.AspNetCore.Mapster;

public class MapsterObjectMapper : IObjectMapper
{
    public IAutoObjectMappingProvider AutoObjectMappingProvider => throw new NotImplementedException();

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TDestination>();
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return source.Adapt(destination);
    }
}