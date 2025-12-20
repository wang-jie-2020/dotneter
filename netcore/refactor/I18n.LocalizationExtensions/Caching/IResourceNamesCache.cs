using System;
using System.Collections.Generic;

namespace I18n.LocalizationExtensions.Caching;

public interface IResourceNamesCache
{
    IList<string> GetOrAdd(string name, Func<string, IList<string>> valueFactory);
}
