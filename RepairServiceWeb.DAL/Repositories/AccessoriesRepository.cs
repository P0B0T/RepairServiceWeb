using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
{
    public class AccessoriesRepository : IBaseRepository<Accessory>
    {
        private readonly ApplicationDbContext _context;

        public AccessoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления записи в таблицу "Запчасти"
        /// </summary>
        /// <param name="entity" - информация о запчасти></param>
        public async Task<bool> Create(Accessory entity)
        {
            await _context.Accessories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Запчасти"
        /// </summary>
        /// <param name="entity" - информация о запчасти></param>
        public async Task<bool> Delete(Accessory entity)
        {
            _context.Accessories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Запчасти"
        /// </summary>
        public IQueryable<Accessory> GetAll()
        {
            return _context.Accessories;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Запчасти"
        /// </summary>
        /// <param name="entity" - информация о запчасти></param>
        public async Task<Accessory> Update(Accessory entity)
        {
            _context.Accessories.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}