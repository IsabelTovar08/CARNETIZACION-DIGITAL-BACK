using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Enums;
using Entity.DTOs.Enums;
using Entity.Enums.Extensions;
using Entity.Enums.Specifics;
using Utilities.Enums.Specifics;

namespace Business.Services.Enums
{
    public class EnumCatalogService : IEnumCatalogService
    {
        public Task<IEnumerable<EnumOptionDto>> GetByTypeAsync(string enumType)
        {
            enumType = enumType.ToLowerInvariant();

            switch (enumType)
            {
                case "documenttypes":
                case "documenttype":
                    return Task.FromResult(
                        EnumExtensions.ToItems<DocumentType>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = i.Acronym,
                                Code = Enum.GetName(typeof(DocumentType), i.Id)
                            })
                    );

                case "bloodtypes":
                case "bloodtype":
                    return Task.FromResult(
                        EnumExtensions.ToItems<BloodType>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = null,
                                Code = Enum.GetName(typeof(BloodType), i.Id)
                            })
                    );

                case "accesspointtypes":
                case "accesspointtype":
                    return Task.FromResult(
                        EnumExtensions.ToItems<AccessPointType>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = null,
                                Code = Enum.GetName(typeof(AccessPointType), i.Id)
                            })
                    );

                case "notification-status":
                    return Task.FromResult(
                        EnumExtensions.ToItems<NotificationStatus>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = null,
                                Code = Enum.GetName(typeof(NotificationStatus), i.Id)
                            })
                    );

                case "notification-type":
                    return Task.FromResult(
                        EnumExtensions.ToItems<NotificationType>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = null,
                                Code = Enum.GetName(typeof(NotificationType), i.Id)
                            })
                    );

                case "modification-request-status":
                    return Task.FromResult(
                        EnumExtensions.ToItems<ModificationRequestStatus>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = null,
                                Code = Enum.GetName(typeof(ModificationRequestStatus), i.Id)
                            })
                    );

                case "modification-fields":
                    return Task.FromResult(
                        EnumExtensions.ToItems<ModificationField>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = null,
                                Code = Enum.GetName(typeof(ModificationField), i.Id)
                            })
                    );

                case "modification-reason":
                    return Task.FromResult(
                        EnumExtensions.ToItems<ModificationReason>()
                            .Select(i => new EnumOptionDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Acronym = null,
                                Code = Enum.GetName(typeof(ModificationReason), i.Id)
                            })
                    );

                default:
                    throw new ArgumentException($"Enum type '{enumType}' not supported.");
            }
        }
    }
}
