﻿using System.Collections.Generic;
using System.Globalization;

namespace JsonLocalizationExtensions.Internal;

public interface IResourceStringProvider
{
    IList<string> GetAllResourceStrings(CultureInfo culture, bool throwOnMissing);
}
