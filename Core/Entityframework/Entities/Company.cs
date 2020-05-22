using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entityframework.Entities
{
    [Table("Company")]
    public class Company
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
