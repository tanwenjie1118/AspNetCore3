using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.DesignPattern.Observer.Imp
{
    public class Leader : IPublisher
    {
        private List<IObserver> observers = new List<IObserver>();
        public void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }
        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public bool Publish(string message)
        {
            try
            {
                foreach (var observer in observers)
                {
                    observer.Receive(message);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
