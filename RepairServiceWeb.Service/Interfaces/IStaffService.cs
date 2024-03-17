using Microsoft.AspNetCore.Http;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IStaffService : IBaseService<Staff>
    {
        Task<IBaseResponse<StaffViewModel>> Get(int id);

        Task<IBaseResponse<Staff>> Create(StaffViewModel model, IFormFile? file = null);

        Task<IBaseResponse<Staff>> Edit(int id, StaffViewModel model, IFormFile? file = null);

        Task<IBaseResponse<IEnumerable<Staff>>> GetFiltered(string fullName = "", int? experiance = null, string post = "", string role = null);
    }
}
