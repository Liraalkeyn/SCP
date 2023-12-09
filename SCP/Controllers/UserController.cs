using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SCP.Context;
using SCP.Entities;
using SCP.Security;

namespace SCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<user>>> Getusers()
        {
            return await _context.users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<user>> Getuser(int id)
        {
            var user = await _context.users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putuser(int id, user user)
        {
            if (id != user.userID)
            {
                return BadRequest();
            }

            _context.users.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<user>> Postuser(user user)
        {
            _context.users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (userExists(user.userID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("Getuser", new { id = user.userID }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteuser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool userExists(int id)
        {
            return _context.users.Any(e => e.userID == id);
        }

       /* 
        private readonly JWTSettings _options; //использование настроек JWT

        public UserController(IOptions<JWTSettings> optAccess) //
        {
            _options = optAccess.Value;
        }
        
        
        //Сгенерили токен юзера
        [HttpGet("GenerateToken")]
        public string GenerateToken(string login, string password)
        {
            List<Claim> claims = new List<Claim>(); Лист это тот, кто получает токен. В лист мы закидываем роль, 
        имя и тд, то есть список параматров для одного человека 
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
        } */
    }
}
