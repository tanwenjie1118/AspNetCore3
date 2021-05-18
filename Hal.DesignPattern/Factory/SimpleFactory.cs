using Hal.DesignPattern.Models;
using System;

namespace Hal.DesignPattern.Factory
{
    /// <summary>
    /// 场景：
    /// 当工厂类负责创建的对象比较少时可以考虑使用简单工厂模式
    /// 客户如果只知道传入工厂类的参数，对于如何创建对象的逻辑不关心时可以考虑使用简单工厂模式
    /// 缺点：
    /// 工厂类集中了所有产品创建逻辑，一旦不能正常工作，整个系统都会受到影响
    /// 系统扩展困难，一旦添加新产品就不得不修改工厂逻辑，这样就会造成工厂逻辑过于复杂。
    /// 这就违反了SOLID原则中的 OCP Open Closed Principle 开闭原则
    /// </summary>
    public class SimpleFactory
    {
        private SimpleFactory()
        {
        }

        public static Product GetProduct(Type type)
        {
            if (typeof(Shoes) == type)
            {
                return new Shoes() {  };
            }
            else
            if (typeof(Knife) == type)
            {
                return new Knife();
            }
            else
            if (typeof(Gloves) == type)
            {
                return new Knife();
            }
            else
            {
                throw new InvalidOperationException("No such product");
            }
        }
    }
}
