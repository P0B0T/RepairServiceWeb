﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Enum;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class StaffService : IStaffService
    {
        private readonly IBaseRepository<Staff> _staffRepository;

        public StaffService(IBaseRepository<Staff> staffRepository)
        {
            _staffRepository = staffRepository;
        }

        /// <summary>
        /// Метод для получения списка сотрудников
        /// </summary>
        /// <returns>Список сотрудников или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Staff>>> GetAll()
        {
            try
            {
                var staff = await _staffRepository.GetAll()
                                                  .Include(x => x.Role)
                                                  .ToListAsync();

                if (!staff.Any())
                {
                    return new BaseResponse<IEnumerable<Staff>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Staff>>()
                {
                    Data = staff,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Staff>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка сотрудников
        /// </summary>
        /// <param name="fullName"> - фио сотрудника</param>
        /// <param name="experiance"> - стаж сотрудника</param>
        /// <param name="post"> - должность сотрудника</param>
        /// <param name="role"> - роль сотрудника</param>
        /// <returns>Отфильтрованный список сотрудников или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Staff>>> GetFiltered(string fullName = "", int? experiance = null, string post = "", string role = null)
        {
            try
            {
                var staff = await _staffRepository.GetAll()
                                                  .Include(x => x.Role)
                                                  .ToListAsync();

                if (fullName != "")
                    staff = staff.Where(x => x.FullName.ToLower().Contains(fullName.ToLower()))
                                 .ToList();

                if (experiance != null)
                    staff = staff.Where(x => x.Experiance == experiance)
                                 .ToList();

                if (post != "")
                    staff = staff.Where(staff => staff.Post.ToLower() == post.ToLower())
                                 .ToList();

                if (role != null)
                    staff = staff.Where(x => x.Role.Role1 == role)
                                 .ToList();

                if (!staff.Any())
                {
                    return new BaseResponse<IEnumerable<Staff>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Staff>>()
                {
                    Data = staff,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Staff>>()
                {
                    Description = $"[GetFiltered] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения сотрудника по коду
        /// </summary>
        /// <param name="id"> - код сотрудника</param>
        /// <returns>Информация о сотруднике или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<StaffViewModel>> Get(int id)
        {
            try
            {
                var staff = await _staffRepository.GetAll()
                                                  .Include(x => x.Role)
                                                  .FirstOrDefaultAsync(x => x.Id == id);

                if (staff == null)
                {
                    return new BaseResponse<StaffViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.StaffNotFound
                    };
                }

                // Перенос данных сотрудника в ViewModel
                var data = new StaffViewModel()
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Surname = staff.Surname,
                    Patronymic = staff.Patronymic,
                    Experiance = staff.Experiance,
                    Post = staff.Post,
                    Salary = staff.Salary,
                    Date_of_employment = staff.DateOfEmployment,
                    Photo = staff.Photo,
                    RoleId = staff.RoleId,
                    Login = staff.Login,
                    Password = staff.Password,
                    Role = staff.Role
                };

                return new BaseResponse<StaffViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StaffViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения сотрудника по фио
        /// </summary>
        /// <param name="name"> - фио сотрудника</param>
        /// <returns>Список найденных сотрудника или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Staff>>> GetByName(string name)
        {
            try
            {
                var staff = (await _staffRepository.GetAll()
                                                  .Include(x => x.Role)
                                                  .ToListAsync())
                                                  .Where(x => x.FullName.ToLower().Contains(name.ToLower()));

                if (!staff.Any())
                {
                    return new BaseResponse<IEnumerable<Staff>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Staff>>()
                {
                    Data = staff,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Staff>>()
                {
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для удаления сотрудника
        /// </summary>
        /// <param name="id"> - код сотрудника</param>
        /// <returns>"Успех" или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var staff = await _staffRepository.GetAll()
                                                  .Include(x => x.Role)
                                                  .FirstOrDefaultAsync(x => x.Id == id);

                if (staff == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.StaffNotFound,
                        Data = false
                    };
                }

                if (staff.Photo != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/StaffPhoto", staff.Photo);

                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                await _staffRepository.Delete(staff);

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
        /// Метод для создания сотрудника
        /// </summary>
        /// <param name="staffViewModel"> - ViewModel</param>
        /// <returns>Добавленный сотрудник</returns>
        public async Task<IBaseResponse<Staff>> Create(StaffViewModel staffViewModel, IFormFile? file = null)
        {
            try
            {
                string fileName = null;

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/StaffPhoto", fileName);

                    var existingPhotos = _staffRepository.GetAll().Where(x => x.Photo.Contains(fileName));

                    if (existingPhotos.Any())
                    {
                        var count = existingPhotos.Count() + 1;
                        fileName = Path.GetFileNameWithoutExtension(fileName) + "(" + count.ToString() + ")" + Path.GetExtension(fileName);
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/StaffPhoto", fileName);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                // Перенос данных из ViewModel
                var staff = new Staff()
                {
                    Name = staffViewModel.Name,
                    Surname = staffViewModel.Surname,
                    Patronymic = staffViewModel.Patronymic,
                    Experiance = staffViewModel.Experiance,
                    Post = staffViewModel.Post,
                    Salary = staffViewModel.Salary,
                    DateOfEmployment = staffViewModel.Date_of_employment,
                    Photo = fileName,
                    RoleId = staffViewModel.RoleId,
                    Login = staffViewModel.Login,
                    Password = staffViewModel.Password,
                    Role = staffViewModel.Role
                };

                await _staffRepository.Create(staff);

                return new BaseResponse<Staff>()
                {
                    StatusCode = StatusCode.OK,
                    Data = staff
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Staff>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для редактирования сотрудника
        /// </summary>
        /// <param name="id"> - код сотрудника</param>
        /// <param name="staffViewModel"> - ViewModel</param>
        /// <returns>Изменённый сотрудник или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<Staff>> Edit(int id, StaffViewModel staffViewModel, IFormFile? file = null)
        {
            try
            {
                var staff = await _staffRepository.GetAll()
                                                  .Include(x => x.Role)
                                                  .FirstOrDefaultAsync(x => x.Id == id);

                if (staff == null)
                {
                    return new BaseResponse<Staff>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.StaffNotFound
                    };
                }

                if (staff.Photo != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/StaffPhoto", staff.Photo);

                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                string fileName = null;

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/StaffPhoto", fileName);

                    var existingPhotos = _staffRepository.GetAll().Where(x => x.Photo.Contains(fileName));

                    if (existingPhotos.Any())
                    {
                        var count = existingPhotos.Count() + 1;
                        fileName = Path.GetFileNameWithoutExtension(fileName) + "(" + count.ToString() + ")" + Path.GetExtension(fileName);
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/StaffPhoto", fileName);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                // Перенос данных из ViewModel
                staff.Name = staffViewModel.Name;
                staff.Surname = staffViewModel.Surname;
                staff.Patronymic = staffViewModel.Patronymic;
                staff.Experiance = staffViewModel.Experiance;
                staff.Post = staffViewModel.Post;
                staff.Salary = staffViewModel.Salary;
                staff.DateOfEmployment = staffViewModel.Date_of_employment;
                staff.Photo = fileName;
                staff.RoleId = staffViewModel.RoleId;
                staff.Login = staffViewModel.Login;
                staff.Password = staffViewModel.Password;

                await _staffRepository.Update(staff);

                return new BaseResponse<Staff>()
                {
                    Data = staff,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Staff>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}