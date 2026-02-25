using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    internal class Zips
    {
        void Method1()
        {
            var zip1 = @"C:\Users\jie.wang21\Desktop\zips\测试数据.zip";
            var zip2 = @"C:\Users\jie.wang21\Desktop\zips\测试数据 - 副本.zip";
            var zip3 = @"C:\Users\jie.wang21\Desktop\zips\新建 ZIP 压缩文件.zip";

            var extract = @"C:\Users\jie.wang21\Desktop\zips";

            using (var stream = new FileStream(zip1, FileMode.Open, FileAccess.Read))
            {
                ZipFile.ExtractToDirectory(stream, extract, Encoding.UTF8, true);
            }

            using (var stream = new FileStream(zip2, FileMode.Open, FileAccess.Read))
            {
                ZipFile.ExtractToDirectory(stream, extract, Encoding.UTF8, true);
            }

            using (var stream = new FileStream(zip3, FileMode.Open, FileAccess.Read))
            {
                ZipFile.ExtractToDirectory(stream, extract, Encoding.UTF8, true);
            }

            ZipFile.ExtractToDirectory(zip1, extract, Encoding.UTF8, true);
            ZipFile.ExtractToDirectory(zip2, extract, Encoding.UTF8, true);
            ZipFile.ExtractToDirectory(zip3, extract, Encoding.UTF8, true);

            //var zip4 = @"C:\Users\jie.wang21\Desktop\zips\新建 文本文档.zip";
            //ZipFile.ExtractToDirectory(zip4, extract, Encoding.UTF8, true);

            var zip5 = @"C:\Users\jie.wang21\Desktop\zips\meeting_2.12双流程周会_source_target_50004009_5612.zip";
            ZipFile.ExtractToDirectory(zip5, extract, Encoding.UTF8, true);
        }

        void Method2()
        {
            var url = @"https://genie.aesc-group.com/minio/aigc-prod/meeting_record/2026-02-12/meeting_2.12%E5%8F%8C%E6%B5%81%E7%A8%8B%E5%91%A8%E4%BC%9A_source_target_50004009_5612.zip";
            var file = @"C:\Users\jie.wang21\Desktop\zips\newf.zip";
            var extract = @"C:\Users\jie.wang21\Desktop\zips\newf.zip";

            var task = Task.Run(async () =>
            {
                await DownloadWithProgressAsync(url, file, new CancellationToken(), new Progress<int>(), 0, 100);
            });

            task.Wait();

            ZipFile.ExtractToDirectory(file, extract, Encoding.UTF8, true);
        }


        private async Task DownloadWithProgressAsync(
         string url,
         string savePath,
         CancellationToken cancellationToken,
         IProgress<int> progress,
         int startPercent,
         int endPercent)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            var contentLength = response.Content.Headers.ContentLength;
            var totalRead = 0L;
            var range = endPercent - startPercent;

            using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);

            var buffer = new byte[81920]; // 80KB buffer
            int bytesRead;

            while ((bytesRead = await contentStream.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                totalRead += bytesRead;

                if (contentLength.HasValue && contentLength.Value > 0)
                {
                    var percentComplete = startPercent + (int)((totalRead * range) / contentLength.Value);
                    progress.Report(Math.Min(percentComplete, endPercent));
                }
            }

            // 确保最后报告完成百分比
            progress.Report(endPercent);
        }
    }
}
