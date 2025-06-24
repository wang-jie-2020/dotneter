using Yi.AspNetCore;
using Yi.AspNetCore.MultiTenancy;
using Yi.Framework.Loggings;
using Yi.Framework.SqlSugarCore;

namespace Yi.System.Monitor.Entities;

/// <summary>
///     操作日志表
/// </summary>
[SugarTable("Sys_OperLog")]
public class OperLogEntity : Entity<long>, IMultiTenant
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