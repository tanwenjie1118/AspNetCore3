using System;

namespace Hal.DesignPattern.Singleton
{
    /// <summary>
    /// 单例模式的实现
    /// 此实现可用于单线程、多线程
    /// </summary>
    public class TheMultiThreadSingleton
    {
        // 定义一个静态变量来保存类的实例
        private static TheMultiThreadSingleton theSingleton;

        // 定义一个静态对象 作为锁的标识
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private TheMultiThreadSingleton()
        {
            InitializeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
        }

        public readonly string InitializeDateTime;

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static TheMultiThreadSingleton GetInstance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (theSingleton == null)
            {
                // 线程锁 没有抢到锁的线程挂起等待 上一个线程释放锁
                lock (locker)
                {
                    // 抢到锁之后 也要判断是否实体仍然不存在 则创建
                    if (theSingleton == null)
                    {
                        theSingleton = new TheMultiThreadSingleton();
                    }
                }
            }

            return theSingleton;
        }
    }
}
