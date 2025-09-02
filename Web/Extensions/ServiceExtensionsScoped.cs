using Business.Classes;
using Business.Classes.Base;
using Business.Implementations.Operational;
using Business.Implementations.Operational;
using Business.Implementations.Organizational.Assignment;
using Business.Implementations.Organizational.Assignment;
using Business.Implementations.Organizational.Location;
using Business.Implementations.Organizational.Structure;
using Business.Implementations.Organizational.Structure;
using Business.Implementations.Parameters;
using Business.Implementations.Security;
using Business.Implementations.Security;
using Business.Interfaces.ApiColombia;
using Business.Interfaces.Auth;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Operational;
using Business.Interfaces.Organizational.Assignment;
using Business.Interfaces.Organizational.Assignment;
using Business.Interfaces.Organizational.Structure;
using Business.Interfaces.Organizational.Structure;
using Business.Interfaces.Parameters;
using Business.Interfaces.Security;
using Business.Interfases;
using Business.Interfases.Organizational.Location;
using Business.Services.ApiColombia;
using Business.Services.Auth;
using Business.Services.Excel;
using Business.Services.JWT;
using Data.Classes.Base;
using Data.Classes.Specifics;
using Data.Implementations.Auth;
using Data.Implementations.Auth;
using Data.Implementations.Notifications;
using Data.Implementations.Notifications;
using Data.Implementations.Operational;
using Data.Implementations.Operational;
using Data.Implementations.Organizational.Assignment;
using Data.Implementations.Organizational.Assignment;
using Data.Implementations.Organizational.Location;
using Data.Implementations.Organizational.Structure;
using Data.Implementations.Organizational.Structure;
using Data.Implementations.Parameters;
using Data.Implementations.Security;
using Data.Implementations.Security;
using Data.Interfaces.Security;
using Data.Interfaces.Security;
using Data.Interfases;
using Data.Interfases.Auth;
using Data.Interfases.Auth;
using Data.Interfases.Notifications;
using Data.Interfases.Notifications;
using Data.Interfases.Operational;
using Data.Interfases.Operational;
using Data.Interfases.Organizational.Assignment;
using Data.Interfases.Organizational.Assignment;
using Data.Interfases.Organizational.Location;
using Data.Interfases.Organizational.Structure;
using Data.Interfases.Organizational.Structure;
using Data.Interfases.Parameters;
using Data.Interfases.Security;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Operational;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.DTOs.Parameter;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Entity.DTOs.Parameter.Response;
using Entity.Models;
using Entity.Models.Auth;
using Entity.Models.Auth;
using Entity.Models.ModelSecurity;
using Entity.Models.Notifications;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Notifications.Implementations;

namespace Web.Extensions
{
    public static class ServiceExtensionsScoped
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            //User 
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IUserBusiness, UserBusiness>();

            //Person 
            services.AddScoped<PersonData>();
            services.AddScoped<IPersonData, PersonData>();
            services.AddScoped<IPersonBusiness, PersonBusiness>();

            //Rol 
            //services.AddScoped<ICrudBase<Role>, RoleData>();
            //services.AddScoped<RoleBusiness>();

            //Form 
            services.AddScoped<ICrudBase<Form>, FormData>();
            services.AddScoped<FormBusiness>();

            //Module
            services.AddScoped<ICrudBase<Module>, ModuleData>();
            services.AddScoped<ModuleBusiness>();


            //Permission 
            services.AddScoped<ICrudBase<Permission>, PermissionData>();
            services.AddScoped<PermissionBusiness>();

            //RolFormPermission 
            services.AddScoped<IRolFormPermissionData, RolFormPermissionData>();
            services.AddScoped<IRolFormPermissionBusiness, RolFormPermissionBusiness>();


            //UserRol 
            services.AddScoped<IUserRoleData, UserRolesData>();
            services.AddScoped<IUserRoleBusiness, UserRoleBusiness>();


            //Menu
            services.AddScoped<IMenuStructureData, MenuStructureData>();
            services.AddScoped<IMenuStructureBusiness, MenuStructureBusiness>();


            //CustomType 
            services.AddScoped<CustomTypeData>();
            services.AddScoped<ICrudBase<CustomType>, CustomTypeData>();

            services.AddScoped<ICustomTypeData, CustomTypeData>();
            services.AddScoped<ICustomTypeBusiness, CustomTypeBusiness>();

            //City 
            services.AddScoped<ICityData, CityData>();
            services.AddScoped<ICityBusiness, CityBusiness>();

            services.AddScoped(typeof(ICrudBase<>), typeof(BaseData<>));
            services.AddScoped(typeof(IBaseBusiness<,,>), typeof(BaseBusiness<,,>));


            // Service Api Colombia
            services.AddHttpClient<IColombiaApiService, ApiColombiaService>();


            //OPERATIONAL
            //Event 
            services.AddScoped<IEventData, EventData>();
            services.AddScoped<IEventBusiness, EventBusiness>();

            //EventType 
            services.AddScoped<IEventTypeData, EventTypeData>();
            services.AddScoped<IEventTypeBusiness, EventTypeBusiness>();

            //AccessPoint 
            services.AddScoped<IAccessPointData, AccessPointData>();
            services.AddScoped<IAccessPointBusiness, AccessPointBusiness>();

            // Attendance
            services.AddScoped<IAttendanceData, AttendanceData>();
            services.AddScoped<IAttendanceBusiness, AttendanceBusiness>();



            // Event-target
            services.AddScoped<IEventTargetAudienceData, EventTargetAudienceData>();
            services.AddScoped<IEventTargetAudienceBusiness, EventTargetAudienceBusiness>();



            //Auth 
            services.AddScoped<UserService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IRefreshTokenData, RefreshTokenData>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddScoped<ICodeGenerator, FiveDigitCodeGenerator>();
            services.AddScoped<ICodeHasher, HmacCodeHasher>();
            services.AddSingleton<IClock, SystemClock>();
            services.AddScoped<UserVerificationService, UserVerificationService>();



            //Notificatios 
            services.AddScoped<IMessageSender, EmailMessageSender>();
            services.AddScoped<IMessageSender, WhatsAppMessageSender>();

            services.AddScoped<INotify, Notifier>();
            services.AddScoped<IMessageSender, TelegramMessageSender>();

            services.AddScoped<INotificationData, NotificationData>();
            services.AddScoped<INotificationBusiness, NotificationsBusiness>();

            //InternaDivision
            services.AddScoped<IInternalDivisionData ,InternalDivisionData>();
            services.AddScoped<IInternalDivisionBusiness, InternalDivisionBusiness>();

            services.AddScoped<OrganizationalUnitBusiness>();
            services.AddScoped<IBaseBusiness<OrganizationalUnit, OrganizationalUnitDtoRequest, OrganizationalUnitDto>,
            OrganizationalUnitBusiness>();

            services.AddScoped<IOrganizationData, OrganizationData>();
            services.AddScoped<IOrganizationBusiness, OrganizationBusiness>();

            //OrganizationUnit
            services.AddScoped<IOrganizationnalUnitData, OrganizationnalUnitData>();
            services.AddScoped<IOrganizationUnitBusiness, OrganizationalUnitBusiness>();

            //OrganizationalUnitBranch
            services.AddScoped<IOrganizationalUnitBranchData, OrganizationalUnitBranchData>();
            services.AddScoped<IOrganizationalUnitBranchBusiness, OrganizationalUnitBranchBusiness>(); 

            //Buscar La cantidad de branch que tienen una sola organizacion
            services.AddScoped<OrganizationalUnitBranchData>();

            //Card
            services.AddScoped<ICardData, CardData>();
            services.AddScoped<ICardBusiness, CardBusiness>();

            //Area categoria
            services.AddScoped<IAreaCategoryData, AreaCategoryData>();
            services.AddScoped<ICategoryAreaBusiness, AreaCategoryBusiness>();

            //Schedule
            services.AddScoped<IScheduleData, ScheduleData>();
            services.AddScoped<IScheduleBusiness, ScheduleBusiness>();

            //PersonDivisionProfile
            services.AddScoped<IPersonDivisionProfileData, PersonDivisionProfileData>();
            services.AddScoped<IPersonDivisionProfileBusiness, PersonDivisionProfileBusiness>();

            //Profiles
            services.AddScoped<IProfileData, ProfileData>();
            services.AddScoped<IProfileBusiness, ProfileBusiness>();

            //Branch
            services.AddScoped<IBranchData, BranchData>();
            services.AddScoped<IBranchBusiness, BranchBusiness>();

            services.AddScoped<IExcelPersonParser, ExcelPersonParser>();

            services.AddScoped<IUserVerificationService, UserVerificationService>();



            return services;
        }
    }
}

