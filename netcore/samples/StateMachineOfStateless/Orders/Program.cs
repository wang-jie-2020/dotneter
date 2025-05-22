// namespace Demo;
//
// class Program
// {
//     static void Main(string[] args)
//     {
//         OrderState orderState = OrderState.OrderCreate;
//         var b = GetTrigger(ref orderState, out int numTrigger, out OrderState newOrderState);
//         while (b)
//         {
//             b = GetTrigger(ref newOrderState, out numTrigger, out newOrderState);
//         }
//
//         Console.WriteLine("结束");
//         Console.ReadKey();
//     }
//
//     public static bool GetTrigger(ref OrderState orderState, out int numTrigger, out OrderState newOrderState)
//     {
//         while (true)
//         {
//             Console.WriteLine($"请输入对订单状态的操作：//n Jump: 0、Cancel: 1、Payment:2、Send: 3、Sign: 4、Refund: 5");
//             numTrigger = Convert.ToInt32(Console.ReadLine());
//             if (numTrigger >= 0 && numTrigger < 7)
//             {
//                 break;
//             }
//         }
//
//         // 初始化状态机
//         OrderStateMachine stateMachine = new OrderStateMachine(orderState);
//         // 初始化状态机调用器
//         StateMachineInvoker stateMachineInvoker = new StateMachineInvoker(stateMachine);
//         var flag = stateMachineInvoker.UpdateOrderState((OrderTrigger)numTrigger, out newOrderState);
//         return flag;
//     }
// }