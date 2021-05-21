using Hal.DesignPattern.Models;

namespace Hal.DesignPattern.AbstractFactory
{
    public class TheJiangXiFactory : TheAbstractFactory
    {
        public override Gloves GetGloves()
        {
            return new Gloves();
        }

        public override Knife GetKnife()
        {
            return new Knife();
        }

        public override Shoes GetShoes()
        {
            return new Shoes();
        }
    }
}
