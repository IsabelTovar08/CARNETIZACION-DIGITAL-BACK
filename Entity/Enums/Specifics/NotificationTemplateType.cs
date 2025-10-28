using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Enums.Specifics
{
    /// <summary>
    /// Tipos de plantillas predefinidas de notificación.
    /// </summary>
    public enum NotificationTemplateType
    {
        BulkImportSuccess,
        FirstLoginWelcome,
        PasswordChangeRequired,
        NewEvent,
        EventReminder,
        EventAttendance,
        ModificationRequest,
        ModificationApproved,
        ModificationRejected,
        CardGenerated,
        Login
    }
}
