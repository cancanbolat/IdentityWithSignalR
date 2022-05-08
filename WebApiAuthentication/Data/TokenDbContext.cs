using Microsoft.EntityFrameworkCore;

namespace WebApiAuthentication.Data
{
    public class TokendbContext : DbContext
    {
        public TokendbContext(DbContextOptions<TokendbContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
