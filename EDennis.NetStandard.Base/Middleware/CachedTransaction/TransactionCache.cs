﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace EDennis.NetStandard.Base {

    /// <summary>
    /// Singleton Lifetime
    /// </summary>
    public class TransactionCache<TContext> : ConcurrentDictionary<Guid,IDbTransaction>
        where TContext : DbContext {  
    
        public void ReplaceDbContext(Guid key, DbContextProvider<TContext> provider) {

            var database = provider.DbContext.Database;
            var trans = GetOrAdd(key, (key) => {
                if (database.IsSqlServer()) {
                    var connection = new SqlConnection(database.GetDbConnection().ConnectionString);
                    connection.Open();
                    return connection.BeginTransaction();
                } else
                    throw new NotSupportedException("IDbTransactionCache supports SQL Server only");
            });

            var builder = new DbContextOptionsBuilder<TContext>();
            var options = builder.UseSqlServer(trans.Connection as SqlConnection).Options;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { options });
            context.Database.AutoTransactionsEnabled = false;
            context.Database.UseTransaction(trans as DbTransaction);

            provider.DbContext = context;
        }


        public DbContextProvider<TContext> CreateDbContextProvider(Guid key, string connectionString) {

            var trans = GetOrAdd(key, (key) => {
                    var connection = new SqlConnection(connectionString);
                    connection.Open();
                    return connection.BeginTransaction();
            });

            var builder = new DbContextOptionsBuilder<TContext>();
            var options = builder.UseSqlServer(trans.Connection as SqlConnection).Options;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { options });
            context.Database.AutoTransactionsEnabled = false;

            var provider = new DbContextProvider<TContext>(context);
            return provider;
        }


        public async Task RollbackAsync(Guid key) {
            if (TryGetValue(key, out IDbTransaction trans)) {
                await Task.Run(() => {
                    trans.Rollback();
                });
                TryRemove(key, out IDbTransaction _);
            }
        }

        public async Task CommitAsync(Guid key) {
            if (TryGetValue(key, out IDbTransaction trans)) {
                await Task.Run(() => {
                    trans.Commit();
                });
                TryRemove(key, out IDbTransaction _);
            }
        }


    }
}
