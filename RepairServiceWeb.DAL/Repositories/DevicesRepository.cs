using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
{
    public class DevicesRepository : IBaseRepository<Device>
    {
        private readonly ApplicationDbContext _context;

        public DevicesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления записи в таблицу "Устройства"
        /// </summary>
        /// <param name="entity" - информация об устройстве></param>
        public async Task<bool> Create(Device entity)
        {
            await _context.Devices.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Устройства"
        /// </summary>
        /// <param name="entity" - информация об устройстве></param>
        public async Task<bool> Delete(Device entity)
        {
            _context.Devices.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Устройства"
        /// </summary>
        public IQueryable<Device> GetAll()
        {
            return _context.Devices;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Устройства"
        /// </summary>
        /// <param name="entity" - информация об устройстве></param>
        public async Task<Device> Update(Device entity)
        {
            _context.Devices.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
