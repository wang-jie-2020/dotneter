namespace Demo.Orders;

public enum OrderTrigger
{
    /// <summary>
    /// 跳转
    /// </summary>
    Jump=0,
 
    /// <summary>
    /// 取消
    /// </summary>
    Cancel=1,
 
    /// <summary>
    /// 支付
    /// </summary>
    Payment=2,
 
    /// <summary>
    /// 配送
    /// </summary>
    Send=3,
 
    /// <summary>
    /// 签收
    /// </summary>
    Sign=4,
 
    /// <summary>
    /// 退款
    /// </summary>
    Refund=5,
}