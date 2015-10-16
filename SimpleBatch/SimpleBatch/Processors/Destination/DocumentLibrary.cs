using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using Microsoft.SharePoint;

using SimpleBatch.Shared.Interfaces;

namespace SimpleBatch.Processors.Destination
{
    public class DocumentLibrary : IDestination<FileStream>
    {
        SimpleBatch.Shared.Configuration.DocumentLibrary _definition;

        public object Configuration
        {
            get;
            set;
        }

        public void Execute(FileStream data)
        {
            _definition = (SimpleBatch.Shared.Configuration.DocumentLibrary)Configuration;
            try
            {
                using (SPSite site = new SPSite(_definition.Site))
                {

                    SPWeb web = site.OpenWeb();

                    SPFolder library = web.Folders[_definition.LibraryName];

                    // We need to add the full URL for our new file                  
                    string URL = string.Format("{0}/{1}/{2}", site.Url, _definition.LibraryName, _definition.Filename);

                    // Upload document
                    SPFile spfile = library.Files.Add(URL, data, true);

                    // Commit 
                    library.Update();

                    Trace.TraceInformation(string.Format("File: {0} uploaded to document library: {1}", URL, _definition.LibraryName));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error uploading file to document library: {0}. Exception: {1}", _definition.LibraryName, ex.Message));
                Environment.Exit(-1);
            }
        }
    }
}
