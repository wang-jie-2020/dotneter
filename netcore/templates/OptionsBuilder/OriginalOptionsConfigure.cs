using Microsoft.Extensions.Options;

namespace Demo
{
    public class OriginalOptionsConfigure : IConfigureOptions<OriginalOptions>
    {
        public void Configure(OriginalOptions options)
        {
            options.Name = "IConfigureOptions";
            options.Extensions.Add("IConfigureOptions-Emit");
        }
    }
}
