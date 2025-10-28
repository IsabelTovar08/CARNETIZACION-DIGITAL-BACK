using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Notifications.Request;
using Entity.Enums.Specifics;
using Utilities.Enums.Specifics;

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
        public static NotificationDtoRequest Create(
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
                        NotificationTypeId = (int)NotificationType.System
                    };

                case NotificationTemplateType.FirstLoginWelcome:
                    return new NotificationDtoRequest
                    {
                        Title = "¡Bienvenido al sistema!",
                        Message = $"Hola {args[0]}, es tu primera vez en el sistema de carnetización digital. " +
                                  "Explora las funcionalidades disponibles y mantén tu información actualizada.",
                        NotificationTypeId = (int)NotificationType.Info
                    };

                case NotificationTemplateType.PasswordChangeRequired:
                    return new NotificationDtoRequest
                    {
                        Title = "Cambio de contraseña requerido",
                        Message = "Por seguridad, debes cambiar la contraseña asignada automáticamente al registrarte. " +
                                  "Accede a tu perfil y establece una nueva contraseña personal.",
                        NotificationTypeId = (int)NotificationType.Warning
                    };

                case NotificationTemplateType.NewEvent:
                    return new NotificationDtoRequest
                    {
                        Title = "Nuevo evento disponible",
                        Message = $"Se ha creado un nuevo evento: \"{args[0]}\" el {((DateTime)args[1]):dd/MM/yyyy} " +
                                  $"a las {((DateTime)args[1]):HH:mm} en {args[2]}. ¡No olvides inscribirte!",
                        NotificationTypeId = (int)NotificationType.Info
                    };

                case NotificationTemplateType.EventReminder:
                    return new NotificationDtoRequest
                    {
                        Title = "Recordatorio de evento",
                        Message = $"Tienes programado el evento \"{args[0]}\" el {((DateTime)args[1]):dd/MM/yyyy} " +
                                  $"a las {((DateTime)args[1]):HH:mm}.",
                        NotificationTypeId = (int)NotificationType.Reminder
                    };

                case NotificationTemplateType.EventAttendance:
                    return new NotificationDtoRequest
                    {
                        Title = "Asistencia confirmada",
                        Message = $"Tu asistencia al evento \"{args[0]}\" del {((DateTime)args[1]):dd/MM/yyyy} " +
                                  "ha sido registrada exitosamente.",
                        NotificationTypeId = (int)NotificationType.Info
                    };

                case NotificationTemplateType.ModificationRequest:
                    return new NotificationDtoRequest
                    {
                        Title = "Solicitud de modificación de datos",
                        Message = $"{args[0]} ha solicitado una modificación de datos: {args[1]}.",
                        NotificationTypeId = (int)NotificationType.Warning
                    };

                case NotificationTemplateType.ModificationApproved:
                    return new NotificationDtoRequest
                    {
                        Title = "Modificación aprobada",
                        Message = $"Tu solicitud de modificación en el campo \"{args[0]}\" ha sido aprobada " +
                                  "y actualizada en el sistema.",
                        NotificationTypeId = (int)NotificationType.Info
                    };

                case NotificationTemplateType.ModificationRejected:
                    return new NotificationDtoRequest
                    {
                        Title = "Modificación rechazada",
                        Message = $"Tu solicitud de modificación en el campo \"{args[0]}\" ha sido rechazada. " +
                                  $"Motivo: {args[1]}.",
                        NotificationTypeId = (int)NotificationType.Warning
                    };

                case NotificationTemplateType.CardGenerated:
                    return new NotificationDtoRequest
                    {
                        Title = "Carnet generado",
                        Message = $"El carnet digital para {args[0]} ha sido generado exitosamente. " +
                                  $"Código: {args[1]}.",
                        NotificationTypeId = (int)NotificationType.System
                    };

                /// <summary>
                /// Notificación al iniciar sesión correctamente.
                /// </summary>
                case NotificationTemplateType.Login:
                    return new NotificationDtoRequest
                    {
                        Title = "Inicio de sesión exitoso",
                        Message = $"Bienvenido {args[0]}, tu acceso ha sido validado correctamente el {DateTime.Now:dd/MM/yyyy HH:mm}.",
                        NotificationTypeId = (int)NotificationType.System
                    };

                default:
                    throw new ArgumentException("Tipo de notificación no soportado.");
            }
        }
    }
}
