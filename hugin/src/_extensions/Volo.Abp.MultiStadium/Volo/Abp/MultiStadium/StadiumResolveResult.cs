using System.Collections.Generic;

namespace Volo.Abp.MultiStadium
{
    public class StadiumResolveResult
    {
        public string StadiumIdOrName { get; set; }

        public List<string> AppliedResolvers { get; }

        public StadiumResolveResult()
        {
            AppliedResolvers = new List<string>();
        }
    }
}