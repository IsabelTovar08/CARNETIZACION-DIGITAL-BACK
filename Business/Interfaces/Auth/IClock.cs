using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Auth
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }

    }
}
