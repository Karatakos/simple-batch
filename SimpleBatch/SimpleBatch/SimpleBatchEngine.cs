using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SimpleBatch.Shared;
using SimpleBatch.Shared.Configuration;
using SimpleBatch.Interfaces;

namespace SimpleBatch
{
    public static class SimpleBatchEngine
    {
        public static void Run(string batchDefinitionXml)
        {
            Run(batchDefinitionXml, string.Empty);
        }

        public static void Run(string batchDefinitionXml, string job)
        {
            Context.BatchDefinition = Context.CreateBatchDefinition(batchDefinitionXml);
            Context.DataDictionary = new Dictionary<string, object>();

            Trace.TraceInformation("Processing batch");
            Trace.Indent();

            if (!String.IsNullOrEmpty(job))
            {
                JobDefinition jobDefinition = null;
                if (jobDefinition == null)
                {
                    Trace.TraceInformation(string.Format("Job: {0} not found in batch definition", job));
                    return;
                }

                foreach (JobDefinition jd in Context.BatchDefinition.Jobs)
                    if (jd.Name == job)
                        jobDefinition = jd;
            }
            else
            {
                // Execute every job in the batch
                //
                foreach (JobDefinition jobDefinition in Context.BatchDefinition.Jobs)
                    ProcessJob(jobDefinition);
            }

            Trace.Unindent();
            Trace.TraceInformation("Batch completed");
        }

        private static void ProcessJob(JobDefinition jobDefinition)
        {
            IBatchElement job = new Job(jobDefinition);
            job.Execute();
        }
    }
}
