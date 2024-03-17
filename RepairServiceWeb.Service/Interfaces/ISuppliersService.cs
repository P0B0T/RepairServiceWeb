using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface ISuppliersService : IBaseService<Supplier>
    {
        Task<IBaseResponse<SuppliersViewModel>> Get(int id);

        Task<IBaseResponse<Supplier>> Create(SuppliersViewModel model);

        Task<IBaseResponse<Supplier>> Edit(int id, SuppliersViewModel model);

        Task<IBaseResponse<IEnumerable<Supplier>>> GetFiltered(string address = "");
    }
}
