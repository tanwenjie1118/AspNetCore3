using Hal.DesignPattern.Builder.Parts;

namespace Hal.DesignPattern.Builder
{
    /// <summary>
    /// 建造者模式 核心在于定义建造者的抽象工作方法和抽象获取成果方式
    /// 调用建造者的人不关注建造者实现一项工作的细节，只需要给出【可用资源】【具体执行】以 达到【具体目的】
    /// 不同的建造者 (builder)的工作具体实现可以不同，但是结果是一致的。
    /// </summary>
    public abstract class AbstractBuilder
    {
        protected abstract void BuildHead();
        protected abstract void BuildBody();
        protected abstract void BuildFoot();
        public abstract AssembliedPart GetAssembliedParts();
    }
}
