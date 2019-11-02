using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ZipTest.Db;

namespace ZipTest.Tests
{
    /// <summary>
    /// A factory for creating test database instances.
    /// </summary>
    /// <typeparam name="TContext">The type of context.</typeparam>
    public class DbContextFactory
    {
        private DbConnection connection;
        private bool isDisposed;

        /// <summary>
        /// Creates a new database context.
        /// </summary>
        /// <returns>The <see cref="TContext"/>.</returns>
        public ApplicationContext CreateContext()
        {
            if (connection is null)
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                var conn = connection as SqliteConnection;
                conn.CreateFunction("getdate", () => DateTime.Now);
            }

            var context = (ApplicationContext)Activator.CreateInstance(typeof(ApplicationContext), CreateOptions());
            context.Database.EnsureCreated();

            return context;
        }


        public DbContextOptions<ApplicationContext> CreateOptions()
        {
            if (connection is null)
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                var conn = connection as SqliteConnection;
                conn.CreateFunction("getdate", () => DateTime.Now);
            }

            return new DbContextOptionsBuilder<ApplicationContext>().UseSqlite(connection).Options;
        }



        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs disposing.
        /// </summary>
        /// <param name="disposing">Whether the method is called via <see cref="IDisposable.Dispose"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    connection?.Dispose();
                    connection = null;
                }

                isDisposed = true;
            }
        }
    }
}
