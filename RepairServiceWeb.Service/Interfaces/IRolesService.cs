using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IRolesService
    {
        Task<IBaseResponse<IEnumerable<Role>>> GetAll();

        Task<IBaseResponse<bool>> Delete(int id);

        Task<IBaseResponse<RolesViewModel>> Get(int id);

        Task<IBaseResponse<Role>> Create(RolesViewModel model);

        Task<IBaseResponse<Role>> Edit(int id, RolesViewModel model);

        Task<IBaseResponse<string>> GetRoleName(int? permissionId);
    }
}
