using AutoMapper;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Parameter;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Entity.Models.Parameter;
namespace Utilities.Helper
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            //Mapeo de la entidad Person 
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType.Name))
                .ForMember(dest => dest.BloodTypeName, opt => opt.MapFrom(src => src.BloodType.Name))
                .ReverseMap();
            CreateMap<Person, PersonDtoRequest>().ReverseMap();


            //Mapeo de la entidad Rol 
            CreateMap<Role, RolDto>().ReverseMap();
            CreateMap<Role, RoleDtoRequest>().ReverseMap();


            //Mapeo de la entidad Form 
            CreateMap<Form, FormDto>().ReverseMap();
            CreateMap<Form, FormDtoRequest>().ReverseMap();


            //Mapeo de la entidad Module 
            CreateMap<Module, ModuleDto>().ReverseMap();
            CreateMap<Module, ModuleDtoRequest>().ReverseMap();


            //Mapeo de la entidad permission
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Permission, PermissionDtoRequest>().ReverseMap();


            //Mapeo de la entidad User
            CreateMap<User, UserDTO>()
             .ForMember(dest => dest.NamePerson, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
             .ForMember(dest => dest.EmailPerson, opt => opt.MapFrom(src => src.Person.Email + " " + src.Person.Email))
             .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Rol.Name).ToList()))
            .ReverseMap();
            CreateMap<User, UserDtoRequest>().ReverseMap();


            //Mapeo de la entidad UserROl
            CreateMap<UserRoles, UserRolDto>()
             .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name))
             .ReverseMap();
            CreateMap<UserRoles, UserRoleDtoRequest>().ReverseMap();


            //Mapeo de la entidad RolFormPermission
            CreateMap<RolFormPermission, RolFormPermissionDto>()
             .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name))
             .ForMember(dest => dest.RolDescription, opt => opt.MapFrom(src => src.Rol.Description))
             .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form.Name))
             .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name))
             .ReverseMap();


            //Parameter
            CreateMap<Status, StatusDto>()
             .ReverseMap();

            CreateMap<CustomType, CustomTypeDto>()
             .ForMember(dest => dest.TypeCategoryName, opt => opt.MapFrom(src => src.TypeCategory.Name))
             .ReverseMap();

            CreateMap<TypeCategory, TypeCategoryDto>()
             .ReverseMap();
        }
    }
}
