using Hal.DesignPattern.Observer.Imp;
using Shouldly;
using Xunit;

namespace Tests.DesignPattern
{
    public class SpecialPatternTest
    {
        /// <summary>
        /// 观察者模式
        /// </summary>
        [Fact]
        public void Observer()
        {
            var message = "Do me a favor";
            Leader a = new Leader();
            Employee s = new Employee();
            Employee d = new Employee();
            Employee f = new Employee();
            a.AddObserver(s);
            a.AddObserver(d);
            a.AddObserver(f);

            a.Publish(message);

            s.CurrentOrder.ShouldBe(message);
            s.CurrentOrder.ShouldBe(message);
            s.CurrentOrder.ShouldBe(message);
        }
    }
}
