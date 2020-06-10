using System.ComponentModel.DataAnnotations.Schema;

namespace Hal.Core.Entities
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Psw { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string NameIdentifier { get; set; }
    }
}
