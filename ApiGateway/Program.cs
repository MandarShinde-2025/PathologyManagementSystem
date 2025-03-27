using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Adding reverse proxy services
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();

app.MapReverseProxy();

app.Run();