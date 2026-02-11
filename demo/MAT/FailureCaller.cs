using main_failure;
using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAT
{
    public class FailureCaller : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //%% 输入数据 - 请根据实际情况修改这些数据
            //% 正常Pack数据（右删失数据）
            //% 格式: [服役时间(月), 数量]
            //        normal_time = [34; 33; 32; 31; 30; 29; 28; 27; 26; 25; 24; 23; 22; 21; 20; 19; 16; 15; 14; 7];
            //        normal_num = [915; 3902; 6067; 6197; 3873; 4650; 3399; 4743; 2847; 3756; 3856; 3731; 5949; 5650; 2737; 8674; 1387; 7045; 220; 12];

            //% 失效Pack数据（区间删失数据）
            //% 格式: [寿命下限(月), 寿命上限(月), 失效数量]
            //% 注意: 如果确切知道失效时间，可设上下限相同
            //failure_time_low = [2; 4; 6; 7; 8; 9; 14; 15; 16; 18; 21; 22; 25; 28; 31];
            //        failure_time_up = [3; 5; 7; 8; 9; 10; 15; 16; 17; 19; 22; 23; 26; 29; 32];
            //        failure_num = [1; 9; 2; 3; 6; 8; 3; 1; 1; 1; 1; 1; 4; 2; 2];

            //% 预测年限
            //t = 20;

            //[F, F_up, annual_F, annual_F_up] = main_failure(normal_time, normal_num, failure_time_low, failure_time_up, failure_num);

            // 陈瑞说是参数是MATLAB行数组
            double[,] normal_time = { { 34 }, { 33 }, { 32 }, { 31 }, { 30 }, { 29 }, { 28 }, { 27 }, { 26 }, { 25 }, { 24 }, { 23 }, { 22 }, { 21 }, { 20 }, { 19 }, { 16 }, { 15 }, { 14 }, { 7 } };
            double[,] normal_num = { { 915 }, { 3902 }, { 6067 }, { 6197 }, { 3873 }, { 4650 }, { 3399 }, { 4743 }, { 2847 }, { 3756 }, { 3856 }, { 3731 }, { 5949 }, { 5650 }, { 2737 }, { 8674 }, { 1387 }, { 7045 }, { 220 }, { 12 } };


            //double[,] failure_time_low = { { 2 }, { 4 }, { 6 }, { 7 }, { 8 }, { 9 }, { 14 }, { 15 }, { 16 }, { 18 }, { 21 }, { 22 }, { 25 }, { 28 }, { 31 } };
            //double[,] failure_time_up = { { 3 }, { 5 }, { 7 }, { 8 }, { 9 }, { 10 }, { 15 }, { 16 }, { 17 }, { 19 }, { 22 }, { 23 }, { 26 }, { 29 }, { 32 } };
            //double[,] failure_num = { { 1 }, { 9 }, { 2 }, { 3 }, { 6 }, { 8 }, { 3 }, { 1 }, { 1 }, { 1 }, { 1 }, { 1 }, { 4 }, { 2 }, { 2 } };

            //double[] normal_time = { 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 16, 15, 14, 7 };
            //double[] normal_num = { 915, 3902, 6067, 6197, 3873, 4650, 3399, 4743, 2847, 3756, 3856, 3731, 5949, 5650, 2737, 8674, 1387, 7045, 220, 12 };

            double[] failure_time_low = { 2, 4, 6, 7, 8, 9, 14, 15, 16, 18, 21, 22, 25, 28, 31 };
            double[] failure_time_up = { 3, 5, 7, 8, 9, 10, 15, 16, 17, 19, 22, 23, 26, 29, 32 };
            double[] failure_num = { 1, 9, 2, 3, 6, 8, 3, 1, 1, 1, 1, 1, 4, 2, 2 };

            //MainFailure mainFailure = new MainFailure();

            failure_pre mainFailure = new failure_pre();

            MWNumericArray input1 = new MWNumericArray(normal_time);
            MWNumericArray input2 = new MWNumericArray(normal_num);
            MWNumericArray input3 = new MWNumericArray(failure_time_low);
            MWNumericArray input4 = new MWNumericArray(failure_time_up);
            MWNumericArray input5 = new MWNumericArray(failure_num);

            MWArray[] results = mainFailure.main_failure(4, input1, input2, input3, input4, input5);

            Array array = results.ToArray();

            return Task.CompletedTask;
        }
    }
}
