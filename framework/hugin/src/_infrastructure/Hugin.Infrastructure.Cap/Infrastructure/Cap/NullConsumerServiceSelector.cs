using System.Collections.Generic;
using DotNetCore.CAP.Internal;

namespace Hugin.Infrastructure.Cap
{
    public class NullConsumerServiceSelector : IConsumerServiceSelector
    {
        public IReadOnlyList<ConsumerExecutorDescriptor> SelectCandidates()
        {
            return new List<ConsumerExecutorDescriptor>();
        }

        public ConsumerExecutorDescriptor SelectBestCandidate(string key, IReadOnlyList<ConsumerExecutorDescriptor> candidates)
        {
            return new ConsumerExecutorDescriptor();
        }
    }
}
