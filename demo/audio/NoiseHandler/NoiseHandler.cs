namespace NoiseHandler
{
    public static class NoiseHandler
    {
        //这里似乎不太合适
        public static double[] kalman_Filter(string filePath, double dataNum, double R, double Q)
        {
            System.IO.StreamReader readerFile = new System.IO.StreamReader(filePath);//读取测量数据文件
            string str = readerFile.ReadToEnd();//获取文件中数据
            Array array = str.Split('\n');//数据分割
            //初始化参数
            int N = array.Length - 1;
            double[] y = new double[N];
            double average = 0, sum = 0, variance_sum = 0, variance = 0;

            //文件数据写进数组
            for (int a = 0; a < array.Length - 1; a++)
            {
                y[a] = Convert.ToDouble(array.GetValue(a));
            }

            //定义参数矩阵
            double[] K = new double[N];//卡尔曼增益矩阵
            double[] P = new double[N];//后验误差估计协方差矩阵
            double[] X = new double[N];//结果矩阵

            //有测量原始数据计算后验测量方差
            for (int n = 0; n < dataNum; n++)
            {
                sum += y[n];
            }
            average = sum / dataNum;//求均值
            for (int m = 0; m < dataNum; m++)
            {
                variance_sum += Math.Sqrt(Math.Abs(y[m] - average));
            }
            variance = variance_sum / dataNum;//求方差

            //矩阵初始化赋值
            X[0] = average;
            P[0] = variance;

            //滤波过程
            for (int i = 1; i < N; i++)
            {
                K[i] = P[i - 1] / (P[i - 1] + R);
                X[i] = X[i - 1] + K[i] * (y[i] - X[i - 1]);
                P[i] = P[i - 1] - K[i] * P[i - 1] + Q;
            }

            readerFile.Close();
            return X;
        }

        //这里似乎不太合适
        public static double[] kalmanFilter(double[] array, double R, double Q)
        {
            int N = array.Length;
            double average = 0, sum = 0, variance_sum = 0, variance = 0;
            double[] K = new double[N];
            double[] P = new double[N];
            double[] X = new double[N];
            for (int n = 0; n < N; n++)
            {
                sum += array[n];
            }
            average = sum / N;
            for (int m = 0; m < N; m++)
            {
                variance_sum += Math.Sqrt(Math.Abs(array[m] - average));
            }
            variance = variance_sum / N;

            //矩阵初始化赋值
            X[0] = average;
            P[0] = variance;

            //滤波过程
            for (int i = 1; i < N; i++)
            {
                K[i] = P[i - 1] / (P[i - 1] + R);
                X[i] = X[i - 1] + K[i] * (array[i] - X[i - 1]);
                P[i] = P[i - 1] - K[i] * P[i - 1] + Q;
            }
            return X;
        }

    }
}