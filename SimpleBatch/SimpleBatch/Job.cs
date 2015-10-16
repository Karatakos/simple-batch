using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SimpleBatch.Shared.Configuration;
using SimpleBatch.Interfaces;
using SimpleBatch.Commands;

namespace SimpleBatch
{
    internal class Job : IBatchElement
    {
        private JobDefinition _jobDefinition;

        internal Job(JobDefinition jobDefinition)
        {
            _jobDefinition = jobDefinition;
        }

        public void Execute()
        {
            Trace.TraceInformation(String.Format("Executing job: {0} ({1})", _jobDefinition.Name, _jobDefinition.Description));
            Trace.Indent();

            foreach (CommandDefinition cmdDefinition in _jobDefinition.Commands)
            {
                ICommand cmd = CommandFactory.Create(cmdDefinition);
                cmd.Execute();
            }

            Trace.Unindent();
            Trace.TraceInformation("Job completed");
        }
    }
}
