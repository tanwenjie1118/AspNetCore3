using Hal.DesignPattern.AbstractFactory;
using Hal.DesignPattern.Factory;
using Hal.DesignPattern.FactoryMethod;
using Hal.DesignPattern.Models;
using Hal.DesignPattern.Prototype.Imp;
using Hal.DesignPattern.Singleton;
using Shouldly;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Tests.DesignPattern
{
    /// <summary>
    /// 创建类型设计模式
    /// </summary>
    public class CreatePatternTest
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        [Fact]
        public void Singleton()
        {
            var result = Parallel.For(0, 100, x =>
              {
                  var role = TheMultiThreadSingleton.GetInstance();
                  role.ShouldNotBeNull();
                  Debug.WriteLine(role.InitializeDateTime);
              });

            while (result.IsCompleted)
            {
                break;
            }
        }

        /// <summary>
        /// 原型模式
        /// </summary>
        [Fact]
        public void Prototype()
        {
            var role = new Role(Guid.NewGuid().ToString("N"));
            var role1 = role.Clone();
            role.Id.ShouldBe(role1.Id);
        }

        /// <summary>
        /// 简单工厂
        /// 内部集中所有的对象创建逻辑
        /// 耦合性高 不适合逻辑修改和功能扩展 违反了 开闭原则
        /// </summary>
        [Fact]
        public void SimpleFactoryTest()
        {
            var product = SimpleFactory.GetProduct(typeof(Shoes));
            var result = product.Use();
            result.ShouldBeTrue();
        }

        /// <summary>
        /// 工厂方法模式
        /// 只规定每个工厂的生产方法，具体的生产实现逻辑由各个具体的工厂来实现
        /// 符合开闭原则 但是 工厂过度
        /// </summary>
        [Fact]
        public void MethodFactoryTest()
        {
            TheMethodFactory factory = new TheGlovesFactory(); ;
            var product = factory.GetProduct();
            var result = product.Use();
            result.ShouldBeTrue();
        }

        /// <summary>
        /// 抽象工厂模式
        /// </summary>
        [Fact]
        public void AbstractFactoryTest()
        {
            TheAbstractFactory factory = new TheHunanFactory(); ;
            var product = factory.GetGloves();
            var result = product.Use();
            result.ShouldBeTrue();
        }
    }
}
