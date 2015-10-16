using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Xml.Schema;
using System.Reflection;

using SimpleBatch.Shared.Configuration;

namespace SimpleBatch.Shared
{
    public static class Context
    {
        public static BatchCycle BatchDefinition
        {
            set;
            get;
        }

        public static Dictionary<string, object> DataDictionary
        {
            get;
            set;
        }

        public static BatchCycle CreateBatchDefinition(string batchDefinitionXml)
        {
            BatchCycle batchDefinition = null;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(XmlSchema.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream(@"SimpleBatch.Shared.Configuration.Schema.SimpleBatch.xsd"), null));
                settings.ValidationType = ValidationType.Schema;

                // We will use the XmlReader in order to validate the XML against our XSD
                XmlReader xr = XmlReader.Create(new StringReader(batchDefinitionXml), settings);

                // we can now use this to create our batch definition
                XmlSerializer xs = new XmlSerializer(typeof(BatchCycle));

                batchDefinition = (BatchCycle)xs.Deserialize(xr);
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error reading batch definition: {1}", ex.Message));
                Environment.Exit(-1);
            }

            return batchDefinition;
        }
    }
}
