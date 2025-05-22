namespace Demo.Orders;

public enum OrderState
{
    /// <summary>
    /// 创建
    /// </summary>
    OrderCreate=0,
 
    /// <summary>
    /// 关闭
    /// </summary>
    Invalided=1,
 
    /// <summary>
    /// 待支付
    /// </summary>
    PendingSign=2,
 
    /// <summary>
    /// 待发货
    /// </summary>
    PendingSend=3,
 
    /// <summary>
    /// 待收货
    /// </summary>
    PendingReceipt=4,
 
    /// <summary>
    /// 待退款
    /// </summary>
    PendingRefund=5,
 
    /// <summary>
    /// 完成
    /// </summary>
    Completed=6,
}