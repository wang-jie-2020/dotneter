namespace Utils.Minio
{
    public interface IMinioContainerFactory
    {
        IMinioContainer Create(string name);
    }
}
