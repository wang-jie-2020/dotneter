namespace Yi.AspNetCore.Common;

public class OperationLogInfo
{
    /// <summary>
    ///     操作模块
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     操作类型
    /// </summary>
    public OperationLogEnum OperationLog { get; set; }

    /// <summary>
    ///     操作人员
    /// </summary>
    public string? Operator { get; set; }
    
    /// <summary>
    ///     操作方法
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    ///     请求方法
    /// </summary>
    public string? RequestMethod { get; set; }
    
    /// <summary>
    ///     请求参数
    /// </summary>
    public string? RequestParam { get; set; }

    /// <summary>
    ///     请求结果
    /// </summary>
    public string? Result { get; set; }
    
    public DateTime ExecutionTime { get; set; }
}