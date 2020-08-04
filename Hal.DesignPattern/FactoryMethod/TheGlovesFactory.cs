using Hal.DesignPattern.Models;

namespace Hal.DesignPattern.FactoryMethod
{
    public class TheGlovesFactory : TheMethodFactory
    {
        public override Product GetProduct()
        {
            return new Gloves();
        }
    }
}
