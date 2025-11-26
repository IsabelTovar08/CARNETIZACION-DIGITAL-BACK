using System.Globalization;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
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
using Entity.DTOs.Specifics.Cards;
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
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.HasCard,
                    opt => opt.MapFrom(src =>
                        src.IssuedCard.Any(c => c.Card != null && !c.IsDeleted)))

                .ForMember(dest => dest.Cards,
                    opt => opt.MapFrom(src =>
                        src.IssuedCard
                            .Where(c => !c.IsDeleted)
                    ))
                .ReverseMap();


            CreateMap<PersonDtoRequest, Person>().ReverseMap();


            CreateMap<Person, PersonInfoDto>()
            // Info básica de la persona
            .ForMember(d => d.PersonalInfo, o => o.MapFrom(s => s))

            // División actual (IsCurrentlySelected)
            .ForMember(d => d.DivissionId, o => o.MapFrom(s =>
                s.IssuedCard.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.Id))
            .ForMember(d => d.DivissionName, o => o.MapFrom(s =>
                s.IssuedCard.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.Name))

            // Unidad
            .ForMember(d => d.UnitId, o => o.MapFrom(s =>
                s.IssuedCard.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.OrganizationalUnit.Id))
            .ForMember(d => d.UnitName, o => o.MapFrom(s =>
                s.IssuedCard.FirstOrDefault(p => p.IsCurrentlySelected).InternalDivision.OrganizationalUnit.Name))

            // Organización (desde Branch → Organization)
            .ForMember(d => d.OrganizationId, o => o.MapFrom(s =>
                s.IssuedCard.FirstOrDefault(p => p.IsCurrentlySelected)
                    .InternalDivision.OrganizationalUnit.OrganizationalUnitBranches
                    .Select(oub => oub.Branch.Organization.Id)
                    .FirstOrDefault()))
            .ForMember(d => d.OrganizationName, o => o.MapFrom(s =>
                s.IssuedCard.FirstOrDefault(p => p.IsCurrentlySelected)
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
             .ForMember(dest => dest.EmailPerson, opt => opt.MapFrom(src => src.Person.Email ?? src.UserName))
             .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(R => R.Rol)))
             .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
             .ReverseMap();

            CreateMap<User, UserDtoRequest>().ReverseMap();


            CreateMap<User, UserMeDto>()
            .ForMember(d => d.PhotoUrl, opt => opt.MapFrom(s => s.Person.Id))
            .ForMember(d => d.PhotoUrl, opt => opt.MapFrom(s => s.Person.PhotoUrl))
            .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRoles.Select(ur => ur.Rol)))
            .ForMember(d => d.Permissions, opt => opt.MapFrom(s =>
        s.UserRoles
         .SelectMany(ur => ur.Rol.RolFormPermissions.Select(rp => rp.Permission))
         .DistinctBy(p => p.Id)));
           

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
            CreateMap<CardConfiguration, CardConfigurationDto>()
            //.ForMember(d => d.PersonId, o => o.MapFrom(s => s.IssuedCard.Person.Id))
            //.ForMember(d => d.PersonFullName, o => o.MapFrom(s => s.IssuedCard.Person.FirstName + " " + s.IssuedCard.Person.LastName))
            //.ForMember(d => d.DivisionId, o => o.MapFrom(s => s.IssuedCard.InternalDivision.Id))
            //.ForMember(d => d.DivisionName, o => o.MapFrom(s => s.IssuedCard.InternalDivision.Name))
            //.ForMember(d => d.ProfileId, o => o.MapFrom(s => s.IssuedCard.Profile.Id))
            .ForMember(d => d.ProfileName, o => o.MapFrom(s => s.Profile.Name))
            //.ForMember(d => d.AreaCategoryName, o => o.MapFrom(s => s.IssuedCard.InternalDivision.AreaCategory.Name))

            .ReverseMap();
            //.ForMember(s => s.IssuedCard, o => o.Ignore());

            CreateMap<CardConfiguration, CardConfigurationDtoRequest>()
                .ReverseMap();



            CreateMap<CardTemplate, CardTemplateRequest>().ReverseMap();
            CreateMap<CardTemplate, CardTemplateResponse>().ReverseMap();

            //IssuedCard
            CreateMap<IssuedCard, IssuedCardDto>()
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
                .ForMember(dest => dest.DivisionName, opt => opt.MapFrom(src => src.InternalDivision.Name))
                .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.Card.Profile.Name))
                .ReverseMap();

            CreateMap<IssuedCard, IssuedCardDtoRequest>()
                .ReverseMap();


            CreateMap<IssuedCard, IssuedCardBasicDto>()
                .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.Card.Profile.Name))
                .ForMember(dest => dest.InternalDivisionName,  opt => opt.MapFrom(src => src.InternalDivision.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.StatusId));


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
                .ReverseMap();

            //Structure

            //Organization 
            CreateMap<Organization, OrganizationDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.OrganizaionType.Name))
                .ReverseMap();
            CreateMap<Organization, OrganizationDtoRequest>().ReverseMap();

            //Area Categoria
            CreateMap<AreaCategory, AreaCategoryDto>()
                .ReverseMap();

            CreateMap<AreaCategory, AreaCategoryDtoRequest>()
                .ReverseMap();

            //Event
            CreateMap<Event, EventDtoResponse>()
                .ForMember(d => d.Ispublic, o => o.MapFrom(s => s.IsPublic))
                .ForMember(d => d.EventTypeName, o => o.MapFrom(s => s.EventType != null ? s.EventType.Name : null))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status != null ? s.Status.Name : null))
                .ForMember(d => d.QrCodeBase64, o => o.MapFrom(s => s.QrCodeBase64))


                // 🔹 Enviar TODOS los horarios del evento
                .ForMember(d => d.Schedules, o => o.MapFrom(s =>
                    s.EventSchedules.Select(es => new ScheduleDto
                    {
                        Id = es.Schedule.Id,
                        Name = es.Schedule.Name,
                        StartTime = es.Schedule.StartTime,
                        EndTime = es.Schedule.EndTime,
                        Days = es.Schedule.Days != null
                            ? es.Schedule.Days.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim()).ToList()
                            : new List<string>()
                    })
                ))



               // 🔹 Access points
               .ForMember(d => d.AccessPoints, opt => opt.MapFrom(s =>
                    s.EventAccessPoints.Select(eap => new AccessPointDtoResponsee
                    {
                        Id = eap.AccessPoint.Id,
                        Name = eap.AccessPoint.Name,
                        Description = eap.AccessPoint.Description,
                        TypeId = eap.AccessPoint.TypeId,
                        Type = eap.AccessPoint.AccessPointType != null
                            ? eap.AccessPoint.AccessPointType.Name
                            : null,

                        
                        QrCodeKey = eap.QrCodeKey,
                         Code = eap.AccessPoint.Code,

                        EventId = eap.EventId,
                        EventName = eap.Event.Name
                    })
                ));



            //EventSchedule
            CreateMap<EventSchedule, EventScheduleDtoResponse>()
            .ForMember(d => d.ScheduleId, o => o.MapFrom(s => s.Schedule.Id))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Schedule.Name))
            .ForMember(d => d.StartTime, o => o.MapFrom(s => s.Schedule.StartTime))
            .ForMember(d => d.EndTime, o => o.MapFrom(s => s.Schedule.EndTime))
            .ForMember(d => d.Days, o => o.MapFrom(s =>
                s.Schedule.Days != null
                    ? s.Schedule.Days.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(x => x.Trim())
                          .ToList()
                    : new List<string>()
            ));


            CreateMap<Event, EventDetailsDtoResponse>()
             .ForMember(d => d.AccessPoints, opt => opt.MapFrom(s =>
                 s.EventAccessPoints.Select(eap => new AccessPointDtoResponsee
                 {
                     Id = eap.AccessPoint.Id,
                     Name = eap.AccessPoint.Name,
                     Description = eap.AccessPoint.Description,
                     TypeId = eap.AccessPoint.TypeId,
                     Type = eap.AccessPoint.AccessPointType != null ? eap.AccessPoint.AccessPointType.Name : null,
                     QrCodeKey = eap.QrCodeKey,
                     Code = eap.AccessPoint.Code
                 })
             ))
             .ForMember(d => d.Audiences, opt => opt.MapFrom(s => s.EventTargetAudiences));

            CreateMap<EventDtoRequest, Event>()
             .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
             .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
             .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
             .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
             .ForMember(d => d.IsPublic, o => o.MapFrom(s => s.Ispublic))
             .ForMember(d => d.EventStart, o => o.MapFrom(s => s.EventStart))
             .ForMember(d => d.EventEnd, o => o.MapFrom(s => s.EventEnd))
             .ForMember(d => d.EventTypeId, o => o.MapFrom(s => s.EventTypeId))
             .ForMember(d => d.StatusId, o => o.MapFrom(s => s.StatusId))
             .ForMember(d => d.QrCodeBase64, o => o.Ignore())

             // 🔹 Ignora relaciones — se manejan manualmente
             .ForMember(d => d.EventAccessPoints, o => o.Ignore())
             .ForMember(d => d.EventTargetAudiences, o => o.Ignore())
             .ForMember(d => d.EventSchedules, o => o.Ignore());




            //EventType
            CreateMap<EventType, EventTypeDtoRequest>().ReverseMap();
            CreateMap<EventType, EventTypeDtoResponse>().ReverseMap();

            //EventAccessPoint
            // Modelo → DTO Response
            CreateMap<EventAccessPoint, EventAccessPointDto>()
                .ForMember(dest => dest.EventName,
                    opt => opt.MapFrom(src => src.Event != null ? src.Event.Name : string.Empty))
                .ForMember(dest => dest.AccessPointName,
                    opt => opt.MapFrom(src => src.AccessPoint != null ? src.AccessPoint.Name : string.Empty));
                

            // DTO Request → Modelo
            CreateMap<EventAccessPointDtoRequest, EventAccessPoint>().ReverseMap();

            // DTO Response → Modelo 
            CreateMap<EventAccessPoint, EventAccessPointDto>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.AccessPointName, opt => opt.MapFrom(src => src.AccessPoint != null ? src.AccessPoint.Name : string.Empty))
                .ReverseMap();

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
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.TypeId, o => o.MapFrom(s => s.TypeId))
                .ForMember(d => d.QrCode, o => o.Ignore());

            CreateMap<AccessPoint, AccessPointDtoRequest>().ReverseMap();

            // ENTIDAD -> DTO
            CreateMap<AccessPoint, AccessPointDtoResponsee>()
             .ForMember(d => d.EventId, opt => opt.MapFrom(s => s.EventAccessPoints.Select(eap => eap.EventId).FirstOrDefault()))
             .ForMember(d => d.EventName, opt => opt.MapFrom(s => s.EventAccessPoints.Select(eap => eap.Event.Name).FirstOrDefault()))
             .ForMember(d => d.Type, opt => opt.MapFrom(s => s.AccessPointType != null ? s.AccessPointType.Name : null))
             .ForMember(d => d.QrCodeKey, opt => opt.MapFrom(s => s.EventAccessPoints.Select(eap => eap.QrCodeKey).FirstOrDefault()));


            // DTO -> ENTIDAD
            CreateMap<AccessPointDtoResponsee, AccessPoint>()
                .ForMember(d => d.AccessPointType, opt => opt.Ignore());


            //Schedule
            CreateMap<Schedule, ScheduleDto>()
            .ForMember(d => d.Days, o => o.MapFrom(s =>
                !string.IsNullOrWhiteSpace(s.Days)
                    ? s.Days.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToList()
                    : new List<string>()))
            .ReverseMap()
            .ForMember(s => s.Days, o => o.MapFrom(d =>
                (d.Days != null && d.Days.Any())
                    ? string.Join(",", d.Days.Select(x => x.Trim()))
                    : null));

            CreateMap<Schedule, ScheduleDtoRequest>()
              .ForMember(d => d.Days, o => o.MapFrom(s =>
                 !string.IsNullOrWhiteSpace(s.Days)
                     ? s.Days.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(x => x.Trim())
                        .ToList()
                         : new List<string>()))
                        .ReverseMap()
              .ForMember(s => s.Days, o => o.MapFrom(d =>
                       (d.Days != null && d.Days.Any())
                         ? string.Join(",", d.Days.Select(x => x.Trim()))
              : null));

            // EventTargetAudience
            CreateMap<EventTargetAudience, EventTargetAudienceDtoRequest>().ReverseMap();

            CreateMap<EventTargetAudience, EventTargetAudienceViewDtoResponse>()
            .ForMember(d => d.ReferenceId, opt => opt.MapFrom(src =>
                src.TypeId == 1 ? src.ProfileId :
                src.TypeId == 2 ? src.OrganizationalUnitId :
                src.TypeId == 3 ? src.InternalDivisionId : 0))
            .ForMember(d => d.ReferenceName, opt => opt.MapFrom(src =>
                src.TypeId == 1 && src.Profile != null ? src.Profile.Name :
                src.TypeId == 2 && src.OrganizationalUnit != null ? src.OrganizationalUnit.Name :
                src.TypeId == 3 && src.InternalDivision != null ? src.InternalDivision.Name :
                null));




            //Notifications
            CreateMap<Notification, NotificationDto>().ReverseMap();

            CreateMap<Notification, NotificationDtoRequest>()
                .ReverseMap();

            //NotificationsReceived
            CreateMap<NotificationReceived, NotificatioReceivedDtoResponse>()
                .ReverseMap();

            CreateMap<NotificationReceived, NotificationReceivedDtoRequest>()
                .ReverseMap();

            CreateMap<NotificationReceived, NotificationReceivedDto>()
                .ReverseMap();


            CreateMap<ModificationRequest, ModificationRequestResponseDto>()
            .ForMember(dest => dest.FieldId, opt => opt.MapFrom(src => (int)src.Field))
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => ((ModificationField)src.Field).GetDisplayName()))
            .ForMember(dest => dest.ReasonId, opt => opt.MapFrom(src => (int)src.Reason))
            .ForMember(dest => dest.ReasonName, opt => opt.MapFrom(src => ((ModificationReason)src.Reason).GetDisplayName()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => ((ModificationRequestStatus)src.Status).GetDisplayName()))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => (src.User.Person.FirstName + " " + src.User.Person.MiddleName + " "+ src.User.Person.LastName + " " + src.User.Person.SecondLastName)))
            .ForMember(dest => dest.UserIdentification, opt => opt.MapFrom(src => (src.User.Person.DocumentNumber)))


            .ReverseMap();

            // De Create DTO a Entity
            //CreateMap<ModificationRequestResponseDto, ModificationRequest>()
            //    .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ModificationRequestStatus.Pending))
            //    .ReverseMap();

            CreateMap<ModificationRequest, ModificationRequestDto>().ReverseMap();
            // Attendance

            CreateMap<Attendance, AttendanceDtoRequest>().ReverseMap();

            CreateMap<Attendance, AttendanceDtoResponse>()
                .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => src.Person != null ? src.Person.FirstName + " " + src.Person.MiddleName + " " + src.Person.LastName + " " + src.Person.SecondLastName : string.Empty))
                .ForMember(dest => dest.AccessPointEntryId, opt => opt.MapFrom(src => src.EventAccessPointEntry.AccessPoint.Id))
                .ForMember(dest => dest.AccessPointExitId, opt => opt.MapFrom(src => src.EventAccessPointExit.AccessPoint.Id))
                .ForMember(dest => dest.AccessPointOfEntryName, opt => opt.MapFrom(src =>  src.EventAccessPointEntry.AccessPoint.Name))
                .ForMember(dest => dest.AccessPointOfExitName, opt => opt.MapFrom(src => src.EventAccessPointExit.AccessPoint.Name))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.EventAccessPointEntry.Event.Name));

            //.ForMember(dest => dest.EventName,
            //    opt => opt.MapFrom(src =>
            //        src.AccessPointEntry != null && src.AccessPointEntry.EventAccessPoints.Any()
            //            ? src.AccessPointEntry.EventAccessPoints.Select(eap => eap.Event.Name).FirstOrDefault()
            //            : (
            //                src.AccessPointExit != null && src.AccessPointExit.EventAccessPoints.Any()
            //                    ? src.AccessPointExit.EventAccessPoints.Select(eap => eap.Event.Name).FirstOrDefault()
            //                    : null
            //            )
            //    ))
            //.ForMember(dest => dest.EventId,
            //    opt => opt.MapFrom(src =>
            //        src.AccessPointEntry != null && src.AccessPointEntry.EventAccessPoints.Any()
            //            ? src.AccessPointEntry.EventAccessPoints.Select(eap => eap.EventId).FirstOrDefault()
            //            : (
            //                src.AccessPointExit != null && src.AccessPointExit.EventAccessPoints.Any()
            //                    ? src.AccessPointExit.EventAccessPoints.Select(eap => eap.EventId).FirstOrDefault()
            //                    : (int?)null
            //            )
            //    ))
            //// ➕ Formateo de fechas a string (cultura es-CO). Sin helpers externos.
            //.ForMember(dest => dest.TimeOfEntryStr,
            //    opt => opt.MapFrom(src => src.TimeOfEntry.ToString("dd/MM/yyyy HH:mm", new CultureInfo("es-CO"))))
            //.ForMember(dest => dest.TimeOfExitStr,
            //    opt => opt.MapFrom(src => src.TimeOfExit.HasValue
            //        ? src.TimeOfExit.Value.ToString("dd/MM/yyyy HH:mm", new CultureInfo("es-CO"))
            //        : null))
            //.ReverseMap()
            //    .ForMember(dest => dest.Person, opt => opt.Ignore())
            //    .ForMember(dest => dest.AccessPointEntry, opt => opt.Ignore())
            //    .ForMember(dest => dest.AccessPointExit, opt => opt.Ignore());


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

            CreateMap<ImportBatchRow, ImportBatchRowDto>()
                .ForMember(d => d.PersonName, opt => opt.MapFrom(s => s.Person.FirstName +" "+ s.Person.MiddleName + " " + s.Person.LastName + " " + s.Person.SecondLastName))
                .ForMember(d => d.Identification, opt => opt.MapFrom(s => s.Person.DocumentType +" - "+ s.Person.DocumentNumber))

                .ReverseMap();

            // Mapear ImportBatch -> ImportBatchDto
            CreateMap<ImportBatch, ImportBatchDto>()
                 .ForMember(d => d.StartedByUserName, opt => opt.MapFrom(s => s.StartedByUser.Person.FirstName + s.StartedByUser.Person.LastName))
                .ReverseMap();

            // Mapear ImportBatchRow -> ImportBatchRowDetailDto
            CreateMap<ImportBatchRow, ImportBatchRowDetailDto>();

            CreateMap<ImportBatchRow, ImportBatchRowTableDto>()
                .ForMember(d => d.Photo, opt => opt.MapFrom(src => src.IssuedCard.Person!.PhotoUrl))
                .ForMember(d => d.Name, opt => opt.MapFrom(src =>
                    src.IssuedCard.Person != null ? $"{src.IssuedCard.Person.FirstName} {src.IssuedCard.Person.LastName}" : "N/A"))
                .ForMember(d => d.Org, opt => opt.MapFrom(src =>
                    src.IssuedCard!.InternalDivision!.OrganizationalUnit!.Name))
                .ForMember(d => d.Division, opt => opt.MapFrom(src =>
                    src.IssuedCard!.InternalDivision!.Name))
                .ForMember(d => d.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
        }
    }
}
