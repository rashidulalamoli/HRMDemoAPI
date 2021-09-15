using DataAccess.GenericRepositoryAndUnitOfWork;
using DataAccess.Models;
using Entity.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Notifications;
using Utility.PasswordHelper;
using Utility.StaticData;

namespace Service
{
    public interface IUserService
    {
        Task<IEnumerable<UserVm>> GetUsers();
        Task<Status> AddUser(User user);
    }
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHasher _hasher;
        private readonly INotificationRegisterService _notificationRegisterService;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IRepository<User> repository, IPasswordHasher hasher, INotificationRegisterService notificationRegisterService, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _hasher = hasher;
            _notificationRegisterService = notificationRegisterService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Status> AddUser(User user)
        {
            try
            {
                Status status = new Status();
                if (await CheckUserExist(user) == false)
                {
                    string hashValue = _hasher.Hash(user.Password);
                    user.PasswordHash = hashValue;
                    user.UserGuid = Guid.NewGuid().ToString();
                    user.CreatedDate = DateTime.UtcNow;
                    user.ModifiedDate = DateTime.UtcNow;
                    user.IsActive = true;
                    user.IsDeleted = false;
                    _repository.Add(user);
                    if (await _unitOfWork.SaveAsync() > 0)
                    {
                        _ = System.Threading.Tasks.Task.Factory.StartNew(delegate ()
                        {
                            _notificationRegisterService.RegisterNotification("User Created Successfully");
                        });
                        status.Message = StaticData.USER_ADD_SUCCESS;
                        status.StatusCode = StatusCodes.Status200OK;
                        return status;
                    }
                    else
                    {
                        status.Message = StaticData.ERROR_TYPE_NONE;
                        status.StatusCode = StatusCodes.Status400BadRequest;
                        return status;
                    }
                }
                else
                {
                    status.Message = StaticData.USER_EXIST;
                    status.StatusCode = StatusCodes.Status409Conflict;
                    return status;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> CheckUserExist(User user)
        {
            try
            {
                User existinguser = await _repository.FindAsync(x => x.Email == user.Email && x.IsActive == true && x.IsDeleted == false);
                if (existinguser == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<UserVm>> GetUsers()
        {
            try
            {
                var users = await _repository.FindAllAsync(x => x.IsActive == true && x.IsDeleted == false);
                var result = users.Select(s => new UserVm
                {

                    Email = s.Email,
                    FirstName = s.FirstName,
                    FullName = s.FullName,
                    Image = s.Image,
                    Initials = s.Initials,
                    LastName = s.LastName,
                    UserName = s.UserName,
                    UserGuid = s.UserGuid,
                    Phone = s.Phone,
                    RoleId = s.RoleId
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Status> EditUser(UserVm user)
        {
            try
            {
                User originalUser = await _repository.FindAsync(x => x.UserGuid == user.UserGuid && x.IsActive == true && x.IsDeleted == false);
                User updatedUser = AssignVmDataToEntity(user, originalUser);
                updatedUser.ModifiedDate = DateTime.UtcNow;
                _repository.Update(updatedUser);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    _ = System.Threading.Tasks.Task.Factory.StartNew(delegate ()
                    {
                        _notificationRegisterService.RegisterNotification("User Updated Successfully");
                    });
                   
                    return new Status
                    {
                        Message = StaticData.USER_UPDATE_SUCCESS,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new Status
                    {
                        Message = StaticData.ERROR_TYPE_NONE,
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private DataAccess.Models.User AssignVmDataToEntity(UserVm user, DataAccess.Models.User entity)
        {
            entity.Email = user.Email;
            entity.FirstName = user.FirstName;
            entity.FullName = user.FullName;
            entity.Image = user.Image;
            entity.Initials = user.Initials;
            entity.LastName = user.LastName;
            entity.UserName = user.UserName;
            entity.UserGuid = user.UserGuid;
            entity.Phone = user.Phone;
            entity.RoleId = user.RoleId;
            return entity;

        }

    }
}
