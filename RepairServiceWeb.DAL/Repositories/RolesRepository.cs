using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
{
    public class RolesRepository : IBaseRepository<Role>
    {
        private readonly ApplicationDbContext _context;

        public RolesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления записи в таблицу "Роли"
        /// </summary>
        /// <param name="entity" - информация о роли></param>
        public async Task<bool> Create(Role entity)
        {
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Роли"
        /// </summary>
        /// <param name="entity" - информация о роли></param>
        public async Task<bool> Delete(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Роли"
        /// </summary>
        public IQueryable<Role> GetAll()
        {
            return _context.Roles;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Роли"
        /// </summary>
        /// <param name="entity" - информация о роли></param>
        public async Task<Role> Update(Role entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
