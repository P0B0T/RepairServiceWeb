using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.DAL.Repositories;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Service.Implementations;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb
{
    public static class Initializer
    {
        /// <summary>
        /// Метод для инициализации репозиториев
        /// </summary>
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

        /// <summary>
        /// Метод для инициализации сервисов
        /// </summary>
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccessoriesService, AccessoriesService>();
            services.AddScoped<ISuppliersService, SuppliersService>();
            services.AddScoped<IOrderAccessoriesService, OrderAccessoriesService>();
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IDevicesService, DevicesService>();
            services.AddScoped<IRepairsService, RepairsService>();
            services.AddScoped<IRoleCheckerService, RoleCheckerService>();
        }
    }
}
