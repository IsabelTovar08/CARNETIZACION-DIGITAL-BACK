using Business.Classes;
using Business.Classes.Base;
using Business.Interfases;
using Business.Services.Auth;
using Business.Services.JWT;
using Data.Classes.Specifics;
using Data.Interfases;
using Entity.DTOs;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Entity.Models.Notifications;
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
            services.AddScoped<ICrudBase<Role>, RoleData>();
            services.AddScoped<RoleBusiness>();

            //Form 
            services.AddScoped<ICrudBase<Form>, FormData>();
            services.AddScoped<FormBusiness>();

            //Module
            services.AddScoped<ICrudBase<Module>, ModuleData>();
            services.AddScoped<ModuleBusiness>();

            //ModuleForm 
            services.AddScoped<ICrudBase<ModuleForm>, ModuleFormData>();
            services.AddScoped<ModuleFormBusiness>();

            //Permission 
            services.AddScoped<ICrudBase<Permission>, PermissionData>();
            services.AddScoped<PermissionBusiness>();

            //RolFormPermission 
            services.AddScoped<ICrudBase<RolFormPermission>, RolFormPermissionData>();
            services.AddScoped<RolFormPermissionBusiness>();

            //UserRol 
            services.AddScoped<UserRoleData>();
            services.AddScoped<ICrudBase<UserRoles>, UserRoleData>();

            services.AddScoped<UserBusiness>();
            services.AddScoped<UserRoleBusiness>();


            //services.AddScoped(typeof(IBaseBusiness<,,>), typeof(BaseBusiness<,,>));

            //services.AddScoped<IBaseBusiness<Rol, RolDto, RolCreate>, RolBusiness>();

            services.AddScoped<IBaseBusiness<UserRoles, UserRolDto>, UserRoleBusiness>();
            services.AddScoped<IBaseBusiness<User, UserDTO>, UserBusiness>();
            services.AddScoped<IBaseBusiness<Person, PersonDto>, PersonBusiness>();
            services.AddScoped<IBaseBusiness<Form, FormDto>, FormBusiness>();
            services.AddScoped<IBaseBusiness<Module, ModuleDto>, ModuleBusiness>();
            services.AddScoped<IBaseBusiness<ModuleForm, ModuleFormDto>, ModuleFormBusiness>();
            services.AddScoped<IBaseBusiness<Permission, PermissionDto>, PermissionBusiness>();
            services.AddScoped<IBaseBusiness<RolFormPermission, RolFormPermissionDto>, RolFormPermissionBusiness>();

            //Auth 
            services.AddScoped<UserService>();

            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();



            //Notificatios 
            services.AddScoped<IMessageSender, EmailMessageSender>();
            services.AddScoped<IMessageSender, WhatsAppMessageSender>();

            services.AddScoped<INotify, Notifier>();
            services.AddScoped<IMessageSender, TelegramMessageSender>();


            return services;
        }
    }
}

