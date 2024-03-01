using Diplom.DAL.Interfaces;
using Diplom.DAL.Repositories;
using Diplom.Domain.Entity;
using Diplom.Service.Implementations;
using Diplom.Service.Interfaces;

namespace Diplom
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Accessory>, AccessoriesRepository>();
            services.AddScoped<IBaseRepository<Client>, ClientsRepository>();
            services.AddScoped<IBaseRepository<OrderAccessory>, OrderAccessoriesRepository>();
            services.AddScoped<IBaseRepository<Device>, DevicesRepository>();
            services.AddScoped<IBaseRepository<Repair>, RepairsRepository>();
            services.AddScoped<IBaseRepository<Staff>, StaffRepositoty>();
            services.AddScoped<IBaseRepository<Supplier>, SuppliersRepository>();
            services.AddScoped<IBaseRepository<Role>, RolesRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccessoriesService, AccessoriesService>();
            services.AddScoped<ISuppliersService, SuppliersService>();
            services.AddScoped<IOrderAccessoriesService, OrderAccessoriesService>();
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<IRolesService, RolesService>();
        }
    }
}
