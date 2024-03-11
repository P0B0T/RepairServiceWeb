using Diplom.Domain.Entity;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;

namespace Diplom.Service.Interfaces
{
    public interface IRepairsService : IBaseService<Repair>
    {
        Task<IBaseResponse<RepairsViewModel>> Get(int id);

        Task<IBaseResponse<Repair>> Create(RepairsViewModel model);

        Task<IBaseResponse<Repair>> Edit(int id, RepairsViewModel model);

        Task<IBaseResponse<IEnumerable<Repair>>> GetFiltered(string clientFullName = "", string staffFullName = "");
    }
}
