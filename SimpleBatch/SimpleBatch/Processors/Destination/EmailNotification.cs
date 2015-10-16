using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Diagnostics;

using SimpleBatch.Shared;
using SimpleBatch.Shared.Interfaces;

namespace SimpleBatch.Processors.Destination
{
    public class EmailNotification : IDestination<FileStream>
    {
        SimpleBatch.Shared.Configuration.EmailNotification _definition;

        public object Configuration
        {
            get;
            set;
        }

        public void Execute(FileStream data)
        {
            using (StreamReader reader = new StreamReader(data))
            {
                _definition = (SimpleBatch.Shared.Configuration.EmailNotification)Configuration;
                try
                {
                    SmtpClient smtpClient = null;

                    // Setup the SMTP client
                    //                    
                    if (Context.BatchDefinition.Settings.SMTPServer.PortSpecified)
                        smtpClient = new SmtpClient(Context.BatchDefinition.Settings.SMTPServer.Address);
                    else
                        smtpClient = new SmtpClient(Context.BatchDefinition.Settings.SMTPServer.Address, Context.BatchDefinition.Settings.SMTPServer.Port);

                    // Send!
                    smtpClient.Send(new MailMessage(_definition.From, _definition.To, _definition.Subject, reader.ReadToEnd()));

                    Trace.TraceInformation("Email sent");
                }
                catch (Exception ex)
                {
                    Trace.TraceError(string.Format("Error sending mail to: {0}. Exception: {1}", _definition.To, ex.Message));
                    Environment.Exit(-1);
                }
            }

        }
    }
}
