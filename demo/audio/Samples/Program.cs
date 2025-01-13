using System.Reflection;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
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

        static void Method1()
        {
            // 创建一个内存流对象ms，用于存储录制的音频数据
            MemoryStream ms = new MemoryStream();

            // 创建一个WaveInEvent对象waveInEvent，用于从麦克风捕获音频
            // 设置它的采样率为22050Hz，声道数为1，设备编号为0（默认设备）
            var waveInEvent = new NAudio.Wave.WaveInEvent();
            waveInEvent.WaveFormat = new NAudio.Wave.WaveFormat(22050, 1);
            waveInEvent.DeviceNumber = 0;

            // 创建一个WaveFileWriter对象waveFileWriter，用于将捕获的音频数据写入内存流ms
            // 设置它的波形格式与waveInEvent相同
            // 注意:在这里需要用NAudio.Utils.IgnoreDisposeStream来包装MemoryStream。因为避免在释放waveFileWriter时，同时释放内存流ms，从而导致无法播放录制的音频,如果这里用的不是MemoryStream,是普通的文件流(FileStream),千万不要用"NAudio.Utils.IgnoreDisposeStream"来包装，这样的话会导致无法保存录音文件到磁盘上。
            var waveFileWriter = new NAudio.Wave.WaveFileWriter(new NAudio.Utils.IgnoreDisposeStream(ms), waveInEvent.WaveFormat);

            // 为waveInEvent添加DataAvailable事件处理程序
            // 当有音频数据可用时，调用waveFileWriter的Write方法，将数据写入内存流ms
            waveInEvent.DataAvailable += (_s, _e) =>
            {
                Console.WriteLine(DateTime.Now.ToString("yyyyMMddhhmmss"));

                waveFileWriter.Write(_e.Buffer, 0, _e.BytesRecorded);
            };

            // 为waveInEvent添加RecordingStopped事件处理程序
            // 当录制停止时，释放waveFileWriter和waveInEvent对象，并将内存流ms的位置重置为0
            waveInEvent.RecordingStopped += (_s, _e) =>
            {
                waveFileWriter.Dispose();
                waveInEvent.Dispose();
                ms.Position = 0;

                // 创建一个WaveOutEvent对象waveOutEvent，用于播放内存流ms中的音频数据
                var waveOutEvent = new NAudio.Wave.WaveOutEvent();

                // 创建一个WaveFileReader对象waveReader，用于从内存流ms中读取音频数据
                var waveReader = new NAudio.Wave.WaveFileReader(ms);

                // 调用waveOutEvent的Init方法，将waveReader作为输入源
                waveOutEvent.Init(waveReader);

                // 调用waveOutEvent的Play方法，开始播放音频
                waveOutEvent.Play();
            };

            // 调用waveInEvent的StartRecording方法，开始录制音频
            waveInEvent.StartRecording();

            // 在控制台输出提示信息，并等待用户输入
            Console.WriteLine("正在录音，按回车键停止。");
            Console.ReadLine();

            // 调用waveInEvent的StopRecording方法，停止录制并触发RecordingStopped事件
            waveInEvent.StopRecording();

            // 在控制台输出提示信息，并等待用户输入
            //Console.WriteLine("按回车键退出");
            //Console.ReadLine();
        }

        static void Method111()
        {
            // 创建一个内存流对象ms，用于存储录制的音频数据
            MemoryStream ms = new MemoryStream();

            // 创建一个WaveInEvent对象waveInEvent，用于从麦克风捕获音频
            // 设置它的采样率为22050Hz，声道数为1，设备编号为0（默认设备）
            var waveInEvent = new NAudio.Wave.WaveInEvent();
            waveInEvent.WaveFormat = new NAudio.Wave.WaveFormat(22050, 1);
            waveInEvent.DeviceNumber = 0;

            // 创建一个WaveFileWriter对象waveFileWriter，用于将捕获的音频数据写入内存流ms
            // 设置它的波形格式与waveInEvent相同
            // 注意:在这里需要用NAudio.Utils.IgnoreDisposeStream来包装MemoryStream。因为避免在释放waveFileWriter时，同时释放内存流ms，从而导致无法播放录制的音频,如果这里用的不是MemoryStream,是普通的文件流(FileStream),千万不要用"NAudio.Utils.IgnoreDisposeStream"来包装，这样的话会导致无法保存录音文件到磁盘上。
            var waveFileWriter = new NAudio.Wave.WaveFileWriter(new NAudio.Utils.IgnoreDisposeStream(ms), waveInEvent.WaveFormat);

            // 为waveInEvent添加DataAvailable事件处理程序
            // 当有音频数据可用时，调用waveFileWriter的Write方法，将数据写入内存流ms
            waveInEvent.DataAvailable += (_s, _e) =>
            {
                waveFileWriter.Write(_e.Buffer, 0, _e.BytesRecorded);
            };

            // 为waveInEvent添加RecordingStopped事件处理程序
            // 当录制停止时，释放waveFileWriter和waveInEvent对象，并将内存流ms的位置重置为0
            waveInEvent.RecordingStopped += (_s, _e) =>
            {
                waveFileWriter.Dispose();
                waveInEvent.Dispose();
                ms.Position = 0;

                // 创建一个WaveOutEvent对象waveOutEvent，用于播放内存流ms中的音频数据
                var waveOutEvent = new NAudio.Wave.WaveOutEvent();

                // 创建一个WaveFileReader对象waveReader，用于从内存流ms中读取音频数据
                var waveReader = new NAudio.Wave.WaveFileReader(ms);

                // 调用waveOutEvent的Init方法，将waveReader作为输入源
                waveOutEvent.Init(waveReader);

                // 调用waveOutEvent的Play方法，开始播放音频
                waveOutEvent.Play();
            };

            // 调用waveInEvent的StartRecording方法，开始录制音频
            waveInEvent.StartRecording();

            // 在控制台输出提示信息，并等待用户输入
            Console.WriteLine("正在录音，按回车键停止。");
            Console.ReadLine();

            // 调用waveInEvent的StopRecording方法，停止录制并触发RecordingStopped事件
            waveInEvent.StopRecording();

            // 在控制台输出提示信息，并等待用户输入
            //Console.WriteLine("按回车键退出");
            //Console.ReadLine();
        }


        static NAudioRecordHelper helper = new NAudioRecordHelper(NAudioRecordHelper.RecordType.microphone, @"D:\Code\NAudioDemo.wav");

        static void Method11()
        {
            helper.StartRecordAudio();
        }

        static void Method12()
        {
            helper.StopRecordAudio();
        }

        //语音听写demo
        static void Method3()
        {
            XyfunHelper2 helper = new XyfunHelper2();
            helper.Test();
        }

        //实时语音转写demo
        static void Method4()
        {
            XyfunHelper3 helper = new XyfunHelper3();
            helper.Test();
        }
    }
}