using Hal.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.DesignPattern.Builder.Parts
{
    public class AssembliedPart
    {
        private Head head;
        private Body body;
        private Foot foot;
        public void AddHead(Head head)
        {
            this.head = head;
        }

        public void AddBody(Body body)
        {
            this.body = body;
        }

        public void AddFoot(Foot foot)
        {
            this.foot = foot;
        }

        public bool HealthCheck()
        {
            if (head.IsNotNull() && body.IsNotNull() && foot.IsNotNull())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
