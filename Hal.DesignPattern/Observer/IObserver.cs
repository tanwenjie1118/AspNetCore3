using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.DesignPattern.Observer
{
   public interface IObserver
    {
        void Receive(string message);
    }
}
