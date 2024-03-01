using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;
using Diplom.Domain.Enum;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Service.Implementations
{
    public class AccessoriesService : IAccessoriesService
    {
        private readonly IBaseRepository<Accessory> _accessoriesRepository;

        public AccessoriesService(IBaseRepository<Accessory> accessoriesRepository)
        {
            _accessoriesRepository = accessoriesRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Accessory>>> GetAll()
        {
            try
            {
                var accessories = _accessoriesRepository.GetAll()
                                                        .Include(x => x.Supplier);

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

        public async Task<IBaseResponse<IEnumerable<Accessory>>> GetFiltered(string name = "", string manufacturer = "", string supplier = "")
        {
            try
            {
                var accessories = _accessoriesRepository.GetAll()
                                                        .Include(x => x.Supplier)
                                                        .ToList();

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

        public async Task<IBaseResponse<Accessory>> GetByName(string name)
        {
            try
            {
                var accessories = await _accessoriesRepository.GetAll()
                                                              .Include(x => x.Supplier)
                                                              .FirstOrDefaultAsync(x => x.Name.ToLower().Contains(name.ToLower()));

                if (accessories == null)
                {
                    return new BaseResponse<Accessory>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.AccessoriesNotFound
                    };
                }

                var data = new Accessory()
                {
                    Id = accessories.Id,
                    Name = accessories.Name,
                    Description = accessories.Description,
                    Manufacturer = accessories.Manufacturer,
                    Cost = accessories.Cost,
                    SupplierId = accessories.SupplierId,
                    Supplier = accessories.Supplier
                };

                return new BaseResponse<Accessory>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Accessory>()
                {
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

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

        public async Task<IBaseResponse<Accessory>> Create(AccessoriesViewModel accessoriesViewModel)
        {
            try
            {
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
