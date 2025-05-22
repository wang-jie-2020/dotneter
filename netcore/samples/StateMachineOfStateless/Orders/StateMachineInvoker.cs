namespace Demo.Orders;

internal class StateMachineInvoker
{
    private readonly OrderStateMachine _stateMachine;

    public StateMachineInvoker(OrderStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public bool UpdateOrderState(OrderTrigger trigger, out OrderState orderState)
    {
        bool flag;
        var machine = _stateMachine.GetStateMachine();
        try
        {
            Console.WriteLine($"修改前的状态为： {machine.State}");
            if (machine.CanFire(trigger))
            {
                // 状态变换，状态变换至 trigger 态
                machine.Fire(trigger);
                // 此时状态已经发生了变换，下一个状态成立当前状态
                Console.WriteLine($"当前状态为： {machine.State}");
                if (machine.State == OrderState.Invalided)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            else
            {
                Console.WriteLine($"与Stateless配置流冲突，不能由 {machine.State} 状态直接变换到 {trigger} 状态。");
                flag = false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"错误： {e.Message}");
            flag = false;
        }

        orderState = machine.State;
        return flag;
    }
}