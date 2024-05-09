using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
{
    public class StaffRepositoty : IBaseRepository<Staff>
    {
        private readonly ApplicationDbContext _context;

        public StaffRepositoty(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления записи в таблицу "Сотрудники"
        /// </summary>
        /// <param name="entity" - информация о сотруднике></param>
        public async Task<bool> Create(Staff entity)
        {
            await _context.Staff.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Сотрудники"
        /// </summary>
        /// <param name="entity" - информация о сотруднике></param>
        public async Task<bool> Delete(Staff entity)
        {
            _context.Staff.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Сотрудники"
        /// </summary>
        public IQueryable<Staff> GetAll()
        {
            return _context.Staff;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Сотрудники"
        /// </summary>
        /// <param name="entity" - информация о сотруднике></param>
        public async Task<Staff> Update(Staff entity)
        {
            _context.Staff.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
