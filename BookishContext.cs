using Bookish_v2.Models;
using Microsoft.EntityFrameworkCore;
namespace Bookish_v2_DB
{
    public class BookishContext : DbContext
    {
        // Put all the tables you want in your database here
        public DbSet<BookViewModel> Books { get; set; }
        public DbSet<MemberViewModel> Members { get; set; }
        public DbSet<MemberBookViewModel> MemberBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This is the configuration used for connecting to the database
            optionsBuilder
                .UseNpgsql(@"Server=localhost;Port=5432;Database=bookish_v2;User Id=bookish;Password=bookish;");
        }
    }
}
