using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;

using SimpleBatch;


namespace SimpleBatch.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.TraceInformation("SimpleBatch V1.0");

            if (args.Length == 0)
            {
                Trace.TraceInformation("No batch definition provided... exiting.");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Trace.TraceInformation("Batch definition file does not exist... exiting.");
                return;
            }

            // Get the batch definition
            string batchDefinitionXml = File.ReadAllText(args[0]);

            if (batchDefinitionXml == string.Empty)
            {
                Trace.TraceInformation("Batch definition file is empty... exiting.");
                return;
            }

            // If a particular job name was supplied, only run that job
            //
            if (args.Length == 2)
            {
                if (args[1] != string.Empty)
                {
                    SimpleBatchEngine.Run(batchDefinitionXml, args[1]);
                    return;
                }            
            }
            
            // Run the batch
            SimpleBatchEngine.Run(batchDefinitionXml);
        }
    }
}
