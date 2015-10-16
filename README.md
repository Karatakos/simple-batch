# simplebatch
XML based batch job management framework enabling the definition of batch jobs in a basic XML format.

A basic selection of data source and destination processors baked in, such as;

- File (Text, CSV, XML)
- Email
- SQL (SQL Server Client)
- SharePoint (Document Library)
- Also supports custom data sources.

You can start a batch either from the command line using the provided console wrapper or by referencing the simple batch engine and triggering a batch directly from your application. The best way to start writing batches using Simple Batch is to use the supplied XSD that ships with the binaries. This ensures your XML is validated on the fly, XML editors such as Altova XML Spy also provide great features such as intellisense which will be helpful as all the baked-in processors (listed above) have been defined in the XSD.
