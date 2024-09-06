using Volo.Abp.Data;

namespace Yi.Abp.Infra.Rbac.Authorization
{
    public static class DataPermissionExtensions
    {
        /// <summary>
        /// 关闭数据权限
        /// </summary>
        /// <param name="dataFilter"></param>
        /// <returns></returns>
        public static IDisposable DisablePermissionHandler(this IDataFilter dataFilter)
        {
            return dataFilter.Disable<IDataPermission>();
        }
    }
}
