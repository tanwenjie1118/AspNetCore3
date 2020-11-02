using Hal.AspDotNetCore3;
using Hal.Core.Entityframework;
using Hal.Infrastructure.Configuration;
using Hal.Infrastructure.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests.CodeFirst
{
    public class TemplateDbcontext : DbContext
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IEFRepository dbcontext;
        private readonly DatabaseOption dbOption;
        public TemplateDbcontext()
        {
            dbcontext = testWebServer.Resolve<IEFRepository>();
            dbOption = testWebServer.Resolve<DatabaseOption>();
        }

        [Fact]
        public void InitMySql()
        {
            MyDbContext.Init(dbOption.MySql.Conn, SqlSugar.DbType.MySql);
        }

        private static void AddExtensionDescription(TemplateDbcontext db)
        {
            var props = typeof(TemplateDbcontext).GetProperties();
            string sql = "EXECUTE sp_addextendedproperty N'MS_Description','{0}',N'user',N'dbo',N'table',N'{1}',N'column',N'{2}' ";
            // mssql --add table comment  
            // EXEC sp_addextendedproperty 'MS_Description', 'This is xxxxxxxxxxxxxx', 'SCHEMA', dbo, 'table', LOG_REV_SPEC, null, null;
            // mssql --add field comment 
            // EXEC sp_addextendedproperty 'MS_Description', 'This is xxxxxxxxxxxxxx', 'SCHEMA', dbo, 'table', LOG_REV_SPEC, 'column', logid;

            string oracleSql = "x";
            // Oracle --add table comment 
            // COMMENT ON TABLE STUDENT_INFO IS 'This is xxxxxxxxxxxxxx';
            // Oracle --add field name 
            // COMMENT ON COLUMN STUDENT_INFO.STU_ID IS 'This is xxxxxxxxxxxxxx';

            string mysqlSql = "x";
            // Mysql --add table comment 
            // ALTER TABLE tb_user COMMENT 'This is xxxxxxxxxxxxxx';
            // Mysql --add field name 
            // ALTER TABLE tb_user MODIFY COLUMN name VARCHAR(30) NOT NULL COMMENT 'This is xxxxxxxxxxxxxx';
            StringBuilder sb = new StringBuilder();
            foreach (var property in props)
            {
                if (property.PropertyType.Name.Contains(nameof(DbSet)))
                {
                    var tableType = property.PropertyType.GenericTypeArguments[0];
                    var tableAttr = tableType.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(TableAttribute));
                    if (tableAttr.IsNull()) return;

                    var tableName = tableAttr.ConstructorArguments[0].Value.ToString();
                    var columns = tableType.GetProperties();
                    foreach (var column in columns)
                    {
                        var attr = column.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(DescriptionAttribute));

                        if (attr.IsNotNull())
                        {
                            var remark = attr.ConstructorArguments[0].Value.ToString();
                            if (remark.IsNotNullOrWhiteSpace())
                            {
                                sb.AppendLine(string.Format(sql, remark, tableName, column.Name));
                            }
                        }
                    }
                }
            }

            var sqls = sb.ToString();
            db.Database.ExecuteSqlCommand(sqls);
        }
    }
}
