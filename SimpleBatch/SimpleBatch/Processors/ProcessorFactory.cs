using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;

using SimpleBatch.Shared.Configuration;
using SimpleBatch.Shared.Interfaces;

using SimpleBatch.Interfaces;


namespace SimpleBatch.Processors
{
    internal static class ProcessorFactory
    {
        private static Dictionary<string, Assembly> _assemblyCache = new Dictionary<string, Assembly>();

        private enum CommandDirection
        {
            Source,
            Destination
        }

        internal static IDestination<T>[] CreateDestinations<T>(DestinationDefinition destinationDefinition)
        {
            List<IDestination<T>> destinations = new List<IDestination<T>>();
            foreach (Processor processorDefinition in destinationDefinition.Items)
            {
                IDestination<T> destination = CreateProcessor<IDestination<T>>(processorDefinition, CommandDirection.Destination);
                destination.Configuration = processorDefinition;

                destinations.Add(destination);
            }

            return destinations.ToArray();
        }

        internal static ISource<T> CreateSource<T>(SourceDefinition sourceDefinition)
        {
            ISource<T> source = CreateProcessor<ISource<T>>(sourceDefinition.Item, CommandDirection.Source);
            source.Configuration = sourceDefinition.Item;

            return source;
        }

        #region Baked-in Processors

        private static object LoadBakedInProcessor(Processor processor, CommandDirection direction)
        {
            string type = string.Format("SimpleBatch.Processors.{0}.{1}", direction.ToString(), processor.GetType().Name);

            return Activator.CreateInstance(Type.GetType(type));
        }

        #endregion

        #region Custom Processors

        private static object LoadCustomProcessor(CustomProcessor definition)
        {
            // Load assembly and create a new instance
            //
            Assembly assembly = GetAssemblyFromCache(definition.Assembly);
            if (assembly == null)
                assembly = GetCustomAssembly(Directory.GetCurrentDirectory(), definition.Assembly);

            return assembly.CreateInstance(definition.Type);
        }

        private static Assembly GetCustomAssembly(string currentDirectory, string assemblyName)
        {
            Assembly assembly = null;

            foreach (string file in Directory.GetFiles(currentDirectory))
            {

                if (Path.GetFileName(file) == assemblyName || Path.GetFileName(file) == assemblyName + ".dll")
                {
                    assembly = Assembly.LoadFile(file);
                    _assemblyCache.Add(assemblyName, assembly);
                }
            }

            if (assembly == null)
            {
                foreach (string directory in Directory.GetDirectories(currentDirectory))
                {
                    assembly = GetCustomAssembly(directory, assemblyName);
                    if (assembly != null)
                        break;
                }
            }

            return assembly;
        }

        private static Assembly GetAssemblyFromCache(string assemblyName)
        {
            if (_assemblyCache.ContainsKey(assemblyName))
                return _assemblyCache[assemblyName];

            return null;
        }

        #endregion

        #region Helpers

        private static Z CreateProcessor<Z>(Processor processor, CommandDirection direction)
        {
            Trace.TraceInformation(string.Format("Loading {0} processor: ({1}) {2}", direction.ToString().ToLower(), processor.GetType().Name, processor.Description));
            Trace.Indent();

            Z destination;

            if (processor is CustomProcessor)
                destination = CheckProcessorCompatibility<Z>(LoadCustomProcessor((CustomProcessor)processor));
            else
                destination = CheckProcessorCompatibility<Z>(LoadBakedInProcessor(processor, direction));

            Trace.Unindent();
            Trace.TraceInformation(string.Format("{0} processor loaded", direction.ToString()));

            return destination;
        }

        private static Z CheckProcessorCompatibility<Z>(object obj)
        {
            try
            {
                // Let's handle any possible miss-use of this custom processor
                //
                if (!typeof(Z).IsAssignableFrom(obj.GetType()))
                    throw new Exception(string.Format("This processor is not compatible with the command type of '{0}'", typeof(Z).Name));
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error loading processor: {0}", ex.Message));
                Environment.Exit(-1);
            }

            return (Z)obj;
        }

        #endregion
    }
}
