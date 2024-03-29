﻿using System.Collections.Generic;
using CLParser;

namespace Common
{
    public delegate void CommandDelegate(string[] Params);

    public class CommandDescriptor
    {
        //public delegate void CommandDelegate(List<string> CommandLine);
        public delegate void CommandHelp(HelpType Type, string Command);

        public CommandDescriptor(CommandDelegate Command, CommandHelp Help)
        {
            this.Command = Command;
            this.Help = Help;
        }
        public CommandDelegate Command { get; set; }
        public CommandHelp Help { get; set; }
    }

    public interface ICommand
    {
        void RunCommand(Commands Command, string[] Params);
    }

    public enum Commands
    {
        None,
        Help,
        Run,
        Tests,
        Describe
    }

}