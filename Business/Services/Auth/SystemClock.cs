using Business.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Auth
// Este servicio se usa para obtener la hora actual en UTC
// en que se genero el codigo.
{
    public class SystemClock: IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
