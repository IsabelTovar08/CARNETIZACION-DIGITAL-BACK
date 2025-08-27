using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class AttendanceData : BaseData<Attendance>, IAttendanceData
    {
        public AttendanceData(ApplicationDbContext context, ILogger<Attendance> logger)
            : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Set<Attendance>()
                .Include(x => x.AccessPointEntry)
                .Include(x => x.AccessPointExit).ThenInclude(x => x.Event)
                .Include(x => x.Person)
                .ToListAsync();
        }

        public async Task<Attendance?> GetOpenAttendanceAsync(int personId, CancellationToken ct = default)
        {
            // AsNoTracking para evitar conflictos de tracking cuando luego actualizamos
            return await _context.Set<Attendance>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted
                            && a.PersonId == personId
                            && a.TimeOfExit == null)
                .OrderByDescending(a => a.TimeOfEntry)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Attendance> UpdateExitAsync(int id, DateTime timeOfExit, int? accessPointOut, CancellationToken ct = default)
        {
            // Obtenemos la entidad "trackeada" y actualizamos sólo los campos de salida
            var entity = await _context.Set<Attendance>()
                .FirstOrDefaultAsync(a => a.Id == id, ct);

            if (entity == null)
                throw new InvalidOperationException($"No se encontró Attendance con Id={id} para actualizar la salida.");

            entity.TimeOfExit = timeOfExit;
            entity.AccessPointOfExit = accessPointOut;

            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}
