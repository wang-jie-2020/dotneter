﻿using System.Threading;
using JetBrains.Annotations;

namespace Demo.Blob.Storage.ViewModels
{
    public class BlobProviderDeleteArgs : BlobProviderArgs
    {
        public BlobProviderDeleteArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            CancellationToken cancellationToken = default)
            : base(
                containerName,
                configuration,
                blobName,
                cancellationToken)
        {
        }
    }
}

