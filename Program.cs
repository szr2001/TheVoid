using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheVoid.Data;
using TheVoid.Interfaces;
using TheVoid.Models;

namespace TheVoid
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("default");
            // Add services to the container.
            builder.Services.AddDbContext<VoidDbContext>
                (
                    options => options.UseInMemoryDatabase("VoidTestDb")
                    //options => options.UseSqlServer(connectionString)
                );

            builder.Services.AddScoped<IAddsHandler, AddsenseAdds>();

            builder.Services.AddIdentity<VoidUser, IdentityRole>()
                .AddEntityFrameworkStores<VoidDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddControllersWithViews();

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
                pattern: "{controller=Account}/{action=WelcomeToVoid}/{id?}");

            app.Run();
        }
    }
}
