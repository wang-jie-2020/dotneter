﻿namespace Yi.AspNetCore.Threading;

public interface IAmbientDataContext
{
    void SetData(string key, object? value);

    object? GetData(string key);
}
