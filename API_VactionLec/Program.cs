using DotnetCoreServer.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// Add services to the container.

builder.Services
    .AddControllers()   // MVC 패턴에서 C 역할을 하는 컨트롤러 서비스 추가
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

builder.Services.AddScoped<IUserDao, UserDao>();        // 의존성 규칙 설정
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
