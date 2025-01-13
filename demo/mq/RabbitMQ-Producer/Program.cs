using RabbitMQ_Producer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<TxSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


//while (true)
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var sender = scope.ServiceProvider.GetRequiredService<TxSender>();
//        sender.Send2();
//    }

//    Thread.Sleep(10000);
//}





app.Run();
