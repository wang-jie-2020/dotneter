namespace Utils.Minio
{
    public interface IMinioNameNormalizer
    {
        string NormalizeBucketName(string name);

        string NormalizeObjectName(string name);
    }
}
