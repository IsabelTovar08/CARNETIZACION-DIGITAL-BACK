using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfases.Transaction;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Implementations.Transaction
{
    /// <summary>
    /// Maneja transacciones usando EF Core DbContext.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            if (_transaction != null)
                return; // ya hay una transacción activa

            _transaction = await _context.Database.BeginTransactionAsync(isolation);
        }

        public async Task CommitAsync()
        {
            if (_transaction == null) return;

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
