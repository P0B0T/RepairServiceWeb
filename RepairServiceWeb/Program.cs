using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            // Получение строки подключения к базе данных
            var connection = builder.Configuration.GetConnectionString("MSSQLSERVER");

            // Добавление контекста базы данных в сервисы
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connection));

            builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Инициализация репозиториев и сервисов
            builder.Services.InitializeRepositories();
            builder.Services.InitializeServices();

            // Настройка аутентификации
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

            // Добавление авторизации
            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
                // Получение контекста базы данных
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Проверка, была ли база данных только что создана
                if (context.Database.EnsureCreated())
                {
                    // Если база данных только что создана:
                    // добавляем роль администратора;
                    context.Roles.Add(new Role { Role1 = "Administrator" });
                    context.SaveChanges();

                    // получаем ID роли администратора;
                    var adminRoleId = context.Roles.FirstOrDefault(r => r.Role1.ToLower().Contains("admin") || r.Role1.ToLower().Contains("админ"))?.Id;

                    // добавляем сотрудника с ролью администратора.
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
