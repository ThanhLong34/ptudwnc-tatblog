using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;

var builder = WebApplication.CreateBuilder(args);
{
    builder
      .ConfigureCors()
      .ConfigureNLog()
      .ConfigureServices()
      .ConfigureSwaggerOpenApi()
      .ConfigureMapster();
}


var app = builder.Build();
{
    // Configure the HTTP request pipeline
    app.SetupRequestPipeline();

    // Configure API endpoints
    app.MapAuthorEndpoints();

    app.Run();
}



