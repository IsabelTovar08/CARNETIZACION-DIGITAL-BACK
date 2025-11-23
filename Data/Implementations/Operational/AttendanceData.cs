using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.DTOs.Operational.Response;
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
            // QUERY BASE SIN INCLUDE
            var baseQuery = _context.Attendances
                .AsNoTracking()
                .Where(a => !a.IsDeleted);

            // Filtros
            if (personId.HasValue)
                baseQuery = baseQuery.Where(a => a.PersonId == personId.Value);

            if (eventId.HasValue)
            {
                baseQuery = baseQuery.Where(a =>
                    a.EventAccessPointEntry.EventId == eventId.Value ||
                    (a.EventAccessPointExit != null && a.EventAccessPointExit.EventId == eventId.Value)
                );
            }

            if (fromUtc.HasValue)
                baseQuery = baseQuery.Where(a => a.TimeOfEntry >= fromUtc.Value);

            if (toUtc.HasValue)
                baseQuery = baseQuery.Where(a => a.TimeOfEntry <= toUtc.Value);

            // 1️⃣ **Solo obtener los IDs de la asistencia más reciente por persona**
            var lastIdsQuery = baseQuery
                .GroupBy(a => a.PersonId)
                .Select(g => g
                    .OrderByDescending(x => x.TimeOfEntry)
                    .Select(x => x.Id)
                    .First());

            // Total (cantidad de personas únicas)
            int total = await lastIdsQuery.CountAsync(ct);

            // Paginación aplicada sobre los IDs
            var pagedIds = await lastIdsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            // 2️⃣ Ahora sí traer las entidades completas con Includes
            var items = await _context.Attendances
                .AsNoTracking()
                .Include(a => a.Person)
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(ep => ep.AccessPoint)
                .Include(a => a.EventAccessPointEntry)
                    .ThenInclude(ep => ep.Event)
                .Include(a => a.EventAccessPointExit)
                    .ThenInclude(ep => ep.AccessPoint)
                .Where(a => pagedIds.Contains(a.Id))
                .ToListAsync(ct);

            // 3️⃣ Ordenar en memoria (ya no se puede ordenar en SQL porque tenemos IDs)
            bool desc = string.Equals(sortDir, "DESC", StringComparison.OrdinalIgnoreCase);

            items = (sortBy ?? "TimeOfEntry") switch
            {
                "TimeOfExit" => desc ? items.OrderByDescending(a => a.TimeOfExit).ToList() : items.OrderBy(a => a.TimeOfExit).ToList(),
                "PersonId" => desc ? items.OrderByDescending(a => a.PersonId).ToList() : items.OrderBy(a => a.PersonId).ToList(),
                _ => desc ? items.OrderByDescending(a => a.TimeOfEntry).ToList() : items.OrderBy(a => a.TimeOfEntry).ToList()
            };

            return (items, total);
        }



        // ✅ NUEVO MÉTODO — necesario para el AttendanceBusiness
        // Permite hacer Include() desde el negocio sin romper capas
        public IQueryable<Attendance> GetQueryable()
        {
            return _context.Set<Attendance>().AsQueryable();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<IList<AttendanceDtoResponse>> GetAllByPersonEventAsync(int personId, int eventId)
        {
            return await _context.Attendances
                .AsNoTracking()
                .Where(a =>
                    a.PersonId == personId &&
                    (a.EventAccessPointEntry.EventId == eventId ||
                    (a.EventAccessPointExit != null && a.EventAccessPointExit.EventId == eventId))
                )
                .OrderByDescending(a => a.TimeOfEntry)
                .Select(a => new AttendanceDtoResponse
                {
                    Id = a.Id,
                    PersonId = a.PersonId,
                    TimeOfEntry = a.TimeOfEntry,
                    TimeOfExit = a.TimeOfExit,
                    AccessPointOfEntryName = a.EventAccessPointEntry.AccessPoint.Name,
                    AccessPointOfExitName = a.EventAccessPointExit != null
                        ? a.EventAccessPointExit.AccessPoint.Name
                        : null
                })
                .ToListAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="eventId"></param>
        /// <param name="currentAttendanceId"></param>
        /// <returns></returns>
        public async Task<bool> PersonHasMoreAttendancesAsync(int personId, int eventId, int currentAttendanceId)
        {
            return await _context.Attendances
                .AsNoTracking()
                .Where(a => a.PersonId == personId &&
                            !a.IsDeleted &&
                            (
                                a.EventAccessPointEntry.EventId == eventId ||
                                (a.EventAccessPointExit != null && a.EventAccessPointExit.EventId == eventId)
                            ) &&
                            a.Id != currentAttendanceId)
                .AnyAsync();
        }

    }
}
