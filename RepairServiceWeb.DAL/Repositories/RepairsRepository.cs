using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
{
    public class RepairsRepository : IBaseRepository<Repair>
    {
        private readonly ApplicationDbContext _context;

        public RepairsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления записи в таблицу "Ремонты"
        /// </summary>
        /// <param name="entity" - информация о ремонте></param>
        public async Task<bool> Create(Repair entity)
        {
            await _context.Repairs.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Ремонты"
        /// </summary>
        /// <param name="entity" - информация о ремонте></param>
        public async Task<bool> Delete(Repair entity)
        {
            _context.Repairs.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Ремонты"
        /// </summary>
        public IQueryable<Repair> GetAll()
        {
            return _context.Repairs;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Ремонты"
        /// </summary>
        /// <param name="entity" - информация о ремонте></param>
        public async Task<Repair> Update(Repair entity)
        {
            _context.Repairs.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
