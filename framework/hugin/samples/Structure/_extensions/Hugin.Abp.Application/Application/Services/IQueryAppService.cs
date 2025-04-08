using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Hugin.Application.Services
{
    public interface IQueryAppService<TEntity, TGetOutputDto, in TKey, in TGetListInput>
        : IQueryAppService<TEntity, TGetOutputDto, TGetOutputDto, TKey, TGetListInput>
    {
    }


    public interface IQueryAppService<TEntity, TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput>
    {
        Task<TGetOutputDto> GetAsync(TKey id);

        Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);
    }
}



