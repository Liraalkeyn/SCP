using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SCP.Context;

public class LoginDBContext : IdentityDbContext
{
    public LoginDBContext(DbContextOptions<LoginDBContext> options) : base(options)
    {
        
    }
}