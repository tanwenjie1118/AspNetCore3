using System;
using System.Collections.Generic;

namespace Hal.DesignPattern.Builder
{
    /// <summary>
    /// 指挥者 用于监监督 建造者的工作
    /// </summary>
    public class Director
    {
        private readonly List<AbstractBuilder> builders = new List<AbstractBuilder>();
        public void Run()
        {
            Console.WriteLine("start");
            foreach (var builder in builders)
            {
                var part = builder.GetAssembliedParts();
                if (part.HealthCheck())
                {
                    Console.WriteLine("build succeed");
                }
                else
                {
                    Console.WriteLine("build failed");
                }
            }

            Console.WriteLine("finish");
        }

        public void AddBuilder(AbstractBuilder builder)
        {
            builders.Add(builder);
        }
    }
}
