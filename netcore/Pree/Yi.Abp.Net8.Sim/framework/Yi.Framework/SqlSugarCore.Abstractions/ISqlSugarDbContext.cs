﻿using SqlSugar;

namespace Yi.Framework.SqlSugarCore.Abstractions;

public interface ISqlSugarDbContext
{
    //  IAbpLazyServiceProvider LazyServiceProvider { get; set; }
    ISqlSugarClient SqlSugarClient { get; }
    DbConnOptions Options { get; }

    /// <summary>
    ///     数据库备份
    /// </summary>
    void BackupDataBase();

    void SetSqlSugarClient(ISqlSugarClient sqlSugarClient);
}