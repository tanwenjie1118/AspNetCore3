using Hal.DesignPattern.Models;

namespace Hal.DesignPattern.AbstractFactory
{
    /// <summary>
    /// 抽象工厂 和 工厂方法的区别是：
    /// 抽象工厂解决的是横向的产品族，工厂方法解决的是纵向的产品等级
    /// 工厂方法 关注的是同一类产品的生产 比如 手机，各个厂商对手机的实现都不同
    /// 但是不应该在工厂方法定义中再 定义一个 电脑的 生产方法，因为 手机厂商不生产电脑，工厂种类应该细化分开。
    /// 而 抽象工厂 关注的是同一产品族的工厂，如小米，苹果，华为，三星 等拥有自己的产品族的工厂，是一个泛概念
    /// </summary>
    public abstract class TheAbstractFactory
    {
        /// <summary>
        /// 鞋类产品
        /// </summary>
        /// <returns></returns>
        public abstract Shoes GetShoes();

        /// <summary>
        /// 刀具产品
        /// </summary>
        /// <returns></returns>
        public abstract Knife GetKnife();

        /// <summary>
        /// 手套产品
        /// </summary>
        /// <returns></returns>
        public abstract Gloves GetGloves();
    }
}
