using Hal.DesignPattern.Models;

namespace Hal.DesignPattern.FactoryMethod
{
    public class TheShoesFactory : TheMethodFactory
    {
        public override Product GetProduct()
        {
            return new Shoes();
        }
    }
}
