using Hal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using SqlSugar;
using System;

namespace Hal.Core.Entityframework
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { set; get; }
        public DbSet<User> Users { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(m =>
            {
                m.HasKey(n => n.Id);
                m.Property(n => n.Id).HasComment("Unique Identitier");
                m.Property(n => n.Name).HasComment("Name of company");
                m.Property(n => n.No).HasComment("The No");
                m.Property(n => n.Version).HasComment("The Version");
            });

            base.OnModelCreating(modelBuilder);
        }

        public static void Init(string conn, DbType dbType)
        {
            var builder = new DbContextOptionsBuilder<MyDbContext>();

            switch (dbType)
            {
                case DbType.MySql:
                    builder.UseMySQL(conn);
                    break;
                case DbType.SqlServer:
                    builder.UseSqlServer(conn);
                    break;
                case DbType.Sqlite:
                    builder.UseSqlite(conn);
                    break;
                case DbType.Oracle:
                    builder.UseOracle(conn);
                    break;
                case DbType.PostgreSQL:
                    builder.UseNpgsql(conn);
                    break;
                default:
                    throw new Exception("NO such db type");
            }

            using var db = new MyDbContext(builder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
