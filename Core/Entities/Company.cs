﻿using Kogel.Dapper.Extension.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hal.Core.Entities
{
    [Table("Company")]
    public class Company
    {
        [Identity(IsIncrease = true)]
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
