namespace Demo.Blob.Storage
{
    public interface IBlobContainerFactory
    {
        IBlobContainer Create(string name);
    }
}
