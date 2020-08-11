using System;

namespace Hal.DesignPattern.TemplateMethod
{
    /// <summary>
    /// 是把相同的部分抽象出来到抽象类中去定义，具体子类来实现具体的不同部分，这个思路也正式模板方法的实现精髓所在
    /// </summary>
    public abstract class TheTemplateMethod
    {
        private readonly Action action;

        public ProcessStatus CurrentStatus { get; private set; } = ProcessStatus.UnStart;

        public TheTemplateMethod()
        {
            action += ThingsStart;
            action += Do;
            action += ThingsDone;
        }

        public void DoThings()
        {
            action();
        }

        /// <summary>
        /// 模板方法，不要把模版方法定义为Virtual或abstract方法，避免被子类重写，防止更改流程的执行顺序
        /// </summary>
        private void ThingsStart()
        {
            CurrentStatus = ProcessStatus.Started;
        }

        private void ThingsDone()
        {
            CurrentStatus = ProcessStatus.Finished;
        }

        public abstract void Do();
    }
}
