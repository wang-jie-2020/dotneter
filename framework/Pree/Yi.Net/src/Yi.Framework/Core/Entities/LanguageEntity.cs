using SqlSugar;
using Yi.Framework.SqlSugarCore;

namespace Yi.Framework.Core.Entities;

[SugarTable("Sys_Language")]
public class LanguageEntity: BizEntity<long>
{
    public string Name { get; set; }
    
    public string Value { get; set; }
    
    public string Culture { get; set; }
}