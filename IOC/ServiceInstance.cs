using DataAccess.GenericRepositoryAndUnitOfWork;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service;
using Utility.Notifications;
using Utility.PasswordHelper;
using Utility.StaticData;

namespace HRMApi
{
    public static class ServiceInstance
    {
        public static void RegisterHRMServiceInstance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(HRMContext));
            services.AddDbContext<HRMContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(StaticData.MIGRATION_ASSEMBLY)));
            services.AddScoped<DbContext, HRMContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(provider => configuration);


            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILeaveRequestService, LeaveRequestService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IWebPushService, WebPushService>();
            services.AddTransient<INotificationPublisherService, NotificationPublisherService>();
            services.AddTransient<INotificationRegisterService, NotificationRegisterService>();

            // generic DI
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
