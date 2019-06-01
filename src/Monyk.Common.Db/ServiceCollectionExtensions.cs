using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Db.Models;

namespace Monyk.Common.Db
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, DatabaseSettings dbSettings, string migrationsAssembly) where T : DbContext
        {
            return services.AddDbContextPool<T>(options =>
            {
                switch (dbSettings.Type)
                {
                    case DatabaseType.Postgres:
                        options.UseNpgsql(dbSettings.ConnectionString, _ => _.MigrationsAssembly(migrationsAssembly));
                        break;
                    case DatabaseType.Sqlite:
                        options.UseSqlite(dbSettings.ConnectionString, _ => _.MigrationsAssembly(migrationsAssembly));
                        break;
                    default:
                        throw new ApplicationException(
                            $"Unable to initialize the storage due to misconfiguration. Database setting value '{dbSettings.Type}' is not supported.");
                }
            });
        }
    }
}
