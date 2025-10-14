using System.Globalization; // ➕ para formateo es-CO
using AutoMapper;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Notifications;
using Entity.DTOs.Notifications.Request;
using Entity.DTOs.Notifications.Response;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Organizational.Location.Response;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.DTOs.Parameter;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Entity.DTOs.Specifics;
using Entity.Enums.Extensions;
using Entity.Enums.Specifics;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Entity.Models.Notifications;
using Entity.Models.Operational;
using Entity.Models.Operational.BulkLoading;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Organizational.Location;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using Utilities.Enums.Specifics;

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


            CreateMap<Person, PersonInfoDto>()
            // Info básica de la persona
            .ForMember(d => d.PersonalInfo, o => o.MapFrom(s => s))

            // División actual (IsCurrentlySelected)
            .ForMember(d => d.DivissionId, o => o.MapFrom(s =>
                s.PersonDivisionProfile.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.Id))
            .ForMember(d => d.DivissionName, o => o.MapFrom(s =>
                s.PersonDivisionProfile.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.Name))

            // Unidad
            .ForMember(d => d.UnitId, o => o.MapFrom(s =>
                s.PersonDivisionProfile.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.OrganizationalUnit.Id))
            .ForMember(d => d.UnitName, o => o.MapFrom(s =>
                s.PersonDivisionProfile.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.OrganizationalUnit.Name))

            // Organización (desde Branch → Organization)
            .ForMember(d => d.OrganizationId, o => o.MapFrom(s =>
                s.PersonDivisionProfile.FirstOrDefault(p => p.IsCurrentlySelected)
                    .InternalDivision.OrganizationalUnit.OrganizationalUnitBranches
                    .Select(oub => oub.Branch.Organization.Id)
                    .FirstOrDefault()))
            .ForMember(d => d.OrganizationName, o => o.MapFrom(s =>
                s.PersonDivisionProfile.FirstOrDefault(p => p.IsCurrentlySelected)
                    .InternalDivision.OrganizationalUnit.OrganizationalUnitBranches
                    .Select(oub => oub.Branch.Organization.Name)
                    .FirstOrDefault()));




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
             .ForMember(dest => dest.NamePerson, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
             .ForMember(dest => dest.EmailPerson, opt => opt.MapFrom(src => src.Person.Email))
             .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(R => R.Rol)))
             .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
             .ReverseMap();

            CreateMap<User, UserDtoRequest>().ReverseMap();


            CreateMap<User, UserMeDto>()
            // ⬇️ AQUÍ el cambio clave: pasa Rol ENTIDAD, no Name string
            .ForMember(d => d.PhotoUrl, opt => opt.MapFrom(s => s.Person.PhotoUrl))
            .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRoles.Select(ur => ur.Rol)))
            .ForMember(d => d.Permissions, opt => opt.MapFrom(s =>
        s.UserRoles
         .SelectMany(ur => ur.Rol.RolFormPermissions.Select(rp => rp.Permission))
         .DistinctBy(p => p.Id)))
            .ForMember(d => d.CurrentProfile, opt => opt.MapFrom(s =>
                s.Person.PersonDivisionProfile.FirstOrDefault(p => p.IsCurrentlySelected)));

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
            CreateMap<Status, StatusDtoRequest>().ReverseMap();
            CreateMap<Status, StatusDtoResponse>().ReverseMap();

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
            CreateMap<City, CityDtoResponse>()
             .ForMember(dest => dest.DeparmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ReverseMap();

            //Deparments
            CreateMap<Department, DepartmentDtoResponse>()
            .ReverseMap();

            //Mapeo de la entidad de organization Unit con sus divisiones
            CreateMap<OrganizationalUnit, OrganizationalUnitDto>()
                .ForMember(d => d.DivisionsCount,
                    m => m.MapFrom(s => s.InternalDivissions.Count))
                .ForMember(d => d.BranchesCount,
                    m => m.MapFrom(s => s.OrganizationalUnitBranches.Count));
            CreateMap<OrganizationalUnitDtoRequest, OrganizationalUnit>();

            //OPERATIONAL

            //Cards
            CreateMap<Card, CardDto>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.Name))
            .ForMember(d => d.PersonId, o => o.MapFrom(s => s.PersonDivisionProfile.Person.Id))
            .ForMember(d => d.PersonFullName, o => o.MapFrom(s => s.PersonDivisionProfile.Person.FirstName + " " + s.PersonDivisionProfile.Person.LastName))
            .ForMember(d => d.DivisionId, o => o.MapFrom(s => s.PersonDivisionProfile.InternalDivision.Id))
            .ForMember(d => d.DivisionName, o => o.MapFrom(s => s.PersonDivisionProfile.InternalDivision.Name))
            .ForMember(d => d.ProfileId, o => o.MapFrom(s => s.PersonDivisionProfile.Profile.Id))
            .ForMember(d => d.ProfileName, o => o.MapFrom(s => s.PersonDivisionProfile.Profile.Name))
            .ForMember(d => d.AreaCategoryName, o => o.MapFrom(s => s.PersonDivisionProfile.InternalDivision.AreaCategory.Name))

            .ReverseMap()
                .ForMember(s => s.Status, o => o.Ignore())
                .ForMember(s => s.PersonDivisionProfile, o => o.Ignore());

            CreateMap<Card, CardDtoRequest>()
                .ReverseMap()
                    .ForMember(s => s.Id, o => o.Ignore())
                    .ForMember(s => s.IsDeleted, o => o.Ignore())
                    .ForMember(s => s.Status, o => o.Ignore())
                    .ForMember(s => s.PersonDivisionProfile, o => o.Ignore());

            CreateMap<CardTemplate, CardTemplateRequest>().ReverseMap();
            CreateMap<CardTemplate, CardTemplateResponse>().ReverseMap();

            //PersonDivisionProfile
            CreateMap<PersonDivisionProfile, PersonDivisionProfileDto>()
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
                .ForMember(dest => dest.DivisionName, opt => opt.MapFrom(src => src.InternalDivision.Name))
                .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.Profile.Name))
                .ReverseMap();

            CreateMap<PersonDivisionProfile, PersonDivisionProfileDtoRequest>()
                .ReverseMap();

            //Profiles
            CreateMap<Profiles, ProfileDto>()
                .ReverseMap();

            CreateMap<Profiles, ProfileDtoRequest>()
                .ReverseMap();

            // Branch
            CreateMap<Branch, BranchDto>()
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.City.Name))
                .ForMember(d => d.OrganizationName, o => o.MapFrom(s => s.Organization.Name))
                .ReverseMap()
                    .ForMember(s => s.City, o => o.Ignore())
                    .ForMember(s => s.Organization, o => o.Ignore());

            CreateMap<Branch, BranchDtoRequest>()
                .ReverseMap()
                    .ForMember(s => s.Id, o => o.Ignore())
                    .ForMember(s => s.IsDeleted, o => o.Ignore())
                    .ForMember(s => s.City, o => o.Ignore())
                    .ForMember(s => s.Organization, o => o.Ignore());

            //Structure

            //Organization 
            CreateMap<Organization, OrganizationDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.OrganizaionType.Name))
                .ReverseMap();
            CreateMap<Organization, OrganizationDtoRequest>();

            //Area Categoria
            CreateMap<AreaCategory, AreaCategoryDto>()
                .ReverseMap();

            CreateMap<AreaCategory, AreaCategoryDtoRequest>()
                .ReverseMap();

            //Event
            CreateMap<Event, EventDtoResponse>()
             .ForMember(d => d.AccessPoints, opt => opt.MapFrom(s =>
                 s.EventAccessPoints.Select(eap => new AccessPointDtoResponsee
                 {
                     Id = eap.AccessPoint.Id,
                     Name = eap.AccessPoint.Name,
                     Description = eap.AccessPoint.Description,
                     TypeId = eap.AccessPoint.TypeId,
                     Type = eap.AccessPoint.AccessPointType != null ? eap.AccessPoint.AccessPointType.Name : null
                 })
             ));

            CreateMap<Event, EventDetailsDtoResponse>()
             .ForMember(d => d.AccessPoints, opt => opt.MapFrom(s =>
                 s.EventAccessPoints.Select(eap => new AccessPointDtoResponsee
                 {
                     Id = eap.AccessPoint.Id,
                     Name = eap.AccessPoint.Name,
                     Description = eap.AccessPoint.Description,
                     TypeId = eap.AccessPoint.TypeId,
                     Type = eap.AccessPoint.AccessPointType != null ? eap.AccessPoint.AccessPointType.Name : null
                 })
             ))
             .ForMember(d => d.Audiences, opt => opt.MapFrom(s => s.EventTargetAudiences));

            CreateMap<EventDtoRequest, Event>()
            .ForMember(d => d.IsPublic, o => o.MapFrom(s => s.Ispublic))
            .ForMember(d => d.Id, o => o.Ignore());


            //EventType
            CreateMap<EventType, EventTypeDtoRequest>().ReverseMap();
            CreateMap<EventType, EventTypeDtoResponse>().ReverseMap();

            //InternalDivision
            CreateMap<InternalDivision, InternalDivisionDto>()
                .ForMember(d => d.OrganizationalUnitName, o => o.MapFrom(s => s.OrganizationalUnit.Name))
                .ForMember(d => d.AreaCategoryName, o => o.MapFrom(s => s.AreaCategory.Name))
                .ReverseMap();

            CreateMap<InternalDivision, InternalDivisionDtoRequest>()
                .ReverseMap();

            // OrganizationalUnitBranch
            CreateMap<OrganizationalUnitBranch, OrganizationalUnitBranchDto>()
                .ForMember(d => d.OrganizationalUnitName, o => o.MapFrom(s => s.OrganizationUnit.Name))
                .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name))
                .ReverseMap();

            CreateMap<OrganizationalUnitBranch, OrganizationalUnitBranchDtoRequest>()
                .ReverseMap();




            //AccessPoints
            CreateMap<AccessPointDtoRequest, AccessPoint>()
             .ForMember(d => d.Id, o => o.Ignore())
             .ForMember(d => d.AccessPointType, o => o.Ignore());

            CreateMap<AccessPoint, AccessPointDtoRequest>().ReverseMap();

            // ENTIDAD -> DTO
            CreateMap<AccessPoint, AccessPointDtoResponsee>()
             .ForMember(d => d.EventId, opt => opt.MapFrom(s =>
                 s.EventAccessPoints.Select(eap => eap.EventId).FirstOrDefault()))
             .ForMember(d => d.EventName, opt => opt.MapFrom(s =>
                 s.EventAccessPoints.Select(eap => eap.Event.Name).FirstOrDefault()))
             .ForMember(d => d.Type, opt => opt.MapFrom(s => s.AccessPointType != null ? s.AccessPointType.Name : null));


            // DTO -> ENTIDAD
            CreateMap<AccessPointDtoResponsee, AccessPoint>()
                .ForMember(d => d.AccessPointType, opt => opt.Ignore());


            //Schedule
            CreateMap<Schedule, ScheduleDto>()
             .ForMember(d => d.OrganizationName,
                 opt => opt.MapFrom(s => s.Organization != null ? s.Organization.Name : null));

            CreateMap<Schedule, ScheduleDtoRequest>()
                .ReverseMap();

            // EventTargetAudience
            CreateMap<EventTargetAudience, EventTargetAudienceDtoRequest>().ReverseMap();

            CreateMap<EventTargetAudience, EventTargetAudienceDtoResponse>()
             .ForMember(d => d.EventName, opt => opt.MapFrom(s => s.Event.Name))
             .ForMember(d => d.ReferenceName, opt => opt.MapFrom(s =>
                 s.TypeId == 1 && s.Profile != null ? s.Profile.Name :
                 s.TypeId == 2 && s.OrganizationalUnit != null ? s.OrganizationalUnit.Name :
                 s.TypeId == 3 && s.InternalDivision != null ? s.InternalDivision.Name :
                 null
             ));

            CreateMap<Schedule, ScheduleDtoRequest>()
                .ReverseMap();

            //Notifications
            CreateMap<Notification, NotificationDto>()
                .ForMember(d => d.NotificationTypeName,
                    opt => opt.MapFrom(s => ((NotificationType)s.NotificationTypeId).ToString())) 
                .ReverseMap();

            CreateMap<Notification, NotificationDtoRequest>()
                .ReverseMap();

            //NotificationsReceived
            CreateMap<NotificationReceived, NotificatioReceivedDtoResponse>()
                .ReverseMap();

            CreateMap<NotificationReceived, NotificationReceivedDtoRequest>()
                .ReverseMap();

            CreateMap<NotificationReceived, NotificationReceivedDto>()
                .ReverseMap();


            CreateMap<ModificationRequest, ModificationRequestDtoResponse>()
            .ForMember(dest => dest.FieldId, opt => opt.MapFrom(src => (int)src.Field))
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.Field.GetDisplayName()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.GetDisplayName()))
            .ReverseMap();

            // De Create DTO a Entity
            CreateMap<ModificationRequestDtoRequest, ModificationRequest>()
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ModificationRequestStatus.Pending))
                .ReverseMap();

            // Attendance

            CreateMap<Attendance, AttendanceDtoRequest>().ReverseMap();

            CreateMap<Attendance, AttendanceDtoResponse>()
                .ForMember(dest => dest.PersonFullName,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.FirstName + " " + src.Person.LastName : string.Empty))
                .ForMember(dest => dest.AccessPointOfEntryName,
                    opt => opt.MapFrom(src => src.AccessPointEntry != null ? src.AccessPointEntry.Name : null))
                .ForMember(dest => dest.AccessPointOfExitName,
                    opt => opt.MapFrom(src => src.AccessPointExit != null ? src.AccessPointExit.Name : null))
               .ForMember(dest => dest.EventName,
                opt => opt.MapFrom(src =>
                    src.AccessPointEntry != null
                        ? src.AccessPointEntry.EventAccessPoints
                            .Select(eap => eap.Event.Name)
                            .FirstOrDefault()
                        : null
                ))

                // ➕ Formateo de fechas a string (cultura es-CO). Sin helpers externos.
                .ForMember(dest => dest.TimeOfEntryStr,
                    opt => opt.MapFrom(src => src.TimeOfEntry.ToString("dd/MM/yyyy HH:mm", new CultureInfo("es-CO"))))
                .ForMember(dest => dest.TimeOfExitStr,
                    opt => opt.MapFrom(src => src.TimeOfExit.HasValue
                        ? src.TimeOfExit.Value.ToString("dd/MM/yyyy HH:mm", new CultureInfo("es-CO"))
                        : null))

                .ReverseMap()
                    .ForMember(dest => dest.Person, opt => opt.Ignore())
                    .ForMember(dest => dest.AccessPointEntry, opt => opt.Ignore())
                    .ForMember(dest => dest.AccessPointExit, opt => opt.Ignore());

            CreateMap<User, UserProfileDto>()
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Person.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Person.LastName))
                .ForMember(d => d.SecondLastName, opt => opt.MapFrom(s => s.Person.SecondLastName))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Person.Email))
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Person.Phone));

            CreateMap<UserProfileRequestDto, User>()
                .ForPath(dest => dest.Person.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.Person.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.Person.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.Person.SecondLastName, opt => opt.MapFrom(src => src.SecondLastName))
                .ForPath(dest => dest.Person.Phone, opt => opt.MapFrom(src => src.Phone));


            CreateMap<ImportBatchStartDto, ImportBatch>().ReverseMap();

            CreateMap<ImportBatchRowDto, ImportBatchRow>().ReverseMap();

            // Mapear ImportBatch -> ImportBatchDto
            CreateMap<ImportBatch, ImportBatchDto>()
                 .ForMember(d => d.StartedByUserName, opt => opt.MapFrom(s => s.StartedByUser.Person.FirstName + s.StartedByUser.Person.LastName))
                .ReverseMap();

            // Mapear ImportBatchRow -> ImportBatchRowDetailDto
            CreateMap<ImportBatchRow, ImportBatchRowDetailDto>();

            CreateMap<ImportBatchRow, ImportBatchRowTableDto>()
                .ForMember(d => d.Photo, opt => opt.MapFrom(src => src.PersonDivisionProfile.Person!.PhotoUrl))
                .ForMember(d => d.Name, opt => opt.MapFrom(src =>
                    src.PersonDivisionProfile.Person != null ? $"{src.PersonDivisionProfile.Person.FirstName} {src.PersonDivisionProfile.Person.LastName}" : "N/A"))
                .ForMember(d => d.Org, opt => opt.MapFrom(src =>
                    src.PersonDivisionProfile!.InternalDivision!.OrganizationalUnit!.Name))
                .ForMember(d => d.Division, opt => opt.MapFrom(src =>
                    src.PersonDivisionProfile!.InternalDivision!.Name))
                .ForMember(d => d.State, opt => opt.MapFrom(src =>
                    src.Card != null ? src.Card.Status!.Name : (src.Success ? "Activo" : "Error")))
                .ForMember(d => d.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
        }
    }
}
