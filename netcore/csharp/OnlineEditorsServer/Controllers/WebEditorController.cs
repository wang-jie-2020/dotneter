using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineEditorsServer.DocumentServer;

namespace OnlineEditorsServer.Controllers;

[Route("api/web-editor")]
[AllowAnonymous]
public class WebEditorController : ControllerBase
{
    private readonly ILogger<WebEditorController> _logger;
    private readonly ICallbackManager _callbackManager;

    public WebEditorController(
        ILogger<WebEditorController> logger,
        ICallbackManager callbackManager)
    {
        _logger = logger;
        _callbackManager = callbackManager;
    }

    [HttpGet("fake-track")]
    [HttpPost("fake-track")]
    public async Task FakeTrack()
    {
        var body = string.Empty;

        try
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
                _logger.LogInformation(body);
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
        }

        await Response.WriteAsync("{\"error\":0}");
    }

    /// <summary>
    ///  ONLY-OFFICE 回调
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="host">ONLYOFFICE-SERVER</param>
    /// <exception cref="BadHttpRequestException"></exception>
    [HttpGet("track")]
    [HttpPost("track")]
    public async Task Track([FromQuery] string name, [FromQuery] string host)
    {
        var body = string.Empty;
        Callback? callback;
        try
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
                if (string.IsNullOrEmpty(body))
                {
                    await Response.WriteAsync("{\"error\":1,\"message\":\"Request stream is empty\"}");
                    return;
                }
            }
        }
        catch (Exception e)
        {
            throw new BadHttpRequestException(e.Message);
        }

        _logger.LogInformation(body);

        try
        {
            callback = JsonConvert.DeserializeObject<Callback>(body);
        }
        catch (Exception e)
        {
            _logger.LogError($"JSON parsing error: {e.Message}, {e.StackTrace}");
            throw;
        }

        var saved = 0;
        switch (callback?.Status)
        {
            case TrackerStatus.Editing:
                try
                {
                    var action = callback.Actions?.FirstOrDefault();
                    if (action != null && action.Type == ActionType.DisConnected)
                    {
                        await _callbackManager.ProcessCommandAsync("forcesave", callback.Key, host);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }

                break;
            case TrackerStatus.MustSave:
            case TrackerStatus.Corrupted:
                try
                {
                    // saving a document
                    saved = await _callbackManager.ProcessSaveAsync(name, callback);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    saved = 1;
                }

                await Response.WriteAsync("{\"error\":" + saved + "}");
                return;
            case TrackerStatus.MustForceSave:
            case TrackerStatus.CorruptedForceSave:
                try
                {
                    // force saving a document
                    saved = await _callbackManager.ProcessForceSaveAsync(name, callback);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    saved = 1;
                }

                await Response.WriteAsync("{\"error\":" + saved + "}");
                return;
        }

        await Response.WriteAsync("{\"error\":0}");
    }
}