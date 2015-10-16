using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleBatch.Shared.Configuration;

namespace SimpleBatch.Shared.Interfaces
{
    public interface IDestination<T>
    {
        void Execute(T data);

        object Configuration
        {
            get;
            set;
        }
    }
}
