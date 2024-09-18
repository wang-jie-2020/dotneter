using Mapster;
using Volo.Abp.ObjectMapping;

namespace Yi.AspNetCore.Mapster;

public class MapsterAutoObjectMappingProvider : IAutoObjectMappingProvider
{
    public TDestination Map<TSource, TDestination>(object source)
    {
        var sss = typeof(TDestination).Name;
        return source.Adapt<TDestination>();
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return source.Adapt(destination);
    }
}