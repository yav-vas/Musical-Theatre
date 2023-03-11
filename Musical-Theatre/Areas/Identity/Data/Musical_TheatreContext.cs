using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Areas.Identity.Data;
using Musical_Theatre.Models;

namespace Musical_Theatre.Data;

public class Musical_TheatreContext : IdentityDbContext<Musical_TheatreUser>
{
    public Musical_TheatreContext(DbContextOptions<Musical_TheatreContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Musical_Theatre.Models.Hall> Hall { get; set; } = default!;
}
