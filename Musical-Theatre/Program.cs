using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Services;

namespace Musical_Theatre
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var connectionString = builder.Configuration.GetConnectionString("Musical_TheatreContextConnection") ?? throw new InvalidOperationException("Connection string 'Musical_TheatreContextConnection' not found.");

			builder.Services.AddDbContext<Musical_TheatreContext>(options => options.UseMySQL(connectionString));

			builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Musical_TheatreContext>();
			
			// AddHall services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddScoped<HallService, HallService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}