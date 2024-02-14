using Microsoft.EntityFrameworkCore;
using the_library.Models;

namespace the_library.Data
{
    public class TheLibraryDbContext : DbContext
    {
        public TheLibraryDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

    }
}
