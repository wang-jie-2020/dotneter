using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.ObjectMapping;

namespace Yi.Web.Controllers;

[ApiController]
[Route("dev-api")]
public class DevController:AbpController
{
    private readonly IObjectMapper _objectMapper;
    private readonly IAutoObjectMappingProvider _autoObjectMappingProvider;

    public DevController(
        IObjectMapper objectMapper,
        IAutoObjectMappingProvider autoObjectMappingProvider)
    {
        _objectMapper = objectMapper;
        _autoObjectMappingProvider = autoObjectMappingProvider;
    }

    [HttpGet("mappers")]
    public void Mappers()
    {
        
    }
}