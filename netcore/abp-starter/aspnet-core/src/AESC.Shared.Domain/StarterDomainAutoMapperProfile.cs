using Lion.AbpPro.DataDictionaryManagement.DataDictionaries.Aggregates;
using Lion.AbpPro.DataDictionaryManagement.DataDictionaries.Dto;

namespace AESC.Starter
{
    public class StarterDomainAutoMapperProfile : Profile
    {
        public StarterDomainAutoMapperProfile()
        {
            //由于作者的字段模块未添加AddMaps,这里补充添加
            CreateMap<DataDictionary, DataDictionaryDto>();
            CreateMap<DataDictionaryDetail, DataDictionaryDetailDto>();
        }
    }
}
