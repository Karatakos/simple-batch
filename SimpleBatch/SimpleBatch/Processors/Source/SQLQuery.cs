using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

using SimpleBatch.Shared.Interfaces;


namespace SimpleBatch.Processors.Source
{
    public class SQLQuery : ISource<DataTable>
    {
        private SimpleBatch.Shared.Configuration.SQLQuery _definition;

        public object Configuration
        {
            get;
            set;
        }

        public DataTable Execute()
        {
            DataTable dataTable = new DataTable();
            string sql = string.Empty;

            _definition = (SimpleBatch.Shared.Configuration.SQLQuery)Configuration;
            try
            {
                Trace.TraceInformation(string.Format("Opening connection to database: {0}", _definition.DataSource));
                using (SqlConnection conn = new SqlConnection(_definition.DataSource))
                {
                    conn.Open();

                    if (_definition.Item is SimpleBatch.Shared.Configuration.SQLQuerySQLFile)
                    {
                        // Process query from file
                        SimpleBatch.Shared.Configuration.SQLQuerySQLFile fileDefinition = (SimpleBatch.Shared.Configuration.SQLQuerySQLFile)_definition.Item;

                        if (!File.Exists(fileDefinition.Name))
                            throw new Exception(string.Format("The provided SQL file '{0}' does not exist", fileDefinition.Name));

                        sql = File.ReadAllText(fileDefinition.Name);
                    }
                    else
                    {
                        // Process query as text                                               
                        sql = _definition.Item.ToString();

                        sql = sql.Replace('\n', ' ');
                        sql = sql.Replace('\t', ' ');
                        sql = sql.Trim();
                    }

                    Trace.TraceInformation(string.Format("Executing query: {0}", sql));

                    // Execute query and fill a datatable with results
                    //
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    dataTable.Load(cmd.ExecuteReader());
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error executing query: {0}\n {1}", sql, ex.Message));
                Environment.Exit(-1);
            }

            return dataTable;
        }
    }
}
