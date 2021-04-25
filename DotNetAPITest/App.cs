using System;
using System.Collections.Generic;
using ALCTests;
using CLParser;
using T27Tests;
using UTSTests;

using System.Linq;

using Common;
using IBMTests;
using VTTests;

namespace DotNetAPITest
{
    public partial class App : IApp
    {
        private bool _basicHelpRequested;
        private bool _dummy;

        string _emulationType;

        private Dictionary<string, OptionDescriptor> _appOptions;
        private Dictionary<string, CommandDescriptor> _commands;
        protected Dictionary<string, Type> _testRunnerFactories;


        #region Construction

        public App()
        {
            InitializeAppOptions();
            InitializeCommands();
            InitializeFactories();
        }

        #endregion Construction

        #region IApp

        public void RunApp(string[] CommandLine)
        {
            string command = string.Empty;
            int commandLineParams = 0;
            try
            {
                List<string> cl = CommandLine.ToList();
                commandLineParams = cl.Count;
                command = cl[0];
                if (!IsOption(command))
                {
                    cl.RemoveAt(0);
                }
                CommandLine = cl.ToArray();
                Parser parser = new Parser(_appOptions);
                string[] unprocessedParams = parser.ParseCommandLine(CommandLine);
                _commands[command.ToLower()].Command(unprocessedParams);
            }
            catch (ArgumentOutOfRangeException)
            {
                //  Command line was empty, just show help
                GeneralHelp();
            }
            catch
            {
                if (commandLineParams != 1 || !_basicHelpRequested)
                {
                    //  More than 1 CL param but we don't recognize the command, or only 1 and it wasn't '/?' so complain
                    Console.WriteLine($"\n Unknown Command '{command}'");
                }
                GeneralHelp();
            }
        }

        #endregion IApp

        void InitializeFactories()
        {
            _testRunnerFactories = new Dictionary<string, Type>();
            _testRunnerFactories.Add("IBM", typeof(IBMTestRunner));
            _testRunnerFactories.Add("VT", typeof(VTTestRunner));
            _testRunnerFactories.Add("ALC", typeof(ALCTestRunner));
            _testRunnerFactories.Add("T27", typeof(T27TestRunner));
            _testRunnerFactories.Add("UTS", typeof(UTSTestRunner));
        }

        void InitializeCommands()
        {
            CommandDescriptor cd;
            _commands = new Dictionary<string, CommandDescriptor>();

            cd = new CommandDescriptor(Command_Run, HelpForCommand_Run);
            _commands.Add("run", cd);

            cd = new CommandDescriptor(Command_Help, HelpForCommand_Help);
            _commands.Add("help", cd);

            cd = new CommandDescriptor(Command_Tests, HelpForCommand_Tests);
            _commands.Add("tests", cd);
        }

        void InitializeAppOptions()
        {
            _appOptions = new Dictionary<string, OptionDescriptor>();

            OptionDescriptor od = new OptionDescriptor(EmulationParser, HelpForOption_Emulation);
            _appOptions.Add("e", od);
            od = new OptionDescriptor(DummyOptionParser, HelpForOption_Dummy);
            _appOptions.Add("d", od);
            od = new OptionDescriptor(BasicHelpParser, HelpForOption_BasicHelp);
            _appOptions.Add("?", od);
        }

        #region Parsers

        OptionParser EmulationParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return EmulationParser;
                }
                _emulationType = null;
                return null;
            }

            _emulationType = Param.ToUpper();
            return null;
        }

        OptionParser BasicHelpParser(string Param)
        {
            _basicHelpRequested = true;
            return null;
        }

        OptionParser DummyOptionParser(string Param)
        {
            _dummy = true;
            return null;
        }

        #endregion Parsers

        #region Commands

        void Command_Help(string[] CommandLine)
        {
            if (string.IsNullOrEmpty(_emulationType))
            {
                try
                {
                    string item = CommandLine[0];
                    if (IsThereHelpForThisCommand(item))
                    {
                        return;
                    }
                    if (IsThereHelpForThisAppOption(item))
                    {
                        return;
                    }
                    if (IsThereHelpForThisCommonOption(item))
                    {
                        return;
                    }
                    Console.WriteLine($"\n No Help for item '{item}'");
                }
                catch
                {
                }
                GeneralHelp();
            }
            else
            {
                Command_Common(Commands.Help, CommandLine);
            }
        }

        void Command_Run(string[] CommandLine)
        {
            if (_dummy)
            {
                Console.Write("I'm sorry dave, I can't do that - HAL");
                return;
            }
            Command_Common(Commands.Run, CommandLine);
        }

        void Command_Tests(string[] CommandLine)
        {
            Command_Common(Commands.Tests, CommandLine);
        }

        void Command_Dummy(string[] CommandLine)
        {
            Console.WriteLine("\n Ya big dummy!");
        }

        void Command_Common(Commands Command, string[] CommandLine)
        {
            try
            {
                _testRunnerFactories.TryGetValue(_emulationType, out Type factory);
                TestRunner o = (TestRunner)Activator.CreateInstance(factory);
                o.RunCommand(Command, CommandLine);
            }
            catch (ArgumentNullException)
            {
                if (string.IsNullOrEmpty(_emulationType))
                {
                    Console.WriteLine("\n No emulation specified");
                }
                else
                {
                    Console.WriteLine($"\n Unknown emulation type: {_emulationType}");
                }
                GeneralHelp();
            }
            catch
            {
                Console.WriteLine("DAB1");
            }
        }

        #endregion Commands

        #region Helpers

        bool IsOption(string Param)
        {
            try
            {
                if (Param[0] == CLParser.Options._optionFlag0[0] || Param[0] == CLParser.Options._optionFlag1[0])
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        #endregion Helpers

        #region DeleteMe

        /*
        */

        #endregion
    }
}
