using Core.Entityframework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
                //m.Property(n => n.Name).HasMaxLength(50);//设置用户名最大长度为50个字符
                //m.Property(n => n.Version).HasMaxLength(20).IsRequired();//设置密码不可空且最大20个字符
            });

            base.OnModelCreating(modelBuilder);
        }

        public static void Init(string conn)
        {
            var builder = new DbContextOptionsBuilder<MyDbContext>();
            builder.UseMySQL(conn);
            using (var db = new MyDbContext(builder.Options))
            {
                db.Database.EnsureCreated();
                //db.Database.Migrate();
            }
        }
    }
}
