using RepairServiceWeb.Domain.Response;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IBaseService<T>
    {
        Task<IBaseResponse<IEnumerable<T>>> GetAll();

        Task<IBaseResponse<T>> GetByName(string name);

        Task<IBaseResponse<bool>> Delete(int id);
    }
}
