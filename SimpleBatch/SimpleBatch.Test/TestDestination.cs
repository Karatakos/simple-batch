using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using SimpleBatch.Shared.Interfaces;
using SimpleBatch.Shared;


namespace SimpleBatch.Test
{
    public class TestDestination : IDestination<DataTable>
    {
        public object Configuration
        {
            get;
            set;
        }

        public void Execute(DataTable data)
        {                       
        }
    }
}
