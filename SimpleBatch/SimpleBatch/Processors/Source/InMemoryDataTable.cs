using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data;

using SimpleBatch.Shared.Interfaces;
using SimpleBatch.Shared.Configuration;
using SimpleBatch.Shared;


namespace SimpleBatch.Processors.Source
{
    public class InMemoryDataTable : ISource<DataTable>
    {
        public object Configuration
        {
            get;
            set;
        }

        public DataTable Execute()
        {
            return (DataTable)Context.DataDictionary[((SimpleBatch.Shared.Configuration.InMemoryDataTable)Configuration).TableName];
        }
    }
}
