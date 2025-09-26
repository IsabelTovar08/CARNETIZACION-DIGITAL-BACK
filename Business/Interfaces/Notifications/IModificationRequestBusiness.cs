using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Notifications.Request;
using Entity.Models.Organizational.Assignment;

namespace Business.Interfaces.Notifications
{
    public interface IModificationRequestBusiness : IBaseBusiness<ModificationRequest, ModificationRequestDtoRequest, ModificationRequestDtoResponse>
    {
    }
}
