using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exeptions
{
    public static class Validation
    {
        private static string Innermost(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }

    }
}
