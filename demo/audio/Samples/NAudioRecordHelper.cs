using System;
using NAudio.Wave;

namespace Samples
{
    public class NAudioRecordHelper
    {
        public enum RecordType
        {
            loudspeaker = 0, // 扬声器
            microphone = 1 //麦克风
        }

        //录制的类型
        RecordType t = RecordType.microphone;

        //录制麦克风的声音
        WaveInEvent waveIn = null; //new WaveInEvent();

        //录制扬声器的声音
        WasapiLoopbackCapture capture = null; //new WasapiLoopbackCapture();

        //生成音频文件的对象
        WaveFileWriter writer = null;

        string audioFile = "";

        public NAudioRecordHelper(RecordType x, string filePath)
        {
            t = x;
            audioFile = filePath;
        }

        /// <summary>
        /// 开始录制
        /// </summary>
        public void StartRecordAudio()
        {
            try
            {
                if (audioFile == "")
                {
                    //System.Windows.Forms.MessageBox.Show("请设置录制文件的路径！");
                    // return;
                    throw new Exception("请设置录制文件的路径!");
                }

                if (t == RecordType.microphone)
                {
                    waveIn = new WaveInEvent();
                    writer = new WaveFileWriter(audioFile, waveIn.WaveFormat);
                    //开始录音，写数据
                    waveIn.DataAvailable += (s, a) =>
                    {
                        Console.WriteLine(a.Buffer.Length);
                        writer.Write(a.Buffer, 0, a.BytesRecorded);
                    };

                    //结束录音
                    waveIn.RecordingStopped += (s, a) =>
                    {
                        writer.Dispose();
                        writer = null;
                        waveIn.Dispose();
                    };


                    waveIn.StartRecording();
                }
                else
                {
                    capture = new WasapiLoopbackCapture();
                    writer = new WaveFileWriter(audioFile, capture.WaveFormat);

                    capture.DataAvailable += (s, a) => { writer.Write(a.Buffer, 0, a.BytesRecorded); };
                    //结束录音
                    capture.RecordingStopped += (s, a) =>
                    {
                        writer.Dispose();
                        writer = null;
                        capture.Dispose();
                    };


                    capture.StartRecording();
                }
            }
            catch (Exception ex)
            {
            }
        }

        //结束录制
        public void StopRecordAudio()
        {
            if (t == RecordType.microphone)
                waveIn.StopRecording();
            else
                capture.StopRecording();
        }
    }
}


//此处可能是录制声卡声音 todo
// private WaveFileWriter m_waveFileWriter;
// private WasapiLoopbackCapture m_capture = new WasapiLoopbackCapture();
// private void btnStartRecord_Click(object sender, EventArgs e)
// {
//     m_waveFileWriter = new WaveFileWriter(Path.Combine(txtOutputDir.Text, String.Format("{0}.wav", txtOutputFileName.Text)), m_capture.WaveFormat);
//     m_capture.DataAvailable += (s, a) =>
//     {
//         m_waveFileWriter.Write(a.Buffer, 0, a.BytesRecorded);
//     };
//     m_capture.StartRecording();
// }
//
// private void btnEndRecord_Click(object sender, EventArgs e)
// {
//     m_capture.StopRecording();
//             
//     m_waveFileWriter.Dispose();
//     m_capture.Dispose();            
// }

