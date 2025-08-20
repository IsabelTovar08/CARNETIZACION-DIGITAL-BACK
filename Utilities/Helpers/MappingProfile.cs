using AutoMapper;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Operational;
using Entity.DTOs.Organizational.Request.Structure;
using Entity.DTOs.Organizational.Response.Location;
using Entity.DTOs.Organizational.Response.Structure;
using Entity.DTOs.Parameter;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Location;
using Entity.Models.Organizational.Structure;
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
            CreateMap<Form, FormDto>()
                .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.Module.Name))
                .ReverseMap();
            CreateMap<Form, FormDtoRequest>().ReverseMap();


            //Mapeo de la entidad Module 
            CreateMap<Module, ModuleDto>().ReverseMap();
            CreateMap<Module, ModuleDtoRequest>().ReverseMap();


            //Mapeo de la entidad permission
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Permission, PermissionDtoRequest>().ReverseMap();


            //Mapeo de la entidad User
            CreateMap<User, UserDTO>()
             .ForMember(dest => dest.NamePerson, opt => opt.MapFrom(src =>
                 (src.Person != null ? (src.Person.FirstName + " " + src.Person.LastName) : string.Empty)))

             .ForMember(dest => dest.EmailPerson, opt => opt.MapFrom(src =>
                 src.Person != null ? src.Person.Email : string.Empty))

             .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(R => R.Rol)))
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
            CreateMap<RolFormPermission, RolFormPermissionDtoRequest>().ReverseMap();

            //Menu
            CreateMap<MenuStructure, MenuStructureDto>()
            .ForMember(d => d.Title,
                o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.Title) ? s.Title : (s.FormId != null ? s.Form.Name : s.Module.Name)))
            .ForMember(d => d.Url, o => o.MapFrom(s => s.Form != null ? s.Form.Url : null))
            .ForMember(d => d.Icon, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.Icon) ? s.Icon : (s.FormId != null ? s.Form.Icon : s.Module.Icon)))
            .ForMember(d => d.Classes, o => o.MapFrom(s => s.Type == "item" ? "nav-item" : null))
            .ForMember(d => d.Children, o => o.MapFrom(s => s.Children.OrderBy(c => c.OrderIndex)));

            CreateMap<MenuStructure, MenuStructureRequest>().ReverseMap();


            //Parameter
            CreateMap<Status, StatusDto>()
             .ReverseMap();

            CreateMap<CustomType, CustomTypeDto>()
             .ForMember(dest => dest.TypeCategoryName, opt => opt.MapFrom(src => src.TypeCategory.Name))
             .ReverseMap();

            CreateMap<CustomType, CustomTypeSpecific>()
             .ReverseMap();
            CreateMap<CustomType, CustomTypeRequest>()
            .ReverseMap();

            CreateMap<TypeCategory, TypeCategoryDto>()
             .ReverseMap();

            //Organizational

            //City
            CreateMap<City, CityDto>()
             .ForMember(dest => dest.DeparmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ReverseMap();

            //Deparments
            CreateMap<Department, DepartmentDto>()
            .ReverseMap();

            //Mapeo de la entidad de organization Unit con sus divisiones
            CreateMap<OrganizationalUnit, OrganizationalUnitDto>()
                .ForMember(d => d.DivisionsCount,
                    m => m.MapFrom(s => s.InternalDivissions.Count))
                //Mapeo de la entidad de organization Unit con sus branchs
                .ForMember(d => d.BranchesCount,
                    m => m.MapFrom(s => s.OrganizationalUnitBranches.Count));
            CreateMap<OrganizationalUnitDtoRequest, OrganizationalUnit>();




            //OPERATIONAL
            //Event
            CreateMap<Event, EventDto>()
           .ReverseMap();

            //EventType
            CreateMap<EventType, EventTypeDto>()
           .ReverseMap();

            //AccessPoint
            CreateMap<AccessPoint, AccessPointDto>()
           .ReverseMap();
        }
    }
}
