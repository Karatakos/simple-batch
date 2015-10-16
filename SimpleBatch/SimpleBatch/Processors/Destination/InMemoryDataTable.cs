using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SimpleBatch.Shared.Interfaces;
using SimpleBatch.Shared;

namespace SimpleBatch.Processors.Destination
{
    public class InMemoryDataTable : IDestination<DataTable>
    {
        public object Configuration
        {
            get;
            set;
        }

        public void Execute(DataTable data)
        {
            Context.DataDictionary.Add(((SimpleBatch.Shared.Configuration.InMemoryDataTable)Configuration).TableName, data);
        }
    }
}
