using Hal.DesignPattern.Models;

namespace Hal.DesignPattern.FactoryMethod
{
    /// <summary>
    /// 抽象工厂只定义每个子类统一的【生产方法】工厂方法模式 因此得名
    /// 由子工厂决定生产什么和怎么生产。把简单工厂中的生产逻辑分发
    /// 到各个子工厂中实现，这样就可以扩展新产品的工厂，而不需要影响别人工厂的使用。
    /// 优点：
    /// 工厂方法模式通过面向对象编程中的多态性来将对象的创建延迟到具体工厂中，从而解决了简单工厂模式中存在的问题，
    /// 也很好地符合了SOLID原则中的 OCP Open Closed Principle 开闭原则
    /// 缺点是：
    /// 实现多个子工厂，若过多则管理和使用比较复杂
    /// </summary>
    public abstract class TheMethodFactory
    {
        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <returns></returns>
        public abstract Product GetProduct();
    }
}
