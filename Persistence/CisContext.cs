using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;



namespace Persistence;

public class CisContext : DbContext
{
    public CisContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Topic> Topics { get; set; }
    public DbSet<Idea> Ideas { get; set; }
    public DbSet<Vote> Votes { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
}