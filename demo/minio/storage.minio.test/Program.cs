using Demo;
using MinioStorage;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<ITestContainerNormalizeNamingService, TestContainerNormalizeNamingService>();

builder.Services.UseMinioStorage(options =>
{
    options.Containers.Configure<TestContainer>(configuration =>
    {
        //configuration.NormalizeNamingServiceTypes.Add(typeof(ITestContainerNormalizeNamingService));
    });

    options.Containers.ConfigureAll((containerName, containerConfiguration) =>
    {
        containerConfiguration.UseMinio(minio =>
        {
            minio.EndPoint = "127.0.0.1:9000";
            minio.AccessKey = "test";
            minio.SecretKey = "12345678";
            //minio.BucketName = "assigned";    //强制指定存储桶
            minio.CreateBucketIfNotExists = true;
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
