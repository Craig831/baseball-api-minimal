using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class GameDb : DbContext
    {
        public GameDb(DbContextOptions<GameDb> options)
            : base(options) { }

        public DbSet<Game> Games => Set<Game>();
        public DbSet<Team> Teams => Set<Team>();
    }
}
