using Hal.DesignPattern.Factory;
using Hal.DesignPattern.Models;
using Hal.DesignPattern.Prototype.Imp;
using Hal.DesignPattern.Singleton;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Tests.DesignPattern
{
    public class CreatePatternTest
    {
        [Fact]
        public void Singleton()
        {
            var nums = Enumerable.Range(1, 100);
            var role = TheMultiThreadSingleton.GetInstance();
            role.ShouldNotBeNull();
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
        /// </summary>
        [Fact]
        public void SimpleFactoryTest()
        {
            var product = SimpleFactory.GetProduct(typeof(Shoes));
            var result = product.Use();
            result.ShouldBeTrue();
        }
    }
}
