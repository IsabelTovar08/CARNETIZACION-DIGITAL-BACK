using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Request;
using Entity.Models;
using Entity.Models.ModelSecurity;

namespace Data.Interfases.Security
{
    public interface IPersonData : ICrudBase<Person>
    {
        Task<Person?> FindByIdentification(string identification);
        Task<(Person Person, User User)> SavePersonAndUser(Person person, User user);

    }
}
