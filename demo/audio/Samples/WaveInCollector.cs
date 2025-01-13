// /************************************************************************
// * @Project:  	AC::WaveInCollector
// * @Decription:  音频采集工具
// * @Verision:  	v1.2.0.0
// * @Author:  	Xin Nie
// * @Create:  	2023/10/8 09:27:00
// * @LastUpdate:  2023/10/30 10:30:00
// ************************************************************************
// * Copyright @ 2025. All rights reserved.
// ************************************************************************/
// namespace Samples
// {
//     /// <summary>
//     /// 声音采集对象
//     /// </summary>
//     /// <summary>
//     /// 声音采集对象
//     ///这是一个功能完整声音采集对象，所有接口通过了测试。
//     ///采样了纯异步实现，接口变得极为简单。只有一个接口：创建异步流(CreateStream)
//     ///需要循环await调用CreateStream获取音频数据（参考dart的stream），如果获取数据频率低于采集频率(比如循环内有耗时操作)，内部会缓存数据，长度为10，超过则丢弃。
//     ///私有化了原来的对象的new、start、stop等方法。只提供两个方法：创建异步流CreateStream。以及获取可用设备AvailableDevices。
//     ///这样的做好处是极度简化了调用，避免了手动资源释，循环退出自动就释放了资源。
//     /// </summary>
//     public class WaveInCollector : IAsyncDisposable
//     {
//         /// <summary>
//         ///创建音频采集数据流
//         ///这是一个异步流(async stream)，是C#8.0的特性，与dart的stream有点类似。
//         ///参考 https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/generate-consume-asynchronous-stream
//         /// 用法：
//         /// await foreach(var i in WaveInCollector.CreateStream())
//         /// {
//         ///  //i即是采集的数据，需要停止采集则直接break。
//         /// }
//         /// 采集过程中遇到错误会抛出异常。
//         /// </summary>
//         /// <param name="deviceId">设备Id，0为默认设备，可以通过WaveInCollector.AvailableDevices获取可用设备，同一个设备不能同时打开多个</param>
//         /// <param name="sf">声音格式，null时默认为：2,44100,16</param>
//         /// <param name="bufferLength">缓冲区长度，即每次获取数据的长度。其值大小会影响采集频率，值越小采集频率越高</param>
//         /// <param name="startedTcs">采集开始的Completion对象，通过这个对象可以获得采集开始事件</param>
//         /// <returns>异步流</returns>
//         public static async IAsyncEnumerable<byte[]> CreateStream(uint deviceId = 0, SampleFormat? sf = null, uint bufferLength = 8192, TaskCompletionSource? startedTcs = null);
//         /// <summary>
//         /// 枚举可用的声音采集设备
//         /// 由于api限制Name长度最大为32
//         /// </summary>
//         public static IEnumerable<AudioDevice> AvailableDevices { get; };
//         /// <summary>
//         /// 异步dispose继承与IAsyncDisposable
//         /// </summary>
//         /// <returns></returns>
//         public async ValueTask DisposeAsync();
//     }
//     /// <summary>
//     /// 声音格式
//     /// </summary>
//     public class SampleFormat
//     {
//         /// <summary>
//         /// 声道数
//         /// </summary>
//         public ushort Channels { set; get; }
//         /// <summary>
//         /// 采样率
//         /// </summary>
//         public uint SampleRate { set; get; }
//         /// <summary>
//         /// 位深
//         /// </summary>
//         public ushort BitsPerSample { set; get; }
//     }
//     /// <summary>
//     /// 音频设备
//     /// </summary>
//     public class AudioDevice
//     {
//         /// <summary>
//         /// 设备Id
//         /// </summary>
//         public uint Id { set; get; }
//         /// <summary>
//         /// 设备名称
//         /// </summary>
//         public string Name { set; get; } = "";
//         /// <summary>
//         /// 声道数
//         /// </summary>
//         public int Channels { set; get; }
//         /// <summary>
//         /// 支持的格式
//         /// </summary>
//         public IEnumerable<SampleFormat> SupportedFormats { set; get; }
//     }
// }
