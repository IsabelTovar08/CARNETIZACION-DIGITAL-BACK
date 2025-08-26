using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models.ModelSecurity;

namespace Business.Interfaces.Security
{
    public interface IPersonBusiness: IBaseBusiness<Person, PersonDtoRequest, PersonDto>
    {
        Task<(PersonRegistrerDto, bool?)> SavePersonAndUser(PersonRegistrer personUser);
    }
}
