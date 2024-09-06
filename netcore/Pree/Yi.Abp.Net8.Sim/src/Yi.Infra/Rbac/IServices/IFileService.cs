using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;
using Yi.Abp.Infra.Rbac.Dtos.FileManager;

namespace Yi.Abp.Infra.Rbac.IServices
{
    public interface IFileService : IApplicationService
    {
        Task<string> GetReturnPathAsync(Guid code, bool? isThumbnail);
        Task<List<FileGetListOutputDto>> Post(IFormFileCollection file);
    }
}
