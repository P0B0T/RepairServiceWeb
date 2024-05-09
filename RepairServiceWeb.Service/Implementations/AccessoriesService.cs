using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Enum;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class AccessoriesService : IAccessoriesService
    {
        private readonly IBaseRepository<Accessory> _accessoriesRepository;

        public AccessoriesService(IBaseRepository<Accessory> accessoriesRepository)
        {
            _accessoriesRepository = accessoriesRepository;
        }

        /// <summary>
        /// Метод для получения списка запчастей
        /// </summary>
        /// <returns>Список запчастей или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Accessory>>> GetAll()
        {
            try
            {
                var accessories = await _accessoriesRepository.GetAll()
                                                              .Include(x => x.Supplier)
                                                              .ToListAsync();

                if (!accessories.Any())
                {
                    return new BaseResponse<IEnumerable<Accessory>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Accessory>>()
                {
                    Data = accessories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Accessory>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка запчастей
        /// </summary>
        /// <param name="name"> - название запчасти</param>
        /// <param name="manufacturer"> - производитель запчасти</param>
        /// <param name="supplier"> - поставщик запчасти</param>
        /// <returns>Отфильтрованный список запчастей или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Accessory>>> GetFiltered(string name = "", string manufacturer = "", string supplier = "")
        {
            try
            {
                var accessories = await _accessoriesRepository.GetAll()
                                                              .Include(x => x.Supplier)
                                                              .ToListAsync();

                if (name != "")
                    accessories = accessories.Where(x => x.Name.ToLower().Contains(name.ToLower()))
                                             .ToList();

                if (manufacturer != "")
                    accessories = accessories.Where(x => x.Manufacturer.ToLower().Contains(manufacturer.ToLower()))
                                             .ToList();

                if (supplier != "")
                    accessories = accessories.Where(x => x.Supplier.CompanyName.ToLower().Contains(supplier.ToLower()))
                                             .ToList();

                if (!accessories.Any())
                {
                    return new BaseResponse<IEnumerable<Accessory>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Accessory>>()
                {
                    Data = accessories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Accessory>>()
                {
                    Description = $"[GetFiltered] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения запчасти по коду
        /// </summary>
        /// <param name="id"> - код запчасти</param>
        /// <returns>Информация о запчасти или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<AccessoriesViewModel>> Get(int id)
        {
            try
            {
                var accessories = await _accessoriesRepository.GetAll()
                                                              .Include(x => x.Supplier)
                                                              .FirstOrDefaultAsync(x => x.Id == id);

                if (accessories == null)
                {
                    return new BaseResponse<AccessoriesViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.AccessoriesNotFound
                    };
                }

                // Перенос данных запчасти в ViewModel
                var data = new AccessoriesViewModel()
                {
                    Id = accessories.Id,
                    Name = accessories.Name,
                    Description = accessories.Description,
                    Manufacturer = accessories.Manufacturer,
                    Cost = accessories.Cost,
                    SupplierId = accessories.SupplierId,
                    Supplier = accessories.Supplier
                };

                return new BaseResponse<AccessoriesViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoriesViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения запчастей по названию
        /// </summary>
        /// <param name="name"> - название запчасти</param>
        /// <returns>Список найденных запчастей или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Accessory>>> GetByName(string name)
        {
            try
            {
                var accessories = (await _accessoriesRepository.GetAll()
                                                               .Include(x => x.Supplier)
                                                               .ToListAsync())
                                                               .Where(x => x.Name.ToLower().Contains(name.ToLower()));

                if (!accessories.Any())
                {
                    return new BaseResponse<IEnumerable<Accessory>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Accessory>>()
                {
                    Data = accessories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Accessory>>()
                {
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для удаления запчасти
        /// </summary>
        /// <param name="id"> - код запчасти</param>
        /// <returns>"Успех" или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var accessories = await _accessoriesRepository.GetAll()
                                                              .FirstOrDefaultAsync(x => x.Id == id);

                if (accessories == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.AccessoriesNotFound,
                        Data = false
                    };
                }

                await _accessoriesRepository.Delete(accessories);

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
        /// Метод для создания запчасти
        /// </summary>
        /// <param name="accessoriesViewModel"> - ViewModel</param>
        /// <returns>Добавленная запчасть</returns>
        public async Task<IBaseResponse<Accessory>> Create(AccessoriesViewModel accessoriesViewModel)
        {
            try
            {
                // Перенос данных из ViewModel
                var accessories = new Accessory()
                {
                    Name = accessoriesViewModel.Name,
                    Description = accessoriesViewModel.Description,
                    Manufacturer = accessoriesViewModel.Manufacturer,
                    Cost = accessoriesViewModel.Cost,
                    SupplierId = accessoriesViewModel.SupplierId,
                    Supplier = accessoriesViewModel.Supplier
                };

                await _accessoriesRepository.Create(accessories);

                return new BaseResponse<Accessory>()
                {
                    StatusCode = StatusCode.OK,
                    Data = accessories
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Accessory>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для редактирования запчасти
        /// </summary>
        /// <param name="id"> - код запчасти</param>
        /// <param name="accessoriesViewModel"> - ViewModel</param>
        /// <returns>Изменённая запчасть или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<Accessory>> Edit(int id, AccessoriesViewModel accessoriesViewModel)
        {
            try
            {
                var accessories = await _accessoriesRepository.GetAll()
                                                              .FirstOrDefaultAsync(x => x.Id == id);

                if (accessories == null)
                {
                    return new BaseResponse<Accessory>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.AccessoriesNotFound
                    };
                }

                // Перенос данных из ViewModel
                accessories.Name = accessoriesViewModel.Name;
                accessories.Description = accessoriesViewModel.Description;
                accessories.Manufacturer = accessoriesViewModel.Manufacturer;
                accessories.Cost = accessoriesViewModel.Cost;
                accessories.SupplierId = accessoriesViewModel.SupplierId;

                await _accessoriesRepository.Update(accessories);

                return new BaseResponse<Accessory>()
                {
                    Data = accessories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Accessory>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
