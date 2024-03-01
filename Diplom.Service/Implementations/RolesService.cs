using Diplom.DAL.Interfaces;
using Diplom.DAL.Repositories;
using Diplom.Domain.Entity;
using Diplom.Domain.Enum;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Service.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly IBaseRepository<Role> _roleRepository;

        public RolesService(IBaseRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Role>>> GetAll()
        {
            try
            {
                var roles = _roleRepository.GetAll();

                if (!roles.Any())
                {
                    return new BaseResponse<IEnumerable<Role>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Role>>()
                {
                    Data = roles,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Role>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<RolesViewModel>> Get(int id)
        {
            try
            {
                var roles = await _roleRepository.GetAll()
                                           .FirstOrDefaultAsync(x => x.Id == id);

                if (roles == null)
                {
                    return new BaseResponse<RolesViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RoleNotFound
                    };
                }

                var data = new RolesViewModel()
                {
                    Id = roles.Id,
                    Role1 = roles.Role1,
                    Description = roles.Description
                };

                return new BaseResponse<RolesViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<RolesViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var roles = await _roleRepository.GetAll()
                                           .FirstOrDefaultAsync(x => x.Id == id);

                if (roles == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RoleNotFound,
                        Data = false
                    };
                }

                await _roleRepository.Delete(roles);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[Delete] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Role>> Create(RolesViewModel rolesViewModel)
        {
            try
            {
                var roles = new Role()
                {
                    Role1 = rolesViewModel.Role1,
                    Description = rolesViewModel.Description
                };

                await _roleRepository.Create(roles);

                return new BaseResponse<Role>()
                {
                    StatusCode = StatusCode.OK,
                    Data = roles
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Role>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Role>> Edit(int id, RolesViewModel rolesViewModel)
        {
            try
            {
                var roles = await _roleRepository.GetAll()
                                           .FirstOrDefaultAsync(x => x.Id == id);

                if (roles == null)
                {
                    return new BaseResponse<Role>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RoleNotFound
                    };
                }

                roles.Role1 = rolesViewModel.Role1;
                roles.Description = rolesViewModel.Description;

                await _roleRepository.Update(roles);

                return new BaseResponse<Role>()
                {
                    Data = roles,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Role>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
