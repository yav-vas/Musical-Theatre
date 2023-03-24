using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Models;
using System.Reflection.Emit;

namespace Musical_Theatre.Data.Context;

public class Musical_TheatreContext : IdentityDbContext<User>
{
    public Musical_TheatreContext(DbContextOptions<Musical_TheatreContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    internal object Entry()
    {
        throw new NotImplementedException();
    }

    public DbSet<Hall> Halls { get; set; }
    public DbSet<Performance> Performances { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<PriceCategory> PriceCategories { get; set; }

}
