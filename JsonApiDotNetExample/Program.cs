using JsonApiDotNetCore.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddJsonApi(
    options: options =>
    {
        options.IncludeTotalResourceCount = true;
        options.AllowUnknownQueryStringParameters = true;
    },
    discovery: discovery => discovery.AddCurrentAssembly()
);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseJsonApi();
app.MapControllers();

app.Run();