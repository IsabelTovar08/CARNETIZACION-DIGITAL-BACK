using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;

namespace Business.Interfaces.Organizational.Structure
{
    public interface IScheduleBusiness : IBaseBusiness<Schedule, ScheduleDtoRequest, ScheduleDto>
    {
    }
}
