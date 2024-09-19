namespace Yi.AspNetCore.Common;

[AttributeUsage(AttributeTargets.Method)]
public class OperationLogAttribute : Attribute
{
    public OperationLogAttribute(string title, OperationLogEnum operationLogType)
    {
        Title = title;
        OperLogType = operationLogType;
    }

    /// <summary>
    ///     操作类型
    /// </summary>
    public OperationLogEnum OperLogType { get; set; }

    /// <summary>
    ///     日志标题（模块）
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///     是否保存请求数据
    /// </summary>
    public bool IsSaveRequestData { get; set; } = true;

    /// <summary>
    ///     是否保存返回数据
    /// </summary>
    public bool IsSaveResponseData { get; set; } = true;
}