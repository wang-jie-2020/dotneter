using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    internal class ConsoleProgressBar
    {
        void Method1()
        {
            int total = 100; // 进度条的总长度
            int progress = 0; // 当前进度

            Console.WriteLine("开始进度条...");

            while (progress < total)
            {
                // 清除当前行
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth)); // 用空格覆盖当前行
                Console.SetCursorPosition(0, Console.CursorTop); // 重置光标位置到行首

                // 绘制进度条
                Console.Write("[");
                int percent = (int)(((float)progress / total) * 100);
                Console.ForegroundColor = ConsoleColor.Green; // 设置进度条颜色为绿色
                Console.Write(new string('#', progress)); // 绘制完成的进度部分
                Console.ResetColor(); // 重置颜色
                Console.Write(new string('-', total - progress)); // 绘制未完成的进度部分
                Console.Write("] {0}%", percent); // 显示百分比
                Console.WriteLine(); // 换行

                progress++; // 增加进度
                Thread.Sleep(100); // 暂停100毫秒，模拟工作进度
            }

            Console.WriteLine("完成！");
        }
    }
}
