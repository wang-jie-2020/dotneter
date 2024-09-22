using Volo.Abp.MultiTenancy;
using Yi.AspNetCore.System.Entities;
using Yi.AspNetCore.System.Loggings;

namespace Yi.Admin.Domains.OperLogging.Entities;

/// <summary>
///     操作日志表
/// </summary>
[SugarTable("OperLog")]
public class OperLogEntity : SimpleEntity<Guid>, IMultiTenant
{
    /// <summary>
    ///     操作模块
    /// </summary>
    [SugarColumn(ColumnName = "Title")]
    public string? Title { get; set; }

    /// <summary>
    ///     操作类型
    /// </summary>
    [SugarColumn(ColumnName = "OperType")]
    public OperLogEnum OperType { get; set; }

    /// <summary>
    ///     请求方法
    /// </summary>
    [SugarColumn(ColumnName = "RequestMethod")]
    public string? RequestMethod { get; set; }

    /// <summary>
    ///     操作人员
    /// </summary>
    [SugarColumn(ColumnName = "OperUser")]
    public string? OperUser { get; set; }

    /// <summary>
    ///     操作Ip
    /// </summary>
    [SugarColumn(ColumnName = "OperIp")]
    public string? OperIp { get; set; }

    /// <summary>
    ///     操作地点
    /// </summary>
    [SugarColumn(ColumnName = "OperLocation")]
    public string? OperLocation { get; set; }

    /// <summary>
    ///     操作方法
    /// </summary>
    [SugarColumn(ColumnName = "Method")]
    public string? Method { get; set; }

    /// <summary>
    ///     请求参数
    /// </summary>
    [SugarColumn(ColumnName = "RequestParam")]
    public string? RequestParam { get; set; }

    /// <summary>
    ///     请求结果
    /// </summary>
    [SugarColumn(ColumnName = "RequestResult", Length = 9999)]
    public string? RequestResult { get; set; }

    /// <summary>
    ///     请求时间
    /// </summary>
    public DateTime ExecutionTime { get; set; }

    public Guid? TenantId { get; set; }
}