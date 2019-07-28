using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Monyk.Common.Db
{
    public static class Bootstrapper
    {
        public static void PrepareDb<T>(IHostingEnvironment env, T db, Action<T> seedAction) where T:DbContext
        {
            Directory.SetCurrentDirectory(env.ContentRootPath); // To make it consistent across different hosts

            if (env.IsDevelopment())
            {
                if (db.Database.IsSqlite())
                {
                    var sqliteFileName = db.Database.GetDbConnection().Database;
                    if (!File.Exists(sqliteFileName))
                    {
                        File.CreateText(sqliteFileName).Close();
                    }
                }

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                
                seedAction(db);
                db.SaveChanges();
            }
            else
            {
                // TODO replace with deployment-time migration
                db.Database.Migrate();
            }
        }
    }
}
