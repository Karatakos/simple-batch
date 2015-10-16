using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

using SimpleBatch.Shared.Configuration;
using SimpleBatch.Shared.Interfaces;
using SimpleBatch.Processors;
using SimpleBatch.Interfaces;


namespace SimpleBatch.Commands
{
    internal class Command<T> : ICommand
    {
        CommandDefinition _cmdDefinition;

        public Command(CommandDefinition cmdDefinition)
        {
            _cmdDefinition = cmdDefinition;
        }

        public void Execute()
        {
            ISource<T> source = null;
            IDestination<T>[] destinations = null;

            Trace.TraceInformation(string.Format("Processing {0} command: {1}", _cmdDefinition.Type.ToString().ToLower(), _cmdDefinition.Description));
            Trace.Indent();

            source = ProcessorFactory.CreateSource<T>(_cmdDefinition.SourceDefinition);
            destinations = ProcessorFactory.CreateDestinations<T>(_cmdDefinition.DestinationDefinition);                          

            T data = source.Execute();
            foreach (IDestination<T> destination in destinations)
                destination.Execute(data);

            Trace.Unindent();
            Trace.TraceInformation("Command complete");
        }
    }
}
