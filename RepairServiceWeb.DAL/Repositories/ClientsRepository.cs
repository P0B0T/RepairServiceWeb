using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
{
    public class ClientsRepository : IBaseRepository<Client>
    {
        private readonly ApplicationDbContext _context;

        public ClientsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления записи в таблицу "Клиенты"
        /// </summary>
        /// <param name="entity" - информация о клиенте></param>
        public async Task<bool> Create(Client entity)
        {
            await _context.Clients.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Клиенты"
        /// </summary>
        /// <param name="entity" - информация о клиенте></param>
        public async Task<bool> Delete(Client entity)
        {
            _context.Clients.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Клиенты"
        /// </summary>
        public IQueryable<Client> GetAll()
        {
            return _context.Clients;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Клиенты"
        /// </summary>
        /// <param name="entity" - информация о клиенте></param>
        public async Task<Client> Update(Client entity)
        {
            _context.Clients.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
