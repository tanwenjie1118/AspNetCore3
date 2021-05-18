using System;

namespace Hal.DesignPattern.Models
{
    public class Gloves : Product
    {
        public string Name { set; get; }
        public override bool Use()
        {
            return true;
        }
    }
}
