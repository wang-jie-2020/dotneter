using System.Threading.Tasks;

namespace Hugin.Application.Services
{
    public interface IEditAppService<TGetOutputDto, TGetEditOutputDto, TKey, in TCreateOrUpdateInput>
    : IEditAppService<TGetOutputDto, TGetEditOutputDto, TKey, TCreateOrUpdateInput, TCreateOrUpdateInput>
        where TKey : struct
    {

    }

    public interface IEditAppService<TGetOutputDto, TGetEditOutputDto, TKey, in TCreateInput, in TUpdateInput>
        where TKey : struct
    {
        Task<TGetEditOutputDto> GetForEdit(TKey? id);

        Task<TGetOutputDto> CreateAsync(TCreateInput input);

        Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input);

        Task DeleteAsync(TKey id);
    }
}
