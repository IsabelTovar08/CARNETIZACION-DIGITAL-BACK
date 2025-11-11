using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Notifications.Request;
using Utilities.Enums.Specifics;

namespace Business.Services.Notifications
{
    /// <summary>
    /// Plantillas predefinidas de notificaciones para el sistema de carnetización digital.
    /// </summary>
    public static class NotificationTemplates
    {
        // =============================
        // CARGA MASIVA
        // =============================

        /// <summary>
        /// Notificación cuando una carga masiva finaliza exitosamente.
        /// </summary>
        public static NotificationDtoRequest CreateBulkImportSuccessNotification(int totalRecords, string fileName)
        {
            return new NotificationDtoRequest
            {
                Title = "Carga masiva completada",
                Message = $"La carga masiva desde el archivo \"{fileName}\" finalizó exitosamente. " +
                          $"Se procesaron {totalRecords} registros correctamente.",
                NotificationTypeId = NotificationType.System
            };
        }

        // =============================
        // BIENVENIDA Y ACCESO
        // =============================

        /// <summary>
        /// Notificación de bienvenida cuando un usuario ingresa por primera vez.
        /// </summary>
        public static NotificationDtoRequest CreateFirstLoginWelcomeNotification(string userName)
        {
            return new NotificationDtoRequest
            {
                Title = "¡Bienvenido al sistema!",
                Message = $"Hola {userName}, es tu primera vez en el sistema de carnetización digital. " +
                          $"Explora las funcionalidades disponibles y mantén tu información actualizada.",
                NotificationTypeId = NotificationType.Info
            };
        }

        /// <summary>
        /// Notificación informando al usuario que debe cambiar su contraseña asignada automáticamente.
        /// </summary>
        public static NotificationDtoRequest CreatePasswordChangeRequiredNotification()
        {
            return new NotificationDtoRequest
            {
                Title = "Cambio de contraseña requerido",
                Message = "Por seguridad, debes cambiar la contraseña asignada automáticamente al registrarte. " +
                          "Accede a tu perfil y establece una nueva contraseña personal.",
                NotificationTypeId = NotificationType.Warning
            };
        }

        // =============================
        // EVENTOS
        // =============================

        /// <summary>
        /// Notificación de recordatorio para asistir a un evento.
        /// </summary>
        public static NotificationDtoRequest CreateEventReminderNotification(string eventName, DateTime eventDate)
        {
            return new NotificationDtoRequest
            {
                Title = "Recordatorio de evento",
                Message = $"Tienes programado el evento \"{eventName}\" el {eventDate:dd/MM/yyyy} a las {eventDate:HH:mm}.",
                NotificationTypeId = NotificationType.Reminder
            };
        }

        /// <summary>
        /// Notificación informando al usuario sobre un nuevo evento disponible.
        /// </summary>
        public static NotificationDtoRequest CreateNewEventNotification(string eventName, DateTime eventDate, string location)
        {
            return new NotificationDtoRequest
            {
                Title = "Nuevo evento disponible",
                Message = $"Se ha creado un nuevo evento: \"{eventName}\" el {eventDate:dd/MM/yyyy} a las {eventDate:HH:mm} " +
                          $"en {location}. ¡No olvides inscribirte!",
                NotificationTypeId = NotificationType.Info
            };
        }

        /// <summary>
        /// Notificación de asistencia registrada en un evento.
        /// </summary>
        public static NotificationDtoRequest CreateEventAttendanceNotification(string eventName, DateTime eventDate)
        {
            return new NotificationDtoRequest
            {
                Title = "Asistencia confirmada",
                Message = $"Tu asistencia al evento \"{eventName}\" del {eventDate:dd/MM/yyyy} ha sido registrada exitosamente.",
                NotificationTypeId = NotificationType.Info
            };
        }

        // =============================
        // MODIFICACIONES DE DATOS
        // =============================

        public static NotificationDtoRequest CreateModificationRequestNotification(string requesterName, string detail)
        {
            return new NotificationDtoRequest
            {
                Title = "Solicitud de modificación de datos",
                Message = $"{requesterName} ha solicitado una modificación de datos: {detail}.",
                NotificationTypeId = NotificationType.Warning
            };
        }

        public static NotificationDtoRequest CreateModificationApprovedNotification(string fieldUpdated)
        {
            return new NotificationDtoRequest
            {
                Title = "Modificación aprobada",
                Message = $"Tu solicitud de modificación en el campo \"{fieldUpdated}\" ha sido aprobada y actualizada en el sistema.",
                NotificationTypeId = NotificationType.Info
            };
        }

        public static NotificationDtoRequest CreateModificationRejectedNotification(string fieldRejected, string reason)
        {
            return new NotificationDtoRequest
            {
                Title = "Modificación rechazada",
                Message = $"Tu solicitud de modificación en el campo \"{fieldRejected}\" ha sido rechazada. Motivo: {reason}.",
                NotificationTypeId = NotificationType.Warning
            };
        }

        // =============================
        // CARNETS
        // =============================

        public static NotificationDtoRequest CreateCardGeneratedNotification(string personName, string cardCode)
        {
            return new NotificationDtoRequest
            {
                Title = "Carnet generado",
                Message = $"El carnet digital para {personName} ha sido generado exitosamente. Código: {cardCode}.",
                NotificationTypeId = NotificationType.System
            };
        }
    }
}
