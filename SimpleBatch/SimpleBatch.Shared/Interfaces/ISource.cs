using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleBatch.Shared.Configuration;

namespace SimpleBatch.Shared.Interfaces
{
    public interface ISource<T>
    {       
        T Execute();

        object Configuration
        {
            get;
            set;
        }
    }
}
