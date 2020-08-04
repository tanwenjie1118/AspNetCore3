using System;

namespace Hal.DesignPattern.Singleton
{
    /// <summary>
    /// 单例模式的实现
    /// 每个线程都有自己的线程栈 因此
    /// 此实现只能用于单线程 因为每个线程都拥有自己的全局唯一静态对象
    /// </summary>
    public class TheSingleThreadSingleton
    {
        // 定义一个静态变量来保存类的实例
        private static TheSingleThreadSingleton theSingleton;
        // 定义私有构造函数，使外界不能创建该类实例
        private TheSingleThreadSingleton()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static TheSingleThreadSingleton GetInstance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (theSingleton == null)
            {
                theSingleton = new TheSingleThreadSingleton();
            }

            return theSingleton;
        }
    }
}
