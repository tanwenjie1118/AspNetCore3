using System;

namespace Hal.DesignPattern.Models
{
    public class Knife : Product
    {
        public string Name { set; get; }
        public override bool Use()
        {
            return true;
        }
    }
}
