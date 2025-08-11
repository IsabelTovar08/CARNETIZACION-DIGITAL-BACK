using Business.Classes;
using Business.Classes.Base;
using Business.Implementations.Organization;
using Business.Implementations.Parameters;
using Business.Interfaces;
using Business.Interfases;
using Business.Services.Auth;
using Business.Services.JWT;
using Data.Classes.Base;
using Data.Classes.Specifics;
using Data.Implementations.Organization;
using Data.Implementations.Parameters;
using Data.Interfases;
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

namespace Web.Extensions
{
    public static class ServiceExtensionsScoped
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            //User 
            services.AddScoped<UserData>();
            services.AddScoped<ICrudBase<User>, UserData>();
            services.AddScoped<UserBusiness>();

            //Person 
            services.AddScoped<PersonData>();

            services.AddScoped<ICrudBase<Person>, PersonData>();
            services.AddScoped<PersonBusiness>();

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
            services.AddScoped<ICrudBase<RolFormPermission>, RolFormPermissionData>();
            services.AddScoped<RolFormPermissionBusiness>();

            services.AddScoped<IRolFormPermissionData, RolFormPermissionData>();
            services.AddScoped<IRolFormPermissionBusiness, RolFormPermissionBusiness>();


            //UserRol 
            services.AddScoped<UserRoleData>();
            services.AddScoped<ICrudBase<UserRoles>, UserRoleData>();

            //User 
            services.AddScoped<UserBusiness>();
            services.AddScoped<UserRoleBusiness>();

            //CustomType 
            services.AddScoped<CustomTypeData>();
            services.AddScoped<ICrudBase<CustomType>, CustomTypeData>();

            services.AddScoped(typeof(ICrudBase<>), typeof(BaseData<>));
            services.AddScoped(typeof(IBaseBusiness<,,>), typeof(BaseBusiness<,,>));


            services.AddScoped<IBaseBusiness<UserRoles, UserRoleDtoRequest, UserRolDto>, UserRoleBusiness>();
            services.AddScoped<IBaseBusiness<User, UserDtoRequest, UserDTO>, UserBusiness>();
            services.AddScoped<IBaseBusiness<Person, PersonDtoRequest , PersonDto>, PersonBusiness>();
            services.AddScoped<IBaseBusiness<Form, FormDtoRequest, FormDto>, FormBusiness>();
            services.AddScoped<IBaseBusiness<Module, ModuleDtoRequest, ModuleDto>, ModuleBusiness>();
            services.AddScoped<IBaseBusiness<Permission, PermissionDtoRequest, PermissionDto>, PermissionBusiness>();
            services.AddScoped<IBaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>, RolFormPermissionBusiness>();

            services.AddScoped<IBaseBusiness<CustomType, CustomTypeDto, CustomTypeDto>, CustomTypeBusiness>();


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

