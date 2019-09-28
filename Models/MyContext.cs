using Microsoft.EntityFrameworkCore;
using SafeSpace.Models;
namespace SafeSpace.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<Users> users {get;set;}
        public DbSet<UserHaveFriends> UserhasFriends {get;set;}
        public DbSet<Friends> Friends {get;set;}
    }
}
