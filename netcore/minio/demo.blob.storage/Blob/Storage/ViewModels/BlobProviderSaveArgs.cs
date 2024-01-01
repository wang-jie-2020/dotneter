using System.IO;
using System.Threading;
using Demo.Blob.Storage.Utils;
using JetBrains.Annotations;

namespace Demo.Blob.Storage.ViewModels
{
    public class BlobProviderSaveArgs : BlobProviderArgs
    {
        [NotNull]
        public Stream BlobStream { get; }

        public bool OverrideExisting { get; }

        public BlobProviderSaveArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            [NotNull] Stream blobStream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
            : base(
                containerName,
                configuration,
                blobName,
                cancellationToken)
        {
            BlobStream = Check.NotNull(blobStream, nameof(blobStream));
            OverrideExisting = overrideExisting;
        }
    }
}

