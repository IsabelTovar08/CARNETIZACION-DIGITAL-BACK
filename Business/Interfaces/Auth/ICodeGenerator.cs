using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Auth
{
    public interface ICodeGenerator 
    {
        string Generate(int digits = 5);
    }
}
