using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IRepairsService : IBaseService<Repair>
    {
        Task<IBaseResponse<RepairsViewModel>> Get(int id);

        Task<IBaseResponse<Repair>> Create(RepairsViewModel model);

        Task<IBaseResponse<Repair>> Edit(int id, RepairsViewModel model);

        Task<IBaseResponse<IEnumerable<Repair>>> GetFiltered(string clientFullName = "", string staffFullName = "");

        Task<IBaseResponse<IEnumerable<Repair>>> GetFilteredByUser(int? userId, string login = "", string password = "");
    }
}
