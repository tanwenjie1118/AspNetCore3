using Core.Entityframework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Entityframework
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { set; get; }

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

        public static void Init(string conn)
        {
            var builder = new DbContextOptionsBuilder<MyDbContext>();
            
            // if mysql
            builder.UseMySQL(conn);

            // if sqlserver
            // builder.UseSqlServer(conn);
            using var db = new MyDbContext(builder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
