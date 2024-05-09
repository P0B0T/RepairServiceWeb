using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Enum;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class DevicesService : IDevicesService
    {
        private readonly IBaseRepository<Device> _devicesRepository;

        public DevicesService(IBaseRepository<Device> devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        /// <summary>
        /// Метод для получения списка устройств
        /// </summary>
        /// <returns>Список устройств или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Device>>> GetAll()
        {
            try
            {
                var devices = await _devicesRepository.GetAll()
                                                      .Include(x => x.Client)
                                                      .ToListAsync();

                if (!devices.Any())
                {
                    return new BaseResponse<IEnumerable<Device>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Device>>()
                {
                    Data = devices,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Device>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка устройств
        /// </summary>
        /// <param name="manufacturer"> - производитель устройства</param>
        /// <param name="type"> - тип устройства</param>
        /// <param name="clientFullName"> - фио клиента</param>
        /// <returns>Отфильтрованный список устройств или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Device>>> GetFiltered(string manufacturer = "", string type = "", string clientFullName = "")
        {
            try
            {
                var devices = await _devicesRepository.GetAll()
                                                      .Include(x => x.Client)
                                                      .ToListAsync();

                if (manufacturer != "")
                    devices = devices.Where(x => x.Manufacturer.ToLower().Contains(manufacturer.ToLower())).ToList();

                if (type != "")
                    devices = devices.Where(x => x.Type.ToLower().Contains(type.ToLower())).ToList();

                if (clientFullName != "")
                    devices = devices.Where(x => x.Client.FullName.ToLower().Contains(clientFullName.ToLower())).ToList();

                if (!devices.Any())
                {
                    return new BaseResponse<IEnumerable<Device>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Device>>()
                {
                    Data = devices,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Device>>()
                {
                    Description = $"[GetFiltered] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка устрйоств по пользвоателю
        /// </summary>
        /// <param name="userId"> - код пользователя</param>
        /// <param name="login"> - логин</param>
        /// <param name="password"> - пароль</param>
        /// <returns>Отфильтрованный список устройств или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Device>>> GetFilteredByUser(int? userId, string login = "", string password = "")
        {
            try
            {
                var clientsDevice = (await _devicesRepository.GetAll()
                                                             .Include(x => x.Client)
                                                             .ToListAsync())
                                                             .Where(x => x.Client.Id == userId && x.Client.Login == login && x.Client.Password == password);

                if (!clientsDevice.Any())
                {
                    return new BaseResponse<IEnumerable<Device>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Device>>()
                {
                    Data = clientsDevice,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Device>>()
                {
                    Description = $"[GetFilteredByUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения устройства по коду
        /// </summary>
        /// <param name="id"> - код устройства</param>
        /// <returns>Информация об устройстве или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<DevicesViewModel>> Get(int id)
        {
            try
            {
                var devices = await _devicesRepository.GetAll()
                                                      .Include(x => x.Client)
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (devices == null)
                {
                    return new BaseResponse<DevicesViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.DevicesNotFound
                    };
                }

                // Перенос данных запчасти в ViewModel
                var data = new DevicesViewModel()
                {
                    Id = devices.Id,
                    ModelDevice = devices.Model,
                    Manufacturer = devices.Manufacturer,
                    Type = devices.Type,
                    Year_of_release = devices.YearOfRelease,
                    Serial_number = devices.SerialNumber,
                    ClientId = devices.ClientId,
                    Photo = devices.Photo,
                    Client = devices.Client
                };

                return new BaseResponse<DevicesViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<DevicesViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения устройств по названию
        /// </summary>
        /// <param name="name"> - название устрйоства</param>
        /// <returns>Список найденных устройств или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Device>>> GetByName(string name)
        {
            try
            {
                var devices = (await _devicesRepository.GetAll()
                                                       .Include(x => x.Client)
                                                       .ToListAsync())
                                                       .Where(x => x.Model.ToLower().Contains(name.ToLower()));

                if (!devices.Any())
                {
                    return new BaseResponse<IEnumerable<Device>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Device>>()
                {
                    Data = devices,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Device>>()
                {
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для удаления устройства
        /// </summary>
        /// <param name="id"> - код устройства</param>
        /// <returns>"Успех" или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var devices = await _devicesRepository.GetAll()
                                                      .Include(x => x.Client)
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (devices == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.DevicesNotFound,
                        Data = false
                    };
                }

                if (devices.Photo != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/DevicesPhoto", devices.Photo);

                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                await _devicesRepository.Delete(devices);

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
        /// Метод для создания устройства
        /// </summary>
        /// <param name="devicesViewModel"> - ViewModel</param>
        /// <returns>Добавленное устройство</returns>
        public async Task<IBaseResponse<Device>> Create(DevicesViewModel devicesViewModel, IFormFile? file = null)
        {
            try
            {
                string fileName = null;

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/DevicesPhoto", fileName);

                    var existingPhotos = _devicesRepository.GetAll().Where(x => x.Photo.Contains(fileName));

                    if (existingPhotos.Any())
                    {
                        var count = existingPhotos.Count() + 1;
                        fileName = Path.GetFileNameWithoutExtension(fileName) + "(" + count.ToString() + ")" + Path.GetExtension(fileName);
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/DevicesPhoto", fileName);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                // Перенос данных из ViewModel
                var devices = new Device()
                {
                    Model = devicesViewModel.ModelDevice,
                    Manufacturer = devicesViewModel.Manufacturer,
                    Type = devicesViewModel.Type,
                    YearOfRelease = devicesViewModel.Year_of_release,
                    SerialNumber = devicesViewModel.Serial_number,
                    ClientId = devicesViewModel.ClientId,
                    Photo = fileName,
                    Client = devicesViewModel.Client
                };

                await _devicesRepository.Create(devices);

                return new BaseResponse<Device>()
                {
                    StatusCode = StatusCode.OK,
                    Data = devices
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Device>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для редактирования устройства
        /// </summary>
        /// <param name="id"> - код устройства</param>
        /// <param name="devicesViewModel"> - ViewModel</param>
        /// <returns>Изменённое устройство или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<Device>> Edit(int id, DevicesViewModel devicesViewModel, IFormFile? file = null)
        {
            try
            {
                var devices = await _devicesRepository.GetAll()
                                                      .Include(x => x.Client)
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (devices == null)
                {
                    return new BaseResponse<Device>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.DevicesNotFound
                    };
                }

                if (devices.Photo != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/DevicesPhoto", devices.Photo);

                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                string fileName = null;

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/DevicesPhoto", fileName);

                    var existingPhotos = _devicesRepository.GetAll().Where(x => x.Photo.Contains(fileName));

                    if (existingPhotos.Any())
                    {
                        var count = existingPhotos.Count() + 1;
                        fileName = Path.GetFileNameWithoutExtension(fileName) + "(" + count.ToString() + ")" + Path.GetExtension(fileName);
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/DevicesPhoto", fileName);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                // Перенос данных из ViewModel
                devices.Model = devicesViewModel.ModelDevice;
                devices.Manufacturer = devicesViewModel.Manufacturer;
                devices.Type = devicesViewModel.Type;
                devices.YearOfRelease = devicesViewModel.Year_of_release;
                devices.SerialNumber = devicesViewModel.Serial_number;
                devices.ClientId = devicesViewModel.ClientId;
                devices.Photo = fileName;

                await _devicesRepository.Update(devices);

                return new BaseResponse<Device>()
                {
                    Data = devices,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Device>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
