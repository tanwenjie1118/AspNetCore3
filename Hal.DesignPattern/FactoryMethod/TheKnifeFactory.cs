using Hal.DesignPattern.Models;

namespace Hal.DesignPattern.FactoryMethod
{
    public class TheKnifeFactory : TheMethodFactory
    {
        public override Product GetProduct()
        {
            return new Knife();
        }
    }
}
