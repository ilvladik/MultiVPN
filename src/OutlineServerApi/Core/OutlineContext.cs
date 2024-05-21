using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OutlineServerApi.Core.Models;
using System.Reflection;

namespace OutlineServerApi.Core
{
    internal class OutlineContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<Country> Countries { get; set; }
        public OutlineContext(DbContextOptions<OutlineContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
