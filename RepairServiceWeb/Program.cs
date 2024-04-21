using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RepairServiceWeb.Controllers;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
using System.Text.Json.Serialization;

namespace RepairServiceWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connection = builder.Configuration.GetConnectionString("MSSQLSERVER");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connection));

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.InitializeRepositories();
            builder.Services.InitializeServices();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = AutorizationOptions.ISSUER,
                        ValidAudience = AutorizationOptions.AUDIENCE,

                        IssuerSigningKey = AutorizationOptions.GetSymmetricSecurityKey(),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddAuthorization();

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

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Autorization}/{action=Index}/{id?}");

            using (var serviceScope = app.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!context.Roles.Any(r => r.Role1.ToLower().Contains("admin") || r.Role1.ToLower().Contains("админ")))
                {
                    // Create the Administrator role
                    context.Roles.Add(new Role { Role1 = "Administrator" });
                    context.SaveChanges();
                }

                var adminRoleId = context.Roles.FirstOrDefault(r => r.Role1.ToLower().Contains("admin") || r.Role1.ToLower().Contains("админ"))?.Id;

                // Check if the staff member already exists
                if (!context.Staff.Any(s => s.RoleId == adminRoleId))
                {
                    // Create the Administrator staff member
                    context.Staff.Add(new Staff 
                    { 
                        Name = "Admin", 
                        Surname = "Admin",
                        Post = "Admin",
                        Salary = 0,
                        DateOfEmployment = default,
                        RoleId = adminRoleId.Value,
                        Login = "Admin",
                        Password = "admin"
                    });
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
