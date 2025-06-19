﻿using System;
using System.Collections.Generic;

namespace JsonLocalizationExtensions.Caching;

public interface IResourceNamesCache
{
    IList<string> GetOrAdd(string name, Func<string, IList<string>> valueFactory);
}
