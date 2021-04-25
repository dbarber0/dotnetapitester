using System;
using System.Collections.Generic;
using CLParser;

namespace Common
{
    public abstract class Test : ICommand
    {
        protected string _emulation;
        protected string _testName = "Unnamed";

        protected readonly Dictionary<string, OptionDescriptor> _options;
        protected readonly Dictionary<Commands, CommandDelegate> _commands;

        protected Test()
        {
            _options = new Dictionary<string, OptionDescriptor>();
            _commands = new Dictionary<Commands, CommandDelegate>();
            _commands.Add(Commands.Run, Command_Run);
            _commands.Add(Commands.Help, Command_Help);
        }

        #region ICommand

        public virtual void RunCommand(Commands Command, string[] Params)
        {
            try
            {
                ICLParser clparser = new Parser(_options);
                clparser.ParseCommandLine(Params);
                _commands[Command](Params);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"Test: Ooops - no command '{Command}'");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion ICommand

        protected abstract void Command_Run(string[] CommandLine);

        protected void Command_Help(string[] CommandLine)
        {
            if (CommandLine.Length == 0)
            {
                HelpOnTest();
            }
            else
            {
                HelpOnOption(CommandLine[0]);
            }
        }

        protected virtual void HelpOnTest()
        {
            Console.WriteLine($"Consider creating help for {_testName}");
        }

        protected virtual void HelpOnOption(string Option)
        {
            string t = Option;
            try
            {
                _options[t].Help(HelpType.Detailed, t);
            }
            catch
            {
                Console.WriteLine($"No help for option '{t}'");
            }
        }

        protected void HelpIsOnTheWay(HelpType Type, string Option)
        {
            Console.WriteLine($"Help for '{Option}' under construction");
        }

        protected void Pause()
        {
            Console.WriteLine("Hit 'Enter' to continue");
            Console.ReadLine();
        }

        protected void Pause(string Message)
        {
            Console.WriteLine(Message);
            Console.ReadLine();
        }
    }

    #region DeleteMe

    /*
    */

    #endregion

}