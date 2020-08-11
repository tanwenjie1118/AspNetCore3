using Hal.DesignPattern.Observer.Imp;
using Shouldly;
using Xunit;

namespace Tests.DesignPattern
{
    public class SpecialPatternTest
    {
        [Fact]
        public void Observer()
        {
            var order = "Do me a favor";
            Leader a = new Leader();
            Employee s = new Employee();
            Employee d = new Employee();
            Employee f = new Employee();
            a.AddObserver(s);
            a.AddObserver(d);
            a.AddObserver(f);

            a.Publish(order);

            s.CurrentOrder.ShouldBe(order);
            s.CurrentOrder.ShouldBe(order);
            s.CurrentOrder.ShouldBe(order);
        }
    }
}
