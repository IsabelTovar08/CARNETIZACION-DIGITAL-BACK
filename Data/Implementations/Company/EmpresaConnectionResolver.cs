using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;

namespace Business.Services.Company
{
    public class EmpresaConnectionResolver
    {
        private readonly MasterDbContext _globalDb;

        public EmpresaConnectionResolver(MasterDbContext globalDb)
        {
            _globalDb = globalDb;
        }

        public string GetConnectionString(int empresaId)
        {
            var empresa = _globalDb.Companies.FirstOrDefault(e => e.Id == empresaId);
            if (empresa == null)
                throw new Exception("Empresa no encontrada");

            return empresa.ConnectionString;
        }
    }

}
