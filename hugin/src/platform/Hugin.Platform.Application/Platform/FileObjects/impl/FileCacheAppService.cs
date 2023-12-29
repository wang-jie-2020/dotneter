using System;
using System.Threading.Tasks;
using Hugin.Cache.CsRedis;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp;

namespace Hugin.Platform.FileObjects.impl
{
    [RemoteService(isEnabled: false)]
    public class FileCacheAppService : BaseAppService, IFileCacheAppService
    {
        private readonly ICsRedisCache<FileCto> _csRedis;

        public FileCacheAppService(ICsRedisCache<FileCto> csRedis)
        {
            _csRedis = csRedis;
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="file"></param>
        public async Task<string> SetFile(FileCto file)
        {
            var token = Guid.NewGuid().ToString("N");
            await _csRedis.SetAsync(token, file, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });

            return token;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<FileCto> GetFile(string token)
        {
            return await _csRedis.GetAsync(token);
        }
    }
}
