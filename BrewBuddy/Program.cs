using BrewBuddy.Interface;
using BrewBuddy.Models;
using BrewBuddy.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BrewBuddy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddAuthorization();

            //dependenciinjektion - Vi registrere her vore brewbuddy i service, så vi kan bruge den i vores razorpage. 
            builder.Services.AddDbContext<BrewBuddyContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("BrewBuddyContext")));

            //dette registrere repositoriet, så jeg kan bruge det i razorpagen 
            builder.Services.AddScoped<IRepository<CoffieMachine>, CoffieMachineRepository>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();

            builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccesDenied";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(60);
            });

            builder.Services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin")); //ikke buges

                //vi laver her en policy, og tilføjer en claim til den policy (vi kan nu tilføje denne policy til vores side (humanresouce))
                options.AddPolicy("UserOnly",
                    policy => policy.RequireClaim("Role", "User"));

                options.AddPolicy("AdminOnly", policy => policy
                .RequireClaim("Role", "Admin"));

            });

            //
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            

            //tilføje authorization middleware, som er ansvarlig for at kalde the authorization haldeler  
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapRazorPages();

            app.Run();
        }
    }
}
