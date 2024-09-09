using Volo.Abp.ObjectMapping;

namespace Yi.Framework.Mapster;

public class MapsterObjectMapper : IObjectMapper
{
    public IAutoObjectMappingProvider AutoObjectMappingProvider => throw new NotImplementedException();

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        throw new NotImplementedException();
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        throw new NotImplementedException();
    }
}