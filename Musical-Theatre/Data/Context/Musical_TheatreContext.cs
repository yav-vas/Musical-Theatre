using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Musical_Theatre.Data.Context;

public class Musical_TheatreContext : IdentityDbContext<User>
{
    public Musical_TheatreContext(DbContextOptions<Musical_TheatreContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Performance>()
            .HasOne(p => p.Hall)
            .WithMany(h => h.Performances)
            .HasForeignKey(p => p.HallId);


        base.OnModelCreating(builder);
     
    }

    public DbSet<Hall> Halls { get; set; }
    public DbSet<Performance> Performances { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<PriceCategory> PriceCategories { get; set; }

}
