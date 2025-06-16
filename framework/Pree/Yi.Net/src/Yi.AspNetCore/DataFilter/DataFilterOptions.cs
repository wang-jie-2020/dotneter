using System;
using System.Collections.Generic;

namespace Yi.AspNetCore.DataFilter;

public class DataFilterOptions
{
    public Dictionary<Type, DataFilterState> DefaultStates { get; }

    public DataFilterOptions()
    {
        DefaultStates = new Dictionary<Type, DataFilterState>();
    }
}
