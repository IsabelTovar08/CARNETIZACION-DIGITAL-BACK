using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Location;
using Entity.Context;
using Entity.Models.Organizational.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Organizational.Location
{
    public class CityData : BaseData<City>, ICityData
    {
        public CityData(ApplicationDbContext context, ILogger<City> logger) : base(context, logger)
        {
        }

        public async Task<List<City>> GetCityesByDepartmentsAsync(int deparmentId)
        {
            return await _context.Set<City>()
                .Include(c => c.Department)
                .Where(c => c.DeparmentId == deparmentId)
                .ToListAsync();
        }
    }
}
