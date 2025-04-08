using Demo.Blob.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [ApiController]
    [Route("minio")]
    public class MinioController : ControllerBase
    {
        private readonly IBlobContainer<TestBlobContainer> _testBlobContainer;

        public MinioController(IBlobContainer<TestBlobContainer> testBlobContainer)
        {
            _testBlobContainer = testBlobContainer;
        }

        [HttpPost]
        public async Task TestBlob(IFormFile form)
        {
            await _testBlobContainer.SaveAsync(form.FileName, form.OpenReadStream(), true);

            bool exists = await _testBlobContainer.ExistsAsync(form.FileName);
            var fileOut = await _testBlobContainer.GetAsync(form.FileName);
        }
    }
}
