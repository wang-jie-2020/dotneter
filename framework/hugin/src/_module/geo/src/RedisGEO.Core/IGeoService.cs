using System.Collections.Generic;
using System.Threading.Tasks;
using RedisGEO.Core.Models;

namespace RedisGEO.Core
{
    public interface IGeoService
    {
        Task<bool> ExistsAsync(string key);

        Task<long> CountAsync(string key);

        Task<bool> AddAsync(string key, string name, decimal longitude, decimal latitude);

        Task<long> DeleteAsync(string key, string name);

        Task<long> BatchDeleteAsync(string key);

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）
        /// </summary>
        /// <param name="pointInput"></param>
        /// <returns></returns>
        Task<IEnumerable<RadiusWithDistResult>> RadiusWithDistAsync(RadiusWithDistInput pointInput);
    }
}
