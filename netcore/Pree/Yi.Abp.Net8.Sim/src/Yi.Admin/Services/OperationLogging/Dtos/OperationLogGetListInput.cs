using Yi.AspNetCore.System;
using Yi.AspNetCore.System.Logging;

namespace Yi.Admin.Services.OperationLogging.Dtos;

public class OperationLogGetListInput : PagedInfraInput
{
    public OperationLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}