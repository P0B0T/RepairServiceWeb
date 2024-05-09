using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
{
    public class SuppliersRepository : IBaseRepository<Supplier>
    {
        private readonly ApplicationDbContext _context;

        public SuppliersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления записи в таблицу "Поставщики"
        /// </summary>
        /// <param name="entity" - информация о поставщиках></param>
        public async Task<bool> Create(Supplier entity)
        {
            await _context.Suppliers.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Поставщики"
        /// </summary>
        /// <param name="entity" - информация о поставщиках></param>
        public async Task<bool> Delete(Supplier entity)
        {
            _context.Suppliers.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Поставщики"
        /// </summary>
        public IQueryable<Supplier> GetAll()
        {
            return _context.Suppliers;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Поставщики"
        /// </summary>
        /// <param name="entity" - информация о поставщиках></param>
        public async Task<Supplier> Update(Supplier entity)
        {
            _context.Suppliers.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
