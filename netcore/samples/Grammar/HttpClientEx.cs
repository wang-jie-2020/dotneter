using System.Security.AccessControl;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace Grammar;

public class HttpClientEx
{
    void Method1()
    {
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;

        var httpClient = new HttpClient(httpClientHandler);
        httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

        var resp = httpClient.GetAsync("/users").Result;
        Console.WriteLine(resp);
    }

    void Method2()
    {
        var sc = new ServiceCollection();

        sc.AddHttpClient("default", (sp, c) =>
            {
                var fac = sp.GetRequiredService<IHttpClientFactory>();
                c.BaseAddress = new Uri("https://examples.com/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new CustomerHttpClientHandler())
            .ConfigureAdditionalHttpMessageHandlers((handlers, sp) => handlers.Add(new CustomerDelegatingHandler("configure syntax")))
            .AddHttpMessageHandler(() => new CustomerDelegatingHandler("add syntax"));

        var sp = sc.BuildServiceProvider();
        var factory = sp.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient("default");
        
        var resp = client.GetAsync("/users").Result;
        Console.WriteLine(resp);
    }
}

public class CustomerHttpClientHandler : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(CustomerHttpClientHandler));
        
        request.RequestUri = new Uri(new Uri("https://jsonplaceholder.typicode.com/"), request.RequestUri?.PathAndQuery);
        return base.SendAsync(request, cancellationToken);
    }
}

public class CustomerDelegatingHandler : DelegatingHandler
{
    private readonly string _name;

    public CustomerDelegatingHandler(string name)
    {
        _name = name;
    }
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine(_name);

        return base.SendAsync(request, cancellationToken);
    }
}