using Business.Interfases;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Assignment
{
    public class PersonDivisionProfileController : GenericController<PersonDivisionProfile, PersonDivisionProfileDtoRequest, PersonDivisionProfileDto>
    {
        public PersonDivisionProfileController(IBaseBusiness<PersonDivisionProfile, PersonDivisionProfileDtoRequest, PersonDivisionProfileDto> business, ILogger<PersonDivisionProfileController> logger) : base(business, logger)
        {
        }
    }
}
