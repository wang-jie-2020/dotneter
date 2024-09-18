﻿using Volo.Abp.Uow;

namespace Yi.AspNetCore.SqlSugarCore.Uow;

public class SqlSugarDatabaseApi : IDatabaseApi
{
    public SqlSugarDatabaseApi(ISqlSugarDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public ISqlSugarDbContext DbContext { get; }
}