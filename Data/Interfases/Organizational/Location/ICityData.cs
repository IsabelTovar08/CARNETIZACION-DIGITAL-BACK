using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Location;

namespace Data.Interfases.Organizational.Location
{
    public interface ICityData : ICrudBase<City>
    {
        Task<List<City>> GetCityesByDepartmentsAsync(int deparmentId);
    }
}
