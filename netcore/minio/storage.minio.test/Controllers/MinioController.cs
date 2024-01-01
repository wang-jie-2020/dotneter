using Microsoft.AspNetCore.Mvc;
using Minio;
using MinioStorage;
using Newtonsoft.Json;

namespace Demo.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("minio")]
    public class MinioController : ControllerBase
    {
        private readonly IMinioContainer<TestContainer> _testContainer;
        private readonly MinioClient _minioClient;

        public MinioController(IMinioContainer<TestContainer> testContainer)
        {
            _testContainer = testContainer;
            _minioClient = _testContainer.Client;
        }

        [HttpPost]
        [Route("container-post")]
        public async Task Post(IFormFile file, string? prefix = "")
        {
            if (string.IsNullOrEmpty(prefix))
            {
                await _testContainer.SaveAsync(file.FileName, file.OpenReadStream(), true);
                return;
            }

            if (!prefix.StartsWith('/'))
            {
                prefix = "/" + prefix;
            }

            if (!prefix.EndsWith('/'))
            {
                prefix = prefix + "/";
            }

            await _testContainer.SaveAsync(prefix + file.FileName, file.OpenReadStream(), true);
        }

        [HttpGet]
        [Route("container-list")]
        public async Task<object> List()
        {
            return await _testContainer.ListAsync();
        }

        [HttpGet]
        [Route("container-get")]
        public async Task<object> Get(string name)
        {
            return await _testContainer.GetAsync(name);
        }

        [HttpDelete]
        [Route("container-delete")]
        public async Task<object> Delete(string name)
        {
            return await _testContainer.DeleteAsync(name);
        }

        [HttpGet]
        [Route("container-pre-get")]
        public async Task<object> PreGetPost(string name)
        {
            var url = await _testContainer.PresignedGetAsync(name, 60 * 60);
            return url;
        }

        [HttpPost]
        [Route("container-pre-post")]
        public async Task PreGetPost(IFormFile file)
        {
            var url = await _testContainer.PresignedSaveAsync(file.FileName, 60 * 60);

            HttpClient httpClient = new HttpClient();
            await httpClient.PutAsync(url, new StreamContent(file.OpenReadStream()));
        }
    }
}
