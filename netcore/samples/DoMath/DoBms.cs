using System.Text;

namespace DoMath;

public class DoBms
{
    private const string tem_bin = @"C:\Users\jie.wang21\Desktop\sd卡解析说明\Bat Temp_2025-04-08_16-14-00.bin";
    private const string vol_bin = @"C:\Users\jie.wang21\Desktop\sd卡解析说明\Bat Vol_2025-05-20_07-19-12.bin";
    private const string sum_bin = @"C:\Users\jie.wang21\Desktop\sd卡解析说明\Summary In_2025-05-20_07-17-39.bin";

    void Method1()
    {
        using (var stream = new FileStream(tem_bin, FileMode.Open, FileAccess.Read))
        {
            if (stream.Length < 62)
            {
                throw new Exception("invalid file");
            }

            using (var reader = new BinaryReader(stream))
            {
                // No1 标记头
                var binVersion = reader.ReadBytes(4);
                var binTag = BitConverter.ToString(reader.ReadBytes(4));
                var binType = 0;
                var binFixedSize = 0;
                var binExtraSize = 0;
                var binExtraNum = 0;
                var binContextSize = 0;
                if (BitConverter.IsLittleEndian) //大小端问题
                {
                    binType = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray());
                    binFixedSize = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray());
                    binExtraSize = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray());
                    binExtraNum = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray());
                    binContextSize = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray());
                }
                else
                {
                    binType = BitConverter.ToInt32(reader.ReadBytes(4));
                    binFixedSize = BitConverter.ToInt32(reader.ReadBytes(4));
                    binExtraSize = BitConverter.ToInt32(reader.ReadBytes(4));
                    binExtraNum = BitConverter.ToInt32(reader.ReadBytes(4));
                    binContextSize = BitConverter.ToInt32(reader.ReadBytes(4));
                }
                var binCrc32 = BitConverter.ToString(reader.ReadBytes(4));
                
                reader.ReadBytes(30);

                // No2 规则头
                var buffer = reader.ReadBytes(32 * 1024 - (int)reader.BaseStream.Position); // 32k --- {内容32}{预留30}{规则}
                var binRules = Encoding.UTF8.GetString(buffer);
                
                var binContentRules = Encoding.UTF8.GetString(buffer.Skip(0).Take(binContextSize).ToArray()).TrimEnd('\0');
                var binContentColumns = binContentRules.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                // No3 内容头
                var segmentSize = binFixedSize + binExtraSize * binExtraNum;
                buffer = reader.ReadBytes(segmentSize);
                var segments = BitConverter.ToString(buffer);
            }
        }
    }
}