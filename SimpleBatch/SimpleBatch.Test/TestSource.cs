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
    public class TestSource : ISource<DataTable>
    {
        public object Configuration
        {
            get;
            set;
        }

        public DataTable Execute()
        {
            DataTable table = new DataTable("Cars");
            table.Columns.Add("Make");
            table.Columns.Add("Model");
            table.Columns.Add("Colour");
            table.Columns.Add("Value");

            DataRow row = table.NewRow();
            row["Make"] = "Ford";
            row["Model"] = "Escort";
            row["Colour"] = "Blue";
            row["Value"] = 5000;
            table.Rows.Add(row);

            row = table.NewRow();
            row["Make"] = "Ford";
            row["Model"] = "Fiesta";
            row["Colour"] = "Red";
            row["Value"] = 5500;
            table.Rows.Add(row);

            row = table.NewRow();
            row["Make"] = "Fiat";
            row["Model"] = "Punto Sport";
            row["Colour"] = "Black";
            row["Value"] = 1000;
            table.Rows.Add(row);

            row = table.NewRow();
            row["Make"] = "Ferrari";
            row["Model"] = "Spider";
            row["Colour"] = "Red";
            row["Value"] = 150000;
            table.Rows.Add(row);

            row = table.NewRow();
            row["Make"] = "BMW";
            row["Model"] = "Z4";
            row["Colour"] = "Blue";
            row["Value"] = 80000;
            table.Rows.Add(row);


            return table;
        }
    }
}
