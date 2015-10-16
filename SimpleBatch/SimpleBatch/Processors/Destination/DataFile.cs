using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Diagnostics;

using SimpleBatch.Shared.Interfaces;

namespace SimpleBatch.Processors.Destination
{
    public class DataFile : IDestination<DataTable>
    {
        SimpleBatch.Shared.Configuration.DataFile _defintion;

        public object Configuration
        {
            get;
            set;
        }

        public void Execute(DataTable data)
        {
            _defintion = (SimpleBatch.Shared.Configuration.DataFile)Configuration;
            try
            {
                if (_defintion.Format == SimpleBatch.Shared.Configuration.DataFileFormat.Xml)
                {
                    data.WriteXml(_defintion.FileName);
                    return;
                }

                using (StreamWriter sw = new StreamWriter(File.Create(_defintion.FileName)))
                {
                    StringBuilder builder = new StringBuilder();
                    if (!_defintion.WithHeadersSpecified || (_defintion.WithHeadersSpecified && _defintion.WithHeaders == true))
                    {
                        foreach (DataColumn column in data.Columns)
                            AppendValueToString(builder, column.ColumnName);

                        WriteLineToStream(sw, builder);
                    }

                    foreach (DataRow row in data.Rows)
                    {
                        foreach (DataColumn column in data.Columns)
                            AppendValueToString(builder, row[column.ColumnName]);

                        WriteLineToStream(sw, builder);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error writing to the file: {0}. Exception: {1}", _defintion.FileName, ex.Message));
                Environment.Exit(-1);
            }
        }

        private void AppendValueToString(StringBuilder builder, object value)
        {
            builder.Append(string.Format("{0}{1}", CleanValue(value.ToString()), _defintion.FieldDelimiter));
        }

        private void WriteLineToStream(StreamWriter sw, StringBuilder builder)
        {
            sw.WriteLine(builder.ToString().TrimEnd(_defintion.FieldDelimiter.ToCharArray()));

            builder.Clear();
        }

        private string CleanValue(string value)
        {
            if (value.Contains(_defintion.FieldDelimiter))
                return value.Replace(_defintion.FieldDelimiter, string.Empty);

            return value;
        }


    }
}
