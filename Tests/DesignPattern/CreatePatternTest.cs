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

        [Fact]
        public void Prototype()
        {
            var role = new Role(Guid.NewGuid().ToString("N"));
            var role1 = role.Clone();
            role.Id.ShouldBe(role1.Id);
        }
    }
}
