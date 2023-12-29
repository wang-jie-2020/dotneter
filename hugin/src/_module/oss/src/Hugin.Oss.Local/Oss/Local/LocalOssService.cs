using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hugin.Oss.Local.@internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hugin.Oss.Local
{
    public class LocalOssService : IOssService
    {
        private readonly LocalOssOptions _options;

        private const string tokenPath = "/api/Token/GetToken";
        private const string normalSavePath = "/api/File/FileUpload";
        private const string chunkSavePath = "/api/File/FileSave";
        private const string chunkMergePath = "/api/File/FileMerge";

        public LocalOssService(IOptions<LocalOssOptions> options)
        {
            _options = options.Value;
        }

        private async Task<string> GetToken()
        {
            var url = _options.Endpoint + tokenPath + $"?appId={_options.AppId}&appKey={_options.AppSecret}";

            var result = await ApiCallerHelper.Execute<string>(url, HttpMethod.Get);
            return result;
        }

        public async Task<OssFileInfo> Save(IFormFile file, string container)
        {
            var url = _options.Endpoint + normalSavePath;

            var boundary = DateTime.Now.Ticks.ToString("X");
            var formData = new MultipartFormDataContent(boundary);

            formData.Add(new StringContent(await GetToken()), "\"Token\"");
            formData.Add(new StringContent(container), "\"Owner\"");

            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream(), encoding: Encoding.UTF8))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            formData.Add(new ByteArrayContent(data), "\"File\"", file.FileName);

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(url, formData);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            var fileData = JsonConvert.DeserializeObject<FileDto>(result);

            return new OssFileInfo
            {
                Container = fileData.File.FrOwnerUser,
                ETag = fileData.Id,
                LastModified = fileData.File.FrCreateTime,
                Name = fileData.File.FrName,
                Length = long.Parse(fileData.File.FrSize),
                Url = $"{_options.Endpoint}{fileData.RelativeUrl}",
                ContentMd5 = fileData.File.FrHash,
                ContentType = fileData.File.FrType
            };
        }

        public async Task ChunkSave(IFormFile file, string container, string guid, int chunk)
        {
            var url = _options.Endpoint + chunkSavePath;

            var boundary = DateTime.Now.Ticks.ToString("X");
            var formData = new MultipartFormDataContent(boundary);

            formData.Add(new StringContent(await GetToken()), "\"Token\"");
            formData.Add(new StringContent(container), "\"Owner\"");
            formData.Add(new StringContent(guid), "\"Guid\"");
            formData.Add(new StringContent(chunk.ToString()), "\"Chunk\"");

            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream(), encoding: Encoding.UTF8))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            formData.Add(new ByteArrayContent(data), "\"File\"", file.FileName);

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(url, formData);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            //return JsonConvert.DeserializeObject<ChunkedFileDto>(result);
        }

        public async Task<OssFileInfo> ChunkMerge(string container, string guid, string fileName)
        {
            var url = _options.Endpoint + chunkMergePath;

            var boundary = DateTime.Now.Ticks.ToString("X");
            var formData = new MultipartFormDataContent(boundary);

            formData.Add(new StringContent(await GetToken()), "\"Token\"");
            formData.Add(new StringContent(container), "\"Owner\"");
            formData.Add(new StringContent(guid), "\"Guid\"");
            formData.Add(new StringContent(fileName), "\"FileName\"");

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(url, formData);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            var fileData = JsonConvert.DeserializeObject<FileDto>(result);

            return new OssFileInfo
            {
                Container = fileData.File.FrOwnerUser,
                ETag = fileData.Id,
                LastModified = fileData.File.FrCreateTime,
                Name = fileData.File.FrName,
                Length = long.Parse(fileData.File.FrSize),
                Url = $"{_options.Endpoint}{fileData.RelativeUrl}",
                ContentMd5 = fileData.File.FrHash,
                ContentType = fileData.File.FrType
            };
        }
    }
}
