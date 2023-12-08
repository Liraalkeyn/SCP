using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SCP.Security;

public class GetToken
{
    private readonly JWTSettings _options; //использование настроек JWT

    public GetToken(IOptions<JWTSettings> optAccess) //
    {
        _options = optAccess.Value;
    }

    public string GenerateToken(string login, string password)
    {
        List<Claim> claims = new List<Claim>(); /*Лист это тот, кто получает токен. В лист мы закидываем роль, 
        имя и тд, то есть список параматров для одного человека */
        claims.Add(new Claim(ClaimTypes.Name, login));
        claims.Add(new Claim(ClaimTypes.Role, password)); //мы обозвали пароль ролью чтобы работало?? ⚆_⚆

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)); //Из опций достаём типа алгоритм JWT?? ⚆_⚆

        var jwt = new JwtSecurityToken( //Генерируем JWT, пихаем данные, которые в нем будут
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(120)),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt); //Возвращение jwt токена.
    }
}

