using System.Threading;

namespace Hal.DesignPattern.TemplateMethod.Imp
{
    public class ThingsA : TheTemplateMethod
    {
        public override void Do()
        {
            Thread.Sleep(10000);
        }
    }
}
