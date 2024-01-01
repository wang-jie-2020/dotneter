namespace Demo.Blob.Storage
{
    public interface IBlobNamingNormalizer
    {
        string NormalizeContainerName(string containerName);

        string NormalizeBlobName(string blobName);
    }
}
