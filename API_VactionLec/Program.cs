using DotnetCoreServer.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

builder.Services.AddScoped<IUserDao, UserDao>();
builder.Services.AddScoped<IStageResultDao, StageResultDao>();
builder.Services.AddScoped<IRankDao, RankDao>();
builder.Services.AddScoped<IUpgradeDao, UpgradeDao>();
builder.Services.AddScoped<IDB, DB>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
