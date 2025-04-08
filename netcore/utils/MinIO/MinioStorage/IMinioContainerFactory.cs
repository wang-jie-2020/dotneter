namespace MinioStorage
{
    public interface IMinioContainerFactory
    {
        IMinioContainer Create(string name);
    }
}
