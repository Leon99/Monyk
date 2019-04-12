﻿using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Models;
using Monyk.GroundControl.Models;

namespace Monyk.GroundControl.Db
{
    public static class Bootstrapper
    {
        private const string SqliteFileName = "GroundControl.db";

        public static IServiceCollection AddDatabase(this IServiceCollection services, DatabaseType type, string connectionString)
        {
            return services.AddDbContextPool<MonykDbContext>(options =>
            {
                switch (type)
                {
                    case DatabaseType.Postgres:
                        options.UseNpgsql(connectionString);
                        break;
                    case DatabaseType.Sqlite:
                        options.UseSqlite(connectionString);
                        break;
                    default:
                        throw new ApplicationException(
                            $"Unable to initialize the storage due to misconfiguration. Database setting value '{type}' is not supported.");
                }
            });
        }

        public static void SeedDataForDevelopment(MonykDbContext db)
        {
            if (db.Database.IsSqlite())
            {
                if (!File.Exists(SqliteFileName))
                {
                    File.CreateText(SqliteFileName).Close();
                }
            }

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Monitors.AddRange(
                new MonitorEntity
                {
                    Type = MonitorType.Http,
                    Target = "https://github.com",
                    Interval = 10,
                    Description = "Happy HTTP monitor",
                },
                new MonitorEntity
                {
                    Type = MonitorType.Http,
                    Target = "https://foo.bar",
                    Interval = 55,
                    Description = "Sad HTTP monitor",
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "github.com",
                    Interval = 55,
                    Description = "Happy Ping monitor"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "foo.bar",
                    Interval = 55,
                    Description = "Sad Ping monitor"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "stopped.foo.bar",
                    Interval = 5,
                    IsStopped = true,
                    Description = "Stopped Ping monitor"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "deleted.foo.bar",
                    Interval = 5,
                    IsDeleted = true,
                    Description = "Deleted Ping monitor"
                }
            );
            db.SaveChanges();
        }
    }
}
