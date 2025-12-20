using System.Collections.Generic;
using System.Globalization;

namespace I18n.LocalizationExtensions.Internal;

public interface IResourceStringProvider
{
    IList<string> GetAllResourceStrings(CultureInfo culture, bool throwOnMissing);
}
