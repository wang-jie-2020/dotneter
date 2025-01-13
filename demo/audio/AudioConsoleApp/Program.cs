using NAudio.Wave;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AudioConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1:wavein");
                Console.WriteLine("2:wsapi");
                Console.WriteLine("3:音频");
                Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
                string input = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (input == "0")
                {
                    break;
                }

                Type? type = MethodBase.GetCurrentMethod()?.DeclaringType;
                if (type != null)
                {
                    var method = type.GetMethod("Method" + input,
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

                    method?.Invoke(null, null);
                }
            }
        }

        /// <summary>
        ///  Wave
        /// </summary>
        static void Method1()
        {
            var convert = new NAudioConverter2(1);
            convert.RunOnce();
        }

        /// <summary>
        /// Wsapi
        /// </summary>
        static void Method2()
        {
            var convert = new NAudioConverter2(2);
            convert.RunOnce();
        }

        /// <summary>
        /// 音频输出
        /// </summary>
        static void Method3()
        {
            var convert = new NAudioConverter2(3);
            convert.RunOnce();
        }

        /// <summary>
        ///  混合1 2 会怎么样,感觉效果很差
        /// </summary>
        static void Method9()
        {
            var capture = new WaveInEvent();
            capture.WaveFormat = new WaveFormat(16000, 16, 1);

            BufferedWaveProvider bufferedWaveProvider = new BufferedWaveProvider(capture.WaveFormat);
            SavingWaveProvider savingWaveProvider = new SavingWaveProvider(bufferedWaveProvider, "temp.wav");

            capture.DataAvailable += (_s, _e) =>
            {
                bufferedWaveProvider.AddSamples(_e.Buffer, 0, _e.BytesRecorded);
            };

            capture.RecordingStopped += (_s, _e) =>
            {
                capture.Dispose();
            };

            WaveOutEvent player = new WaveOutEvent();
            player.Init(savingWaveProvider);

            player.Play();
            capture.StartRecording();
        }
    }
}