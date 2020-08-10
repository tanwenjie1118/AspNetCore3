using Hal.DesignPattern.Builder.Parts;

namespace Hal.DesignPattern.Builder.Builders
{
    public class TheBestBuilder : AbstractBuilder
    {
        private readonly AssembliedPart assemblied = new AssembliedPart();
        protected override void BuildBody()
        {
            var thebody = new Body();
            assemblied.AddBody(thebody);
        }

        protected override void BuildFoot()
        {
            var thefoot = new Foot();
            assemblied.AddFoot(thefoot);
        }

        protected override void BuildHead()
        {
            var thehead = new Head();
            assemblied.AddHead(thehead);
        }

        public override AssembliedPart GetAssembliedParts()
        {
            BuildHead();
            BuildBody();
            BuildFoot();
            return assemblied;
        }
    }
}
