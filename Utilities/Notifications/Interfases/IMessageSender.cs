using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notifications.Interfases
{
    public interface IMessageSender
    {
        Task SendMessageAsync(string to, string subject, string message);
    }
}
