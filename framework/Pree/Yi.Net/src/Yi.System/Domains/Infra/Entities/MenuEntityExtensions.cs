using Yi.AspNetCore.Helpers;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Domains.Infra.Entities;

/// <summary>
///     实体扩展
/// </summary>
public static class MenuEntityExtensions
{
    /// <summary>
    ///     构建vue3路由
    /// </summary>
    /// <param name="menus"></param>
    /// <returns></returns>
    public static List<Vue3RouterDto> Vue3RouterBuild(this List<MenuEntity> menus)
    {
        menus = menus.Where(m => m.MenuType != MenuTypeEnum.Component).ToList();
        List<Vue3RouterDto> routers = new();
        foreach (var m in menus)
        {
            var r = new Vue3RouterDto();
            r.OrderNum = m.OrderNum;
            var routerName = m.Router?.Split("/").LastOrDefault();
            r.Id = m.Id;
            r.ParentId = m.ParentId;

            //开头大写
            r.Name = routerName?.First().ToString().ToUpper() + routerName?.Substring(1);
            r.Path = m.Router!;
            r.Hidden = !m.IsShow;


            if (m.MenuType == MenuTypeEnum.Catalogue)
            {
                r.Redirect = "noRedirect";
                r.AlwaysShow = true;

                //判断是否为最顶层的路由
                if (Guid.Empty == m.ParentId)
                    r.Component = "Layout";
                else
                    r.Component = "ParentView";
            }

            if (m.MenuType == MenuTypeEnum.Menu)
            {
                r.Redirect = "noRedirect";
                r.AlwaysShow = true;
                r.Component = m.Component!;
                r.AlwaysShow = false;
            }

            r.Meta = new Meta
            {
                Title = m.MenuName!,
                Icon = m.MenuIcon!,
                NoCache = !m.IsCache
            };
            if (m.IsLink)
            {
                r.Meta.link = m.Router!;
                r.AlwaysShow = false;
            }

            routers.Add(r);
        }

        return TreeHelper.SetTree(routers);
    }
}