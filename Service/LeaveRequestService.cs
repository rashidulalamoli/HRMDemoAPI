using DataAccess.GenericRepositoryAndUnitOfWork;
using Entity.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Notifications;
using Utility.StaticData;

namespace Service
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequestVm>> GetLeaveRequests();
        Task<LeaveRequestVm> GetLeaveRequest(string leaveId);
        Task<LeaveRequestVm> AddLeaveRequest(DataAccess.Models.LeaveRequest leave);
        Task<Status> EditLeaveRequest(LeaveRequestVm leave);
        Task<Status> DeleteLeaveRequest(string leaveId);
        Task<Status> ApproveLeaveRequest(LeaveRequestVm leave);
    }
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IRepository<DataAccess.Models.LeaveRequest> _repository;
        private readonly IRepository<DataAccess.Models.NotificationManager> _notificationManagerRepository;
        private readonly INotificationRegisterService _notificationRegisterService;
        private readonly IUnitOfWork _unitOfWork;
        public LeaveRequestService(IRepository<DataAccess.Models.LeaveRequest> repository, 
            IRepository<DataAccess.Models.NotificationManager> notificationManagerRepository,
            INotificationRegisterService notificationRegisterService,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _notificationManagerRepository = notificationManagerRepository;
            _notificationRegisterService = notificationRegisterService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LeaveRequestVm> AddLeaveRequest(DataAccess.Models.LeaveRequest leave)
        {
            try
            {
                leave.LeaveRequstGuid = Guid.NewGuid().ToString();
                leave.IsActive = true;
                leave.IsDeleted = false;
                leave.CreatedDate = DateTime.UtcNow;
                leave.ModifiedDate = DateTime.UtcNow;
                _repository.Add(leave);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    await NotifyByCriteria(leave.LeaveApprover,leave.LeaveApplication);
                    return ConvertToVm(leave);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Status> DeleteLeaveRequest(string leaveId)
        {
            try
            {
                DataAccess.Models.LeaveRequest leave = await _repository.FindAsync(x => x.LeaveRequstGuid == leaveId && x.IsActive == true && x.IsDeleted == false);
                _repository.Delete(leave);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return new Status
                    {
                        Message = StaticData.LEAVE_DELETE_SUCCESS,
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

        public async Task<Status> ApproveLeaveRequest(LeaveRequestVm leave)
        {
            try
            {
                leave.IsApproved = true;
                leave.LeaveApproveDate = DateTime.UtcNow;
                var editStat = await EditLeaveRequest(leave);
                if (editStat.StatusCode == StatusCodes.Status200OK)
                {
                    await NotifyByCriteria(leave.LeaveIssuer, leave.LeaveApplication);
                    return new Status
                    {
                        Message = StaticData.LEAVE_UPDATE_SUCCESS,
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
        public async Task<Status> EditLeaveRequest(LeaveRequestVm leave)
        {
            try
            {
                DataAccess.Models.LeaveRequest originalLeave = await _repository.FindAsync(x => x.LeaveRequstGuid == leave.LeaveRequstGuid && x.IsActive == true && x.IsDeleted == false);
                DataAccess.Models.LeaveRequest updatedLeave = AssignVmDataToEntity(leave, originalLeave);
                updatedLeave.ModifiedDate = DateTime.UtcNow;
                _repository.Update(updatedLeave);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return new Status
                    {
                        Message = StaticData.LEAVE_UPDATE_SUCCESS,
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

        public async Task<LeaveRequestVm> GetLeaveRequest(string leaveId)
        {
            try
            {
                DataAccess.Models.LeaveRequest leave = await _repository.FindAsync(x => x.LeaveRequstGuid == leaveId && x.IsActive == true && x.IsDeleted == false);
                return ConvertToVm(leave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<LeaveRequestVm>> GetLeaveRequests()
        {
            try
            {
                IEnumerable<DataAccess.Models.LeaveRequest> leaves = await _repository.FindAllAsync(x =>x.IsActive == true && x.IsDeleted == false);
                IEnumerable<LeaveRequestVm> result = leaves.Select(s => ConvertToVm(s));
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //helper methods
        private LeaveRequestVm ConvertToVm(DataAccess.Models.LeaveRequest leave)
        {
            return new LeaveRequestVm
            {
                LeaveRequstGuid = leave.LeaveRequstGuid,
                LeaveIssuer = leave.LeaveIssuer,
                LeaveApprover = leave.LeaveApprover,
                LeaveApplication = leave.LeaveApplication,
                LeaveStartDate = leave.LeaveStartDate,
                LeaveEndDate = leave.LeaveEndDate,
                LeaveIssueDate = leave.LeaveIssueDate,
                LeaveApproveDate = leave.LeaveApproveDate,
                IsApproved = leave.IsApproved
            };
        }
        private DataAccess.Models.LeaveRequest AssignVmDataToEntity(LeaveRequestVm leave, DataAccess.Models.LeaveRequest entity)
        {
            leave.LeaveIssuer = entity.LeaveIssuer;
            leave.LeaveApprover = entity.LeaveApprover;
            leave.LeaveApplication = entity.LeaveApplication;
            leave.LeaveStartDate = entity.LeaveStartDate;
            leave.LeaveEndDate = entity.LeaveEndDate;
            leave.LeaveIssueDate = entity.LeaveIssueDate;
            leave.LeaveApproveDate = entity.LeaveApproveDate;
            leave.IsApproved = entity.IsApproved;
            return entity;
            
        }

        private async Task NotifyByCriteria(int userId,string msgBody)
        {
            var notifications = await _notificationManagerRepository.FindAllAsync(x =>x.UserId == userId && x.IsActive == true && x.IsDeleted == false);
            List<int> notificationIds = new List<int>();
            Parallel.ForEach(notifications, notfy =>
            {
                notificationIds.Add(notfy.Notification.NotificationId);
            });

            _ = System.Threading.Tasks.Task.Factory.StartNew(delegate ()
            {
                _notificationRegisterService.RegisterNotificationByPreference(notificationIds, msgBody);
            });
        }
    }
}
