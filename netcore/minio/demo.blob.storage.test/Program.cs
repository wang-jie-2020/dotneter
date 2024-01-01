using Demo;
using Demo.Blob.Storage;
using Demo.Blob.Storage.Minio;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
/*
/*	 									BlobStorage->Minio/Aliyun/FastDFS...
/*	
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

builder.Services.AddBlobStorage(options =>
{
    options.Containers.ConfigureAll((containerName, containerConfiguration) =>
    {
        containerConfiguration.UseMinio(minio =>
        {
            minio.EndPoint = "127.0.0.1:9000";
            minio.AccessKey = "test";
            minio.SecretKey = "12345678";
            minio.CreateBucketIfNotExists = true;
        });

        //options.Containers.Configure<TestContainer>(options =>
        //{
        //    options.UseAli(ali =>
        //    {
        //        ali.EndPoint = "";
        //    });
        //})l
    });
}).AddMinioStorage();

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
