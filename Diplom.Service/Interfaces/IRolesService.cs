using Diplom.Domain.Entity;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;

namespace Diplom.Service.Interfaces
{
    public interface IRolesService
    {
        Task<IBaseResponse<IEnumerable<Role>>> GetAll();

        Task<IBaseResponse<bool>> Delete(int id);

        Task<IBaseResponse<RolesViewModel>> Get(int id);

        Task<IBaseResponse<Role>> Create(RolesViewModel model);

        Task<IBaseResponse<Role>> Edit(int id, RolesViewModel model);
    }
}
