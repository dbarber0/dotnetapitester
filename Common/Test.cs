using System;
using System.Collections.Generic;
using CLParser;

namespace Common
{
    public abstract class Test : ICommand
    {
        protected string _emulation;
        protected string _testName = "Unnamed";

        protected readonly Dictionary<string, OptionDescriptor> _parsers;
        protected readonly Dictionary<Commands, CommandDelegate> _commands;

        protected Test()
        {
            _parsers = new Dictionary<string, OptionDescriptor>();
            _commands = new Dictionary<Commands, CommandDelegate>();
            _commands.Add(Commands.Run, Command_Run);
            _commands.Add(Commands.Help, Command_Help);
        }

        #region ICommand

        public virtual void Run(Commands Command, string[] Params)
        {
            try
            {
                ICLParser clparser = new Parser(_parsers);
                clparser.ParseCommandLine(Params);
                _commands[Command](Params);
            }
            catch
            {
                Console.WriteLine($"Test: Ooops - no command '{Command}'");
            }
        }

        #endregion

        protected abstract void Command_Run(string[] CommandLine);

        protected virtual void Command_Help(string[] CommandLine)
        {
            Console.WriteLine($"\n Generic Help - Consider creating help for {_testName}");
        }

        /*
        public virtual void Run(string[] Params)
        {
            ICLParser clparser = new Parser(_parsers);
            clparser.ParseCommandLine(Params);
        }
        */
    }

    #region DeleteMe

    #endregion

}