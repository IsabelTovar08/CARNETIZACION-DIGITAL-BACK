using Business.Classes;
using Business.Classes.Base;
using Business.Implementations.Notifications;
using Business.Implementations.Operational;
using Business.Implementations.Organizational.Assignment;
using Business.Implementations.Organizational.Location;
using Business.Implementations.Organizational.Structure;
using Business.Implementations.Parameters;
using Business.Implementations.Storage;
using Business.Interfaces.ApiColombia;
using Business.Interfaces.Auth;
using Business.Interfaces.Enums;
using Business.Interfaces.Logging;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Organizational.Assignment;
using Business.Interfaces.Organizational.Structure;
using Business.Interfaces.Parameters;
using Business.Interfaces.Security;
using Business.Interfaces.Services;
using Business.Interfaces.Storage;
using Business.Interfases;
using Business.Interfases.Organizational.Location;
using Business.Interfases.Storage;
using Business.Services.ApiColombia;
using Business.Services.Auth;
using Business.Services.Cards;
using Business.Services.CodeGenerator;
using Business.Services.Enums;
using Business.Services.Excel;
using Business.Services.Export;
using Business.Services.JWT;
using Business.Services.Logging;
using Business.Services.Storage;
using Data.Classes.Base;
using Data.Classes.Specifics;
using Data.Implementations.Auth;
using Data.Implementations.Logging;
using Data.Implementations.Notifications;
using Data.Implementations.Operational;
using Data.Implementations.Organizational.Assignment;
using Data.Implementations.Organizational.Location;
using Data.Implementations.Organizational.Structure;
using Data.Implementations.Parameters;
using Data.Implementations.Transaction;
using Data.Interfases;
using Data.Interfases.Auth;
using Data.Interfases.Logging;
using Data.Interfases.Notifications;
using Data.Interfases.Operational;
using Data.Interfases.Organizational.Assignment;
using Data.Interfases.Organizational.Location;
using Data.Interfases.Organizational.Structure;
using Data.Interfases.Parameters;
using Data.Interfases.Security;
using Data.Interfases.Transaction;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client.Extensions.Msal;
using Utilities.Helpers;
using Utilities.Helpers.Excel;
using Utilities.Notifications.Implementations;
using Web.Auth;
using Web.Realtime.Dispatchers;

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
            services.AddScoped<IRoleData, RoleData>();
            services.AddScoped<IRoleBusiness ,RoleBusiness>();

            //Form 
            services.AddScoped<ICrudBase<Form>, FormData>();
            services.AddScoped<FormBusiness>();

            //Module
            services.AddScoped<IModuleData, ModuleData>();
            services.AddScoped<IModuleBusiness, ModuleBusiness>();

            services.AddScoped<IMenuService, MenuService>();


            //Permission 
            services.AddScoped<ICrudBase<Permission>, PermissionData>();
            services.AddScoped<PermissionBusiness>();

            //RolFormPermission 
            services.AddScoped<IRolFormPermissionData, RolFormPermissionData>();
            services.AddScoped<IRolFormPermissionBusiness, RolFormPermissionBusiness>();


            //UserRol 
            services.AddScoped<IUserRoleData, UserRolesData>();
            services.AddScoped<IUserRoleBusiness, UserRoleBusiness>();


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

            //EventAccessPoint
            services.AddScoped<IEventAccessPointData, EventAccessPointData>();
            services.AddScoped<IEventAccessPointBusiness, EventAccessPointBusiness>();


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
            services.AddScoped<INotificationDispatcher, SignalRNotificationDispatcher>();

            services.AddScoped<INotificationData, NotificationData>();
            services.AddScoped<INotificationBusiness, NotificationsBusiness>();

            services.AddScoped<INotificationsReceivedData, NotificationsReceivedData>();
            services.AddScoped<INotificationReceivedBusiness, NotificationReceivedBusiness>();

            services.AddScoped<IModificationRequestData, ModificationRequestData>();
            services.AddScoped<IModificationRequestBusiness, ModificationRequestBusiness>();


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
            services.AddScoped<ICardConfigurationData, CardConfigurationData>();
            services.AddScoped<ICardConfigurationBusiness, CardConfigurationBusiness>();

            //Card Templates
            services.AddScoped<ICardTemplateData, CardTemplateData>();
            services.AddScoped<ICardTemplateBusiness, CardTemplateBusiness>();

            //Area categoria
            services.AddScoped<IAreaCategoryData, AreaCategoryData>();
            services.AddScoped<ICategoryAreaBusiness, AreaCategoryBusiness>();

            //Schedule
            services.AddScoped<IScheduleData, ScheduleData>();
            services.AddScoped<IScheduleBusiness, ScheduleBusiness>();

            //PersonDivisionProfile
            services.AddScoped<IIssuedCardData, IssuedCardData>();
            services.AddScoped<IIssuedCardBusiness, IssuedCardBusiness>();

            //Profiles
            services.AddScoped<IProfileData, ProfileData>();
            services.AddScoped<IProfileBusiness, ProfileBusiness>();

            //Branch
            services.AddScoped<IBranchData, BranchData>();
            services.AddScoped<IBranchBusiness, BranchBusiness>();

            services.AddScoped<IExcelPersonParser, ExcelPersonParser>();
            services.AddScoped<IExcelBulkImporter, ExcelBulkImporter>();


            services.AddScoped<IUserVerificationService, UserVerificationService>();


            //EventSchedule
            services.AddScoped<IEventScheduleData, EventScheduleData>();
            services.AddScoped<IEventScheduleBusiness, EventScheduleBusiness>();


            //Enums
            services.AddScoped<IEnumCatalogService, EnumCatalogService>();

            services.AddScoped<IFileStorageService, SupabaseStorageService>();

            services.AddScoped(typeof(ICodeGeneratorService<>), typeof(CodeGeneratorService<>));

            // storage route images
            services.AddScoped<IAssetUploader, AssetUploader>();
            services.AddScoped<IExcelReaderHelper, ClosedXmlExcelReaderHelper>();


            // Data
            services.AddScoped<IImportBatchData, ImportBatchData>();
            services.AddScoped<IImportBatchRowData, ImportBatchRowData>();

            // Business
            services.AddScoped<IImportHistoryBusiness, ImportHistoryBusiness>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDeviceInfoService, DeviceInfoService>();


            // PDF Y EXCEL

            services.AddScoped<IExportService, ExportService>();

            services.AddHttpClient<ICardPdfService, CardPdfService>();
            services.AddHttpClient<ICardPdfService, CardPdfService>()
            .ConfigureHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(20);
                client.DefaultRequestHeaders.Add("User-Agent", "CardPdfService");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = true,
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            });

            services.AddScoped<IAttendanceNotifier, AttendanceNotifier>();

            services.AddSignalR();




            return services;
        }
    }
}

