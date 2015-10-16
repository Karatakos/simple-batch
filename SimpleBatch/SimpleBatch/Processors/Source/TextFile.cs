using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using SimpleBatch.Shared.Interfaces;

namespace SimpleBatch.Processors.Source
{
    public class TextFile : ISource<FileStream>
    {
        private SimpleBatch.Shared.Configuration.TextFile _definition;

        public object Configuration
        {
            get;
            set;
        }

        public FileStream Execute()
        {
            _definition = (SimpleBatch.Shared.Configuration.TextFile)Configuration;

            FileStream fs = null;
            try
            {
                fs = File.OpenRead(_definition.FileName);
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error reading file: {0}. Exception: {1}", _definition.FileName, ex.Message));
                Environment.Exit(-1);
            }

            return fs;
        }
    }
}
