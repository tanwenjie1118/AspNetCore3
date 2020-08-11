using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.DesignPattern.Observer
{
    public interface IPublisher
    {
        public void AddObserver(IObserver observer);
        public void RemoveObserver(IObserver observer);
        public bool Publish(string message);
    }
}
