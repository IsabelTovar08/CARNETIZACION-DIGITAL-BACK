using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Notifications.Request;
using Entity.Enums.Specifics;
using Microsoft.AspNetCore.Http;
using Utilities.Enums.Specifics;
using Utilities.Helpers;

namespace Business.Services.Notifications
{
    /// <summary>
    /// Fábrica centralizada para crear notificaciones predefinidas.
    /// </summary>
    public static class NotificationFactory
    {
        /// <summary>
        /// Genera una notificación según el tipo de plantilla solicitado.
        /// </summary>
        public async static Task<NotificationDtoRequest> Create(
            NotificationTemplateType type,
            params object[] args)
        {
            switch (type)
            {
                case NotificationTemplateType.BulkImportSuccess:
                    return new NotificationDtoRequest
                    {
                        Title = "Carga masiva completada",
                        Message = $"La carga masiva desde el archivo \"{args[1]}\" finalizó exitosamente. " +
                                  $"Se procesaron {args[0]} registros correctamente.",
                        RedirectUrl = $"dashboard/operational/import-batches/{args[2]}/details",
                        NotificationType = NotificationType.System
                    };

                case NotificationTemplateType.FirstLoginWelcome:
                    return new NotificationDtoRequest
                    {
                        Title = "¡Bienvenido al sistema!",
                        Message = $"Hola {args[0]}, es tu primera vez en el sistema de carnetización digital. " +
                                  "Explora las funcionalidades disponibles y mantén tu información actualizada.",
                        NotificationType = NotificationType.Info
                    };

                case NotificationTemplateType.PasswordChangeRequired:
                    return new NotificationDtoRequest
                    {
                        Title = "Cambio de contraseña requerido",
                        Message = "Por seguridad, debes cambiar la contraseña asignada automáticamente al registrarte. " +
                                  "Accede a tu perfil y establece una nueva contraseña personal.",
                        NotificationType = NotificationType.Warning
                    };

                case NotificationTemplateType.NewEvent:
                    return new NotificationDtoRequest
                    {
                        Title = "Nuevo evento disponible",
                        Message = $"Se ha creado un nuevo evento: \"{args[0]}\" el {((DateTime)args[1]):dd/MM/yyyy} " +
                                  $"a las {((DateTime)args[1]):HH:mm} en {args[2]}. ¡No olvides inscribirte!",
                        NotificationType = NotificationType.Info
                    };

                case NotificationTemplateType.EventReminder:
                    return new NotificationDtoRequest
                    {
                        Title = "Recordatorio de evento",
                        Message = $"Tienes programado el evento \"{args[0]}\" el {((DateTime)args[1]):dd/MM/yyyy} " +
                                  $"a las {((DateTime)args[1]):HH:mm}.",
                        NotificationType = NotificationType.Reminder
                    };

                case NotificationTemplateType.EventAttendance:
                    return new NotificationDtoRequest
                    {
                        Title = "Asistencia confirmada",
                        Message = $"Tu asistencia al evento \"{args[0]}\" del {((DateTime)args[1]):dd/MM/yyyy} " +
                                  "ha sido registrada exitosamente.",
                        NotificationType = NotificationType.Info
                    };

                case NotificationTemplateType.ModificationRequest:
                    return new NotificationDtoRequest
                    {
                        Title = "Solicitud de modificación de datos",
                        Message = $"{args[0]} ha solicitado una modificación de datos: {args[1]}.",
                        NotificationType = NotificationType.Warning,
                        RedirectUrl = $"dashboard/operational/modification-request",
                        UserId = (int)args[2]
                    };

                case NotificationTemplateType.ModificationSent:
                    return new NotificationDtoRequest
                    {
                        Title = "Solicitud registrada",
                        Message = $"La solicitud de modificación del campo \"{args[0]}\" ha sido registrada. " +
                                  "Será evaluada conforme y se informará el resultado oportunamente.",
                        NotificationType = NotificationType.Info,
                        UserId = (int)args[1],
                        //RedirectUrl = 
                    };

                case NotificationTemplateType.ModificationApproved:
                    return new NotificationDtoRequest
                    {
                        Title = "Modificación aprobada",
                        Message = $"Tu solicitud de modificación en el campo \"{args[0]}\" ha sido aprobada " +
                                  "y actualizada en el sistema.",
                        NotificationType = NotificationType.Info,
                        UserId = (int)args[1],

                    };

                case NotificationTemplateType.ModificationRejected:
                    return new NotificationDtoRequest
                    {
                        Title = "Modificación rechazada",
                        Message = $"Tu solicitud de modificación en el campo \"{args[0]}\" ha sido rechazada. " +
                                  $"Motivo: {args[3]}.",
                        NotificationType = NotificationType.Warning,
                        UserId = (int)args[1],

                    };

                case NotificationTemplateType.CardGenerated:
                    return new NotificationDtoRequest
                    {
                        Title = "Carnet generado",
                        Message = $"El carnet digital para {args[0]} ha sido generado exitosamente. " +
                                  $"Código: {args[1]}.",
                        NotificationType = NotificationType.System
                    };

                /// <summary>
                /// Notificación al iniciar sesión correctamente.
                /// </summary>
                /// <summary>
                /// Notificación al iniciar sesión correctamente.
                /// </summary>
                case NotificationTemplateType.Login:
                    {
                        // args[0] = nombre usuario
                        // args[1] = id usuario
                        // args[2] = IDeviceInfoService
                        // args[3] = IHttpContextAccessor

                        var deviceInfoService = args[2] as IDeviceInfoService;
                        var httpContextAccessor = args[3] as IHttpContextAccessor;

                        var userAgent = httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"].ToString();

                        // ✔️ Obtener la IP real (funciona en Azure)
                        var realIp = deviceInfoService?.GetRealIp(httpContextAccessor);

                        // ✔️ Obtener ubicación real mediante la IP real
                        var location = deviceInfoService != null
                            ? await deviceInfoService.GetLocationFromIpAsync(realIp)
                            : "Ubicación desconocida";

                        var deviceModel = deviceInfoService?.GetDeviceModel(userAgent) ?? "Dispositivo desconocido";

                        return new NotificationDtoRequest
                        {
                            Title = "Inicio de sesión exitoso",
                            Message = $"Bienvenido {args[0]}, tu acceso fue validado el {DateTime.Now:dd/MM/yyyy HH:mm}",
                            NotificationType = NotificationType.System,
                            UserId = (int)args[1]
                        };
                    }

                case NotificationTemplateType.AttendanceEntry:
                    return new NotificationDtoRequest
                    {
                        Title = "Entrada registrada",
                        Message = $"Ingresaste a {args[1]} por el punto de acceso {args[2]} el {((DateTime)args[0]):dd/MM/yyyy HH:mm}.",
                        NotificationType = NotificationType.Info
                    };

                case NotificationTemplateType.AttendanceExit:
                    return new NotificationDtoRequest
                    {
                        Title = "Salida registrada",
                        Message = $"Saliste de {args[1]} por el punto de acceso {args[2]} el {((DateTime)args[0]):dd/MM/yyyy HH:mm}.",
                        NotificationType = NotificationType.Info
                    };


                default:
                    throw new ArgumentException("Tipo de notificación no soportado.");
            }
        }
    }
}
