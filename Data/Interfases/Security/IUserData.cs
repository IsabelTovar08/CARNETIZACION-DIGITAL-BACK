using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;

namespace Data.Interfases.Security
{
    public interface IUserData : ICrudBase<User>
    {
        Task<User?> FindByEmail(string email);
    }
}
