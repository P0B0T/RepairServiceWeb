using Diplom.Domain.Entity;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;

namespace Diplom.Service.Interfaces
{
    public interface ISuppliersService : IBaseService<Supplier>
    {
        Task<IBaseResponse<SuppliersViewModel>> Get(int id);

        Task<IBaseResponse<Supplier>> Create(SuppliersViewModel model);

        Task<IBaseResponse<Supplier>> Edit(int id, SuppliersViewModel model);

        Task<IBaseResponse<IEnumerable<Supplier>>> GetFiltered(string address = "");
    }
}
