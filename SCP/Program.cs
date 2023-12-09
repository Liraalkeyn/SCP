using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SCP.Context;
using SCP.Security;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
//Инициализация
var config = builder.Configuration;

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<MyDbContext>(); //добавление с помощью Entity Framework Identity юзера, он берёт инфу для логина из БД

//Добавление строки подключения из AppSettings.json
builder.Services.AddDbContext<MyDbContext>(
    options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

//Добавление в конфигуры наш JWTSettings, короче токен
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

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
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.MapGroup("/identity").MapIdentityApi<IdentityUser>();
app.Run();

