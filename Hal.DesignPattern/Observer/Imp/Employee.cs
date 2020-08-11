using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.DesignPattern.Observer.Imp
{
    public class Employee : IObserver
    {
        public string CurrentOrder { set; get; }
        public void Receive(string message)
        {
            CurrentOrder = message;
        }
    }
}
