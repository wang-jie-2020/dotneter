# minio.container

`Utils.Minio` 是对dotnet-minio-client的一个简单封装,主要考虑是简化minio-client的api调用

## 注册和配置

```csharp
builder.Services.UseMinioStorage(options =>
{
    //指定配置
    options.Containers.Configure<TestContainer>(configuration =>
    {
      	//此处可以指定存储配置,优先级>默认配置
    });
	
    //默认配置
    options.Containers.ConfigureAll((containerName, containerConfiguration) =>
    {
        containerConfiguration.UseMinio(minio =>
        {
            minio.EndPoint = "ip:port";
            minio.AccessKey = "test";
            minio.SecretKey = "12345678";
            //minio.BucketName = "assigned";    //推荐以标注形式指定存储桶,注意此处配置>标注
            //minio.CreateBucketIfNotExists = true; //是否创建存储桶
        });
    });
});
```

```csharp
[ContainerName("test")]
public class TestContainer
{
    
}
```

## 注入

泛型注册`services.AddTransient(typeof(IMinioContainer<>), typeof(MinioContainer<>));`

```csharp
private readonly IMinioContainer<TestContainer> _testContainer;

public MinioController(IMinioContainer<TestContainer> testContainer)
{
    _testContainer = testContainer;
}
```

## API参考

```csharp
public interface IMinioContainer
{
    MinioClient Client { get; }

    Task<List<string>> ListAsync(string prefix = "");

    Task SaveAsync(string name, Stream stream, bool overrideExisting = false);

    Task<bool> ExistsAsync(string name);

    Task<Stream> GetAsync(string name);

    Task<bool> DeleteAsync(string name);

    //url for http-get without credentials
    //https://docs.min.io/docs/dotnet-client-api-reference.html#presignedGetObject
    Task<string> PresignedGetAsync(string name, int expiry);

    //url for http-put without credentials
    //https://docs.min.io/docs/dotnet-client-api-reference.html#presignedPutObject
    Task<string> PresignedSaveAsync(string name, int expiry, bool overrideExisting = false);
}
```

- 存储

  ```csharp
   await _testContainer.SaveAsync("test-save", fileStream);
   await _testContainer.SaveAsync("/1234567/test-save", fileStream);
  ```

- 读取

  ```csharp
  var files = await _testContainer.ListAsync();
  var fileOut = await _testContainer.GetAsync("test-save");
  ```

- pre-get、pre-post

  ```csharp
  var url = await _testContainer.PresignedGetAsync("test-save", 60);
  
  HttpClient httpClient = new HttpClient();
  var url = await _testContainer.PresignedSaveAsync("pre-test-save", 60 * 60);
  await httpClient.PutAsync(url, new StreamContent(fileStream));
  ```

  