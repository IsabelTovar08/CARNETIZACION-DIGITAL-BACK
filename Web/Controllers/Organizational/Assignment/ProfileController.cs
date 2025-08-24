using Business.Interfases;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Assignment
{
    public class ProfileController : GenericController<Profiles, ProfileDtoRequest, ProfileDto>
    {
        public ProfileController(IBaseBusiness<Profiles, ProfileDtoRequest, ProfileDto> business, ILogger<Profiles> logger) : base(business, logger)
        {
        }
    }
}
