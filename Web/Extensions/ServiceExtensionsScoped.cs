using Business.Classes;
using Business.Classes.Base;
using Business.Implementations.Organizational.Location;
using Business.Implementations.Organization;
using Business.Implementations.Parameters;
using Business.Interfaces.ApiColombia;
using Business.Interfaces.Parameters;
using Business.Interfaces.Security;
using Business.Interfases;
using Business.Interfases.Organizational.Location;
using Business.Services.ApiColombia;
using Business.Services.Auth;
using Business.Services.JWT;
using Data.Classes.Base;
using Data.Classes.Specifics;
using Data.Implementations.Organization;
using Data.Implementations.Organizational.Location;
using Data.Implementations.Parameters;
using Data.Interfases;
using Data.Interfases.Organizational.Location;
using Data.Interfases.Parameters;
using Data.Interfases.Security;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Request.Structure;
using Entity.DTOs.Organizational.Response.Structure;
using Entity.DTOs.Parameter;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Entity.Models.Notifications;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using Infrastructure.Notifications.Interfases;
using Utilities.Notifications.Implementations;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Data.Implementations.Security;
using Business.Implementations.Security;
using Data.Interfaces.Security;

namespace Web.Extensions
{
    public static class ServiceExtensionsScoped
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            //User 
            services.AddScoped<UserData>();
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
            services.AddScoped<MenuStructureData>();

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


            services.AddScoped<IBaseBusiness<UserRoles, UserRoleDtoRequest, UserRolDto>, UserRoleBusiness>();
            services.AddScoped<IBaseBusiness<User, UserDtoRequest, UserDTO>, UserBusiness>();
            services.AddScoped<IBaseBusiness<Person, PersonDtoRequest , PersonDto>, PersonBusiness>();
            services.AddScoped<IBaseBusiness<Form, FormDtoRequest, FormDto>, FormBusiness>();
            services.AddScoped<IBaseBusiness<Module, ModuleDtoRequest, ModuleDto>, ModuleBusiness>();
            services.AddScoped<IBaseBusiness<Permission, PermissionDtoRequest, PermissionDto>, PermissionBusiness>();
            services.AddScoped<IBaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>, RolFormPermissionBusiness>();

            services.AddScoped<IBaseBusiness<CustomType, CustomTypeRequest, CustomTypeDto>, CustomTypeBusiness>();

            // Service Api Colombia
            services.AddHttpClient<IColombiaApiService, ApiColombiaService>();



            //Auth 
            services.AddScoped<UserService>();

            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();



            //Notificatios 
            services.AddScoped<IMessageSender, EmailMessageSender>();
            services.AddScoped<IMessageSender, WhatsAppMessageSender>();

            services.AddScoped<INotify, Notifier>();
            services.AddScoped<IMessageSender, TelegramMessageSender>();

            //InternaDivision
            services.AddScoped<InternalDivisionData>();
            services.AddScoped<OrganizationalUnitBusiness>();
            services.AddScoped<IBaseBusiness<OrganizationalUnit, OrganizationalUnitDtoRequest, OrganizationalUnitDto>,
            OrganizationalUnitBusiness>();


            //Buscar La cantidad de branch que tienen una sola organizacion
            services.AddScoped<OrganizationalUnitBranchData>();
            
            
            
            return services;
        }
    }
}

