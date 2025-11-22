using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
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
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(eap => eap.AccessPoint)
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(eap => eap.Event)

                .Include(a => a.EventAccessPointExit)
                    .ThenInclude(eap => eap.AccessPoint)
                .Include(a => a.EventAccessPointExit)
                    .ThenInclude(eap => eap.Event)

                .Include(a => a.Person)

                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una asistencia por Id incluyendo todas sus relaciones necesarias.
        /// </summary>
        public override async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
                .Where(a => !a.IsDeleted && a.Id == id)
                .Include(a => a.Person) // Persona
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(eap => eap.Event) // Evento de entrada
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(eap => eap.AccessPoint) // Punto de acceso de entrada
                .Include(a => a.EventAccessPointExit)
                    .ThenInclude(eap => eap.Event) // Evento de salida
                .Include(a => a.EventAccessPointExit)
                    .ThenInclude(eap => eap.AccessPoint) // Punto de acceso de salida
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public async Task<Attendance?> GetOpenAttendanceAsync(int personId,int eventAccessPointId)
        {
            // 1️ Obtener el evento al que pertenece ESTE EventAccessPoint
            var eventId = await _context.EventAccessPoints
                .Where(ep => ep.Id == eventAccessPointId) 
                .Select(ep => ep.EventId)
                .FirstOrDefaultAsync();

            if (eventId == 0)
                return null;

            // 2️ Obtener TODOS los EventAccessPoints del MISMO evento
            //    (porque la entrada pudo ser en otro access point)
            var eventAccessPointsOfEvent = await _context.EventAccessPoints
                .Where(ep => ep.EventId == eventId)
                .Select(ep => ep.Id)   
                .ToListAsync();

            // 3️ Buscar asistencia abierta cuya ENTRADA pertenezca a ese evento
            return await _context.Attendances
                .Where(a =>
                    !a.IsDeleted &&
                    a.PersonId == personId &&
                    a.EventAccessPointExitId == null &&
                    eventAccessPointsOfEvent.Contains(a.EventAccessPointEntryId)
                )
                .OrderByDescending(a => a.TimeOfEntry)
                .FirstOrDefaultAsync();
        }


        // ✅ Nuevo método sobrescrito para recargar relaciones después de guardar (entrada)
        public override async Task<Attendance> SaveAsync(Attendance entity)
        {
            await base.SaveAsync(entity);

            // 🔹 Recargar la asistencia con sus relaciones completas
            var saved = await _context.Set<Attendance>()
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(eap => eap.Event)
                .Include(a => a.EventAccessPointExit)
                    .ThenInclude(eap => eap.Event)
                .FirstOrDefaultAsync(a => a.Id == entity.Id);

            return saved!;
        }

        public async Task<(IList<Attendance> Items, int Total)> QueryAsync(
            int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
            string? sortBy, string? sortDir, int page, int pageSize,
            CancellationToken ct = default)
        {
            var q = _context.Set<Attendance>()
                .AsNoTracking()
                .Include(a => a.Person)
                .Include(a => a.EventAccessPointEntry) 
                    .ThenInclude(ep => ep.AccessPoint)
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(ep => ep.Event)
                .Include(a => a.EventAccessPointExit)
                    .ThenInclude(ep => ep.AccessPoint)
                .Where(a => !a.IsDeleted);

            // 1️ Filtro por persona
            if (personId.HasValue)
                q = q.Where(a => a.PersonId == personId.Value);

            // 2️ Filtro por EVENTO usando EventAccessPoint
            if (eventId.HasValue)
            {
                q = q.Where(a =>
                    _context.EventAccessPoints.Any(ep =>
                        ep.Id == a.EventAccessPointEntryId &&
                        ep.EventId == eventId.Value
                    )
                    ||
                    _context.EventAccessPoints.Any(ep =>
                        ep.Id == a.EventAccessPointExitId &&
                        ep.EventId == eventId.Value
                    )
                );
            }

            // 3️ Filtro por fechas
            if (fromUtc.HasValue)
                q = q.Where(a => a.TimeOfEntry >= fromUtc.Value);

            if (toUtc.HasValue)
                q = q.Where(a => a.TimeOfEntry <= toUtc.Value);

            // 4️⃣ Sorting
            bool desc = string.Equals(sortDir, "DESC", StringComparison.OrdinalIgnoreCase);
            q = (sortBy ?? "TimeOfEntry") switch
            {
                "TimeOfExit" => desc ? q.OrderByDescending(a => a.TimeOfExit) : q.OrderBy(a => a.TimeOfExit),
                "PersonId" => desc ? q.OrderByDescending(a => a.PersonId) : q.OrderBy(a => a.PersonId),
                _ => desc ? q.OrderByDescending(a => a.TimeOfEntry) : q.OrderBy(a => a.TimeOfEntry)
            };

            // 5️⃣ Paginación
            int total = await q.CountAsync(ct);
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 20 : pageSize;
            int skip = (page - 1) * pageSize;

            var items = await q.Skip(skip).Take(pageSize).ToListAsync(ct);

            return (items, total);
        }


        // ✅ NUEVO MÉTODO — necesario para el AttendanceBusiness
        // Permite hacer Include() desde el negocio sin romper capas
        public IQueryable<Attendance> GetQueryable()
        {
            return _context.Set<Attendance>().AsQueryable();
        }
    }
}   