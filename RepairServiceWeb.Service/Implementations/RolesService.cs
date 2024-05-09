using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Enum;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly IBaseRepository<Role> _roleRepository;

        public RolesService(IBaseRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Метод для получения списка ролей
        /// </summary>
        /// <returns>Список ролей или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Role>>> GetAll()
        {
            try
            {
                var roles = await _roleRepository.GetAll()
                                                 .ToListAsync();

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

        /// <summary>
        /// Метод для получения роли по коду
        /// </summary>
        /// <param name="id"> - код роли</param>
        /// <returns>Информация о роли или сообщение "Элемент не найден"</returns>
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

                // Перенос данных роли в ViewModel
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

        /// <summary>
        /// Метод для удаления роли
        /// </summary>
        /// <param name="id"> - код роли</param>
        /// <returns>"Успех" или сообщение "Элемент не найден"</returns>
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

        /// <summary>
        /// Метод для создания роли
        /// </summary>
        /// <param name="rolesViewModel"> - ViewModel</param>
        /// <returns>Добавленная роль</returns>
        public async Task<IBaseResponse<Role>> Create(RolesViewModel rolesViewModel)
        {
            try
            {
                // Перенос данных из ViewModel
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

        /// <summary>
        /// Метод для редактирования роли
        /// </summary>
        /// <param name="id"> - код роли</param>
        /// <param name="rolesViewModel"> - ViewModel</param>
        /// <returns>Изменённая роль или сообщение "Элемент не найден"</returns>
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

                // Перенос данных из ViewModel
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

        /// <summary>
        /// Метод для получения названия роли
        /// </summary>
        /// <param name="id"> - код роли</param>
        /// <returns>Название роли или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<string>> GetRoleName(int? id)
        {
            try
            {
                var roles = await _roleRepository.GetAll()
                                                 .FirstOrDefaultAsync(x => x.Id == id);

                if (roles == null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RoleNotFound
                    };
                }

                return new BaseResponse<string>()
                {
                    Data = roles.Role1,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $"[GetRoleName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
