using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PuzzleServer.Models;
using System;

namespace PuzzleServer
{
    public class DbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Score> Scores { get; set; }
        public DbSet<Backup> Backups { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
