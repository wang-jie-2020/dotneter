namespace Yi.AspNetCore.Mvc.OperLogging;

[AttributeUsage(AttributeTargets.Method)]
public class OperLogAttribute : Attribute
{
    public OperLogAttribute(string title, OperLogEnum operLogType)
    {
        Title = title;
        OperLogType = operLogType;
    }

    /// <summary>
    ///     操作类型
    /// </summary>
    public OperLogEnum OperLogType { get; set; }

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