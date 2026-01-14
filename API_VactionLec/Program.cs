using DotnetCoreServer.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// Add services to the container.
// API패턴의 서버이기 때문에 뷰는 없습니다.
// Controllers 폴더 안에 있는 컨트롤러들을 서비스로 추가합니다.
builder.Services
    .AddControllers()   // MVC 패턴에서 C 역할을 하는 컨트롤러 서비스 추가
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });
// MVC 패턴에서 M 역할을 하는 모델 서비스 추가
// Models/Dao 폴더 안에 있는 DAO 서비스들을 추가합니다.
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

app.MapControllers();   // 컨트롤러들을 매핑합니다.
                        // api 주소 패턴과 컨트롤러의 액션 메서드를 매핑합니다.   
app.Run();
