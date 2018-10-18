using System;
using System.Collections.Generic;
using System.Text;

namespace BarcampBot.IoC.Interfaces
{
    public interface IServiceLocator : IServiceProvider
    {
        object GetService(string name);
    }
}
