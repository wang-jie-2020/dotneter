using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace npoi_draw
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var demo = new NPOIChart();
            demo.RunDemo();
        }
    }
}
