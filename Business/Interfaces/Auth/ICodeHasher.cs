using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Auth
{
    public interface ICodeHasher
    {
        string Hash(string code);
        bool Verify(string code, string hash);
    }

}
