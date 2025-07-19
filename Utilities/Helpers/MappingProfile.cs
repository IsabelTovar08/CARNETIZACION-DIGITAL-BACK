using AutoMapper;
using Entity.DTOs;
using Entity.DTOs.Create;
using Entity.Models;
using Entity.Models.Organization;

namespace Utilities.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Mapeo de la entidad Person 
            CreateMap<Person, PersonDto>().ReverseMap();

            //Mapeo de la entidad Rol 
            CreateMap<Role, RolDto>().ReverseMap();

            //Mapeo de la entidad Form 
            CreateMap<Form, FormDto>().ReverseMap();

            //Mapeo de la entidad Module 
            CreateMap<Module, ModuleDto>().ReverseMap();

            //Mapeo de la entidad ModuleForm 
            CreateMap<ModuleForm, ModuleFormDto>()
             .ForMember(dest => dest.NameForm, opt => opt.MapFrom(src => src.Form.Name))
             .ForMember(dest => dest.NameModule, opt => opt.MapFrom(src => src.Module.Name))
             .ReverseMap();

            //Mapeo de la entidad permission
            CreateMap<Permission, PermissionDto>().ReverseMap();

            //Mapeo de la entidad User
            CreateMap<User, UserDTO>()
             .ForMember(dest => dest.NamePerson, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
            .ReverseMap();

            //Mapeo de la entidad UserROl
            CreateMap<UserRoles, UserRolDto>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Email))
             .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name))
             .ReverseMap();

            //Mapeo de la entidad RolFormPermission
            CreateMap<RolFormPermission, RolFormPermissionDto>()
             .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name))
             .ForMember(dest => dest.RolDescription, opt => opt.MapFrom(src => src.Rol.Description))
             .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form.Name))
             .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name))
             .ReverseMap();
            CreateMap<RolFormPermission, RolFormPermissionCreateDto>().ReverseMap();
        }
    }
}
