using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;
using Yi.Infra.Rbac.Dtos.FileManager;

namespace Yi.Infra.Rbac.IServices;

public interface IFileService : IApplicationService
{
    Task<List<FileGetListOutputDto>> Post(IFormFileCollection file);
}