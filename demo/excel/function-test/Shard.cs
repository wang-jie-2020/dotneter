using System;
using System.IO;

namespace demo
{
    public static class Shard
    {
        // 4M - 9.5w cells - 1.8w rows
        public static string TinyFile = @"D:\远景\TVCDataAnalysis\demo\T21120107C-Power.xlsx";

        // 80M - 2400w cells - 21.6w row
        public static string MiddlingFile = @"D:\远景\TVCDataAnalysis\demo\2022-04-04@15-49-26_1-PACK-重卡-0FNPB122300101C360000012-25℃-容量和能量.xlsx";

        // 500M - (2400w cells - 21.6w row) * 4
        public static string LargeFile = @"D:\远景\TVCDataAnalysis\demo\2022-04-04@15-49-26_1-PACK-重卡-0FNPB122300101C360000012-25℃-容量和能量 - multi.xlsx";

        // 450M - 9616w cells - 86.6w row
        public static string LargeFile2 = @"D:\远景\TVCDataAnalysis\demo\2022-04-04@15-49-26_1-PACK-重卡-0FNPB122300101C360000012-25℃-容量和能量 - large.xlsx";

        // 330M - 7212w cells - 65w row
        public static string LargeFile3 = @"D:\远景\TVCDataAnalysis\demo\2022-04-04@15-49-26_1-PACK-重卡-0FNPB122300101C360000012-25℃-容量和能量 - large2.xlsx";

        // 260M - 4335w cells - 39w row
        public static string LargeFile4 = @"D:\远景\TVCDataAnalysis\demo\2022-04-04@15-49-26_1-PACK-重卡-0FNPB122300101C360000012-25℃-容量和能量 - large3.xlsx";

        public static string LargeFile5 = @"D:\远景\TVCDataAnalysis\demo\2022-04-04@15-49-26_1-PACK-重卡-0FNPB122300101C360000012-25℃-容量和能量 - large4.xlsx";

        //测试输出文件
        public static string FileOut = @"D:\远景\TVCDataAnalysis\demo\out.xlsx";

        //测试作图文件
        public static string DrawFile = @"D:\远景\TVCDataAnalysis\demo\011、制作序号_PACK_项目名称_样品编号_测试温度_容量和能量_Summary_制作日期.xlsx";

        public static string GetNewFileOutDir()
        {
            return Path.Combine(@"D:\远景\TVCDataAnalysis\demo", $"out -{DateTime.Now:yyMMddHHmmss}.xlsx");
        }
    }
}
