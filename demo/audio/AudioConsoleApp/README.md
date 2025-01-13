











讯飞-实时语音转写：https://www.xfyun.cn/doc/asr/rtasr/API.html

## 资料查询

Windows的音频应用程序大部分通过微软提供的上层api完成音频处理和数据流抓取。基于api封装的媒体库或应用包括

Media Foundation，MME(waveIn/waveOut)，Directsound，Wasapi（Windows audio session API）

推荐的是直接通过C++调用api，代表如FFmpeg

Net方向的包：

- NAudio --- 集成了MME、WASAPI
- Directx SDK --- 还未查询
- Mcapture SharpCapture --- 商业工具，还未查询

## 转写效果记录

Net方向遇到了一样的问题，转写效果不稳定，短语、有停顿效果好，但正常语速语调的情况下效果不好，还容易被环境音干扰



效果体现：

- 讯飞的产品效果最好。
  - 讯飞听见同传：https://tongchuan.iflyrec.com/?channel=TJTC002









一、官方DEMO(js)



官方js demo

​	效果相对好

NAudio + 讯飞

​	麦克风输入、扬声器输入

​	效果时好时坏，坏的体现主要是一旦出现识别不准确，接下里的一段时间里就是胡乱翻译

​	短语效果好,有停顿效果好







