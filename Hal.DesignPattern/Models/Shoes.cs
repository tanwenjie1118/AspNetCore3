using System;

namespace Hal.DesignPattern.Models
{
    public class Shoes : Product
    {
        public string Name { set; get; }
        public override bool Use()
        {
            return true;
        }
    }
}
