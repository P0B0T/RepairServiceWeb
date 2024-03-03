using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;
using Diplom.Domain.Enum;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Diplom.Service.Implementations
{
    public class StaffService : IStaffService
    {
        private readonly IBaseRepository<Staff> _staffRepository;

        public StaffService(IBaseRepository<Staff> staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Staff>>> GetAll()
        {
            try
            {
                var staff = _staffRepository.GetAll()
                                            .Include(x => x.Role);

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

        public async Task<IBaseResponse<IEnumerable<Staff>>> GetFiltered(string fullName = "", int? experiance = null, string post = "", string role = null)
        {
            try
            {
                var staff = _staffRepository.GetAll()
                                            .Include(x => x.Role)
                                            .ToList();

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

        public async Task<IBaseResponse<Staff>> GetByName(string name)
        {
            try
            {
                var staff = (await _staffRepository.GetAll()
                                                  .Include(x => x.Role)
                                                  .ToListAsync())
                                                  .FirstOrDefault(x => x.FullName.ToLower().Contains(name.ToLower()));

                if (staff == null)
                {
                    return new BaseResponse<Staff>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.StaffNotFound
                    };
                }

                var data = new Staff()
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Surname = staff.Surname,
                    Patronymic = staff.Patronymic,
                    Experiance = staff.Experiance,
                    Post = staff.Post,
                    Salary = staff.Salary,
                    DateOfEmployment = staff.DateOfEmployment,
                    Photo = staff.Photo,
                    RoleId = staff.RoleId,
                    Login = staff.Login,
                    Password = staff.Password,
                    Role = staff.Role
                };

                return new BaseResponse<Staff>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Staff>()
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

                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
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

        public async Task<IBaseResponse<Staff>> Create(StaffViewModel staffViewModel, IFormFile? file = null)
        {
            try
            {
                string fileName = null;

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);

                    var existingPhotos = _staffRepository.GetAll().Where(x => x.Photo.Contains(fileName));

                    if (existingPhotos.Any())
                    {
                        var count = existingPhotos.Count() + 1;
                        fileName = Path.GetFileNameWithoutExtension(fileName) + "(" + count.ToString() + ")" + Path.GetExtension(fileName);
                    }

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/StaffPhoto", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

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

                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
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
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

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
