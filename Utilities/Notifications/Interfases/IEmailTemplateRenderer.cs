using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Notifications.Interfases
{
    public interface IEmailTemplateRenderer
    {
        // Renderiza una plantilla Scriban con un diccionario de variables
        Task<string> RenderAsync(string templatePath, IDictionary<string, object> model);
    }
}
