namespace Hugin.Application.Services
{
    public interface ICrudAppService<TEntity, TGetOutputDto, TGetEditOutputDto, TKey, in TGetListInput, in TCreateOrUpdateInput>
        : ICrudAppService<TEntity, TGetOutputDto, TGetOutputDto, TGetEditOutputDto, TKey, TGetListInput, TCreateOrUpdateInput, TCreateOrUpdateInput>
            where TKey : struct
    {

    }

    public interface ICrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TGetEditOutputDto, TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IQueryAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>,
            IEditAppService<TGetOutputDto, TGetEditOutputDto, TKey, TCreateInput, TUpdateInput>
                where TKey : struct
    {

    }
}