using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using SimpleBatch.Shared.Configuration;
using SimpleBatch.Interfaces;

namespace SimpleBatch.Commands
{
    internal static class CommandFactory
    {
        internal static ICommand Create(CommandDefinition cmdDefinition)
        {
            ICommand cmd = null;
            switch (cmdDefinition.Type)
            {
                case CommandDefinitionType.Data:
                    cmd = new Command<DataTable>(cmdDefinition);
                    break;

                default:
                    cmd = new Command<FileStream>(cmdDefinition);
                    break;
            }

            return cmd;
        }
    }
}
