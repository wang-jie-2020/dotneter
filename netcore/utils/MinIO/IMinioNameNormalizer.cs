namespace MinioStorage
{
    public interface IMinioNameNormalizer
    {
        string NormalizeBucketName(string name);

        string NormalizeObjectName(string name);
    }
}
