using Business.Interfaces.Notifications;
using Business.Interfases;
using Entity.DTOs.Notifications.Request;
using Entity.Models.Organizational.Assignment;
using Web.Controllers.Base;

namespace Web.Controllers.Notifications
{
    public class ModificationRequestController : GenericController<ModificationRequest, ModificationRequestDtoRequest, ModificationRequestDtoResponse>
    {
        protected readonly IModificationRequestBusiness _business;
        public ModificationRequestController(IModificationRequestBusiness business, ILogger<ModificationRequestController> logger) : base(business, logger)
        {
            _business = business;   
        }
    }
}
