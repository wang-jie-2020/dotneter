using System.Text;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;

namespace SSE_Server.Controllers;

[ApiController]
[Route("/api/chat")]
public class SSEEventController : ControllerBase
{
    async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
    {
        for (int i = 0; i < count; i++)
        {
            await Task.Delay(1000); // 模拟异步操作，例如从网络或文件系统读取数据
            yield return i; // 每次迭代返回一个值
        }
    }

    [HttpGet("numbers")]
    public async Task AsyncEnumerableDemo()
    {
        await foreach (var number in GenerateNumbersAsync(10))
        {
            Console.WriteLine($"返回第{number + 1}个值");
        }
    }

    //似乎不支持如此? 网页端是正常的
    [HttpPost]
    public async Task Input([FromForm] string message, CancellationToken cancellationToken)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.StatusCode = 200;
        
        await foreach (var w in GenerateCharsAsync(message).WithCancellation(cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            await Response.Body.WriteAsync(Encoding.UTF8.GetBytes($"{w}"), cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }

    async IAsyncEnumerable<char> GenerateCharsAsync(string message)
    {
        foreach (var s in message)
        {
            await Task.Delay(1000);
            yield return s;
        }
    }

    [HttpGet("test")]
    public async Task Test()
    {
        var baseUrl = "http://localhost:5000";

        using (var httpClient = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/chat");
            request.Headers.Add("Accept", "text/event-stream");

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("message", "Hello World!")
            });
            request.Content = formContent;

            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                // var content = await response.Content.ReadAsStringAsync();
                // Console.WriteLine(content);

                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(responseStream);
                while (!streamReader.EndOfStream)
                {
                    var line = await streamReader.ReadLineAsync();
                    Console.WriteLine(line);
                }
            }
        }
    }
}