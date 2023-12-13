using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SCP.Context;
using SCP.Security;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, //Место хранения токена в хедере
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>(); //Добавление блока сваггера для блокировки, короче тут всё понятно
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
//Инициализация
var config = builder.Configuration;

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<LoginDBContext>(); //добавление с помощью Entity Framework Identity юзера, он берёт инфу для логина из БД
//Добавление строки подключения из AppSettings.json
builder.Services.AddDbContext<LoginDBContext>(
    options => options.UseNpgsql(config.GetConnectionString("LoginConnection")));



//Добавление строки подключения из AppSettings.json
builder.Services.AddDbContext<MyDbContext>(
    options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));


//Добавление в конфигуры наш JWTSettings, короче токен
/* builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

var secretKey = builder.Configuration.GetSection("JWTSettings:SecretKey").Value;
var issuer = builder.Configuration.GetSection("JWTSettings:Issuer").Value;
var audience = builder.Configuration.GetSection("JWTSettings:Audience").Value; //Тоже самое, что и выше

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); //Генерация токена

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //добавление аутентификации, берём медведей JWT
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            IssuerSigningKey = signingKey,
            ValidateIssuerSigningKey = true //Настройка параметров валидации токенов
        };
    }); */

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// app.MapGroup("/identity").MapIdentityApi<IdentityUser>();
app.Run();

