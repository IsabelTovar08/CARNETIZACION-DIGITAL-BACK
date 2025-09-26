using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Transaction
{
    /// <summary>
    /// Contrato para manejar transacciones.
    /// </summary>
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.ReadCommitted);
        Task CommitAsync();
        Task RollbackAsync();
    }
}
