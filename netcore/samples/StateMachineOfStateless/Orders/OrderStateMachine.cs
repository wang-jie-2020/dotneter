using Stateless;

namespace Demo.Orders;

internal class OrderStateMachine
{
    private readonly StateMachine<OrderState, OrderTrigger> _stateMachine;
    public OrderStateMachine(OrderState orderState)
    {
        //因为 Stateless的状态可能来自ORM 等外部环境，所以初始化状态机时接收一个来自外界的状态 orderState 作为当前状态
        _stateMachine = new StateMachine<OrderState, OrderTrigger>(orderState);
        ConfigureStateMachine();
    }
    
    /// <summary>
    /// 配置流程
    /// </summary>
    private void ConfigureStateMachine()
    {
        //订单 => 待支付
        _stateMachine.Configure(OrderState.OrderCreate)
            .Permit(OrderTrigger.Jump, OrderState.PendingSign)
            .Permit(OrderTrigger.Cancel,OrderState.Invalided);
        // 待支付 => 代发货/关闭
        _stateMachine.Configure(OrderState.PendingSign)
            .Permit(OrderTrigger.Payment, OrderState.PendingSend)
            .Permit(OrderTrigger.Cancel, OrderState.Invalided);
        // 代发货 => 待收货/待退款
        _stateMachine.Configure(OrderState.PendingSend)
            .Permit(OrderTrigger.Send, OrderState.PendingReceipt)
            .Permit(OrderTrigger.Cancel,OrderState.PendingRefund);
        // 待退款 => 关闭
        _stateMachine.Configure(OrderState.PendingRefund)
            .Permit(OrderTrigger.Refund, OrderState.Invalided);
        // 待收货 => 完成
        _stateMachine.Configure(OrderState.PendingReceipt)
            .Permit(OrderTrigger.Sign, OrderState.Completed);
    }
    
    /// <summary>
    /// 获取当前状态的方法
    /// </summary>
    public StateMachine<OrderState, OrderTrigger> GetStateMachine()
    {
        return _stateMachine;
    }
}