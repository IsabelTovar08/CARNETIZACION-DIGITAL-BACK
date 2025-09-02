using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Enums;
using Entity.DTOs.Enums;
using Utilities.Enums.Extensions;
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

                default:
                    throw new ArgumentException($"Enum type '{enumType}' not supported.");
            }
        }
    }
}
