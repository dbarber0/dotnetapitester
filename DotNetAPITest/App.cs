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
    public class App : IApp
    {
        string _emulationType;
        //private string[] _params;
        //private string _command;
        //private string[] _unprocessedParams;

        private readonly Dictionary<string, OptionDescriptor> _parsers;
        private readonly Dictionary<string, CommandDescriptor> _commands;
        //private int _optionCount;
        //private OptionHelp _help;
        protected readonly Dictionary<string, Type> _testRunnerFactories1;


        #region Construction

        public App()
        {
            CommandDescriptor cd;
            _parsers = new Dictionary<string, OptionDescriptor>();

            OptionDescriptor od = new OptionDescriptor(EmulationParser, null);
            _parsers.Add("e", od);

            _commands = new Dictionary<string, CommandDescriptor>();
            cd = new CommandDescriptor(HelpCommand, ShowHelp);
            _commands.Add("help", cd);

            cd = new CommandDescriptor(BasicHelpCommand, ShowHelp);
            _commands.Add("?", cd);

            cd = new CommandDescriptor(DummyCommand, null);
            _commands.Add("dummy", cd);


            cd = new CommandDescriptor(RunCommand, HelpOnCommand_Run);
            _commands.Add("run", cd);

            //cd = new CommandDescriptor(TestsCommand, HelpOnCommand_Tests);
            //_commands.Add("tests", cd);

            _testRunnerFactories1 = new Dictionary<string, Type>();
            _testRunnerFactories1.Add("IBM", typeof(IBMTestRunner));
            _testRunnerFactories1.Add("VT", typeof(VTTestRunner));
            _testRunnerFactories1.Add("ALC", typeof(ALCTestRunner));
            _testRunnerFactories1.Add("T27", typeof(T27TestRunner));
            _testRunnerFactories1.Add("UTS", typeof(UTSTestRunner));
        }

        #endregion Construction

        #region IApp

        public void Run(string[] CommandLine)
        {
            string command = string.Empty;
            List<string> cl = null;
            try
            {
                cl = CommandLine.ToList();
                command = cl[0];
                cl.RemoveAt(0);
                CommandLine = cl.ToArray();

                Parser parser = new Parser(_parsers);
                string[] unprocessedParams = parser.ParseCommandLine(CommandLine);

                _commands[command].Command(unprocessedParams);
            }
            catch (ArgumentOutOfRangeException)
            {
                ShowHelp();
            }
            catch
            {
                Console.WriteLine($"\n Unknown Command '{command}'");
                ShowHelp();
            }
        }

        #endregion IApp

        #region Parsers

        void HelpCommand(string[] /*List<string>*/ CommandLine)
        {
            if (string.IsNullOrEmpty(_emulationType))
            {
                string command = string.Empty;
                CommandDescriptor cd = null;
                try
                {
                    command = CommandLine[0];
                    _commands.TryGetValue(command, out cd);
                    cd.Help();
                }
                catch (NullReferenceException)
                {
                    if (cd == null)
                    {
                        Console.WriteLine($"\n Unknown command '{command}'");
                    }
                    else
                    {
                        Console.WriteLine($"\n No Help for command '{command}'");
                    }
                    ShowHelp();
                }
                catch /*(ArgumentOutOfRangeException)*/
                {
                    ShowHelp();
                }
            }
            else
            {
                CommonCommand(Commands.Help, CommandLine);
            }
        }

        void RunCommand(string[] /*List<string>*/ CommandLine)
        {
            CommonCommand(Commands.Run, CommandLine);
        }

        void BasicHelpCommand(string[] /*List<string>*/ CommandLine)
        {
            ShowHelp();
        }

        void DummyCommand(string[] /*List<string>*/ CommandLine)
        {
            Console.WriteLine("\n Ya big dummy!");
        }

        /*
        void TestsCommand(List<string> CommandLine)
        {
            CommonCommand(Commands.Tests, CommandLine);
        }

        void DescribeCommand(List<string> CommandLine)
        {
            CommonCommand(Commands.Describe, CommandLine);
        }
        */

        void ParseCommandLine()
        {

        }
        void CommonCommand(Commands Command, string[] /*List<string>*/ CommandLine)
        {
            /*
            Parser parser = new Parser(_parsers);
            string[] unprocessedParams = parser.ParseCommandLine(CommandLine);
            */

            try
            {
                Type factory;
                _testRunnerFactories1.TryGetValue(_emulationType, out factory);
                TestRunner o = (TestRunner)Activator.CreateInstance(factory);
                o.Run(Command, CommandLine);
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
                ShowHelp();
            }
            catch
            {
                Console.WriteLine("DAB1");
            }
        }
        void HelpOnCommand_Run()
        {
            Console.WriteLine("'Run' Command Help");
        }

        /*
        void HelpOnCommand_Tests()
        {
            Console.WriteLine(" 'Tests' Command Help");
        }

        void HelpOnCommand_Describe()
        {
            Console.WriteLine(" 'Describe' Command Help");
        }
        */

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
            //_optionCount++;
            return null;
        }

        #endregion Parsers

        #region Help

        private void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine(" DESCRIPTION:\tExercise RIC Desktop's .NET API");
            Console.WriteLine(" USAGE:\t\tDotNetAPITest command [<OptionSpec> ...], where:");
            Console.WriteLine("");
            Console.WriteLine("\t\tUnder Construction");
            /*
            Console.WriteLine("\t\tOptionSpec\t- <Flag><Option>[<Separator><Param>]");
            Console.WriteLine("\t\tFlag\t\t- <'-' | '/'>");
            Console.WriteLine("\t\tOption\t\t- Case-insensitive, see below");
            Console.WriteLine("\t\tSeparator\t- <'=' | ' '>");
            Console.WriteLine("\t\tParam\t\t- Case-insensitive");
            Console.WriteLine("");
            Console.WriteLine("\t\tOption/Param:");
            Console.WriteLine("");
            Console.WriteLine("\t\t?\t\t- Basic help, same as running with no options");
            Console.WriteLine("\t\thelp\t\t");
            Console.WriteLine("\t\thelp <Option>\t- Detailed help on <Option>");
            Console.WriteLine("\t\te <Type>\t- Emulation Type, where <Type> is one of ALC, T27, UTS, IBM, VT");
            Console.WriteLine("");
            */
        }


        #endregion Help

        #region DeleteMe

        /*
        bool HelpRequested()
        {
            if (_optionCount == 0)
            {
                ShowHelp();
                return true;
            }
            if (_help != null && _optionCount == 1)
            {
                _help();
                return true;
            }
            return false;
        }

        OptionParser BasicHelpParser(string Param)
        {
            _help = ShowHelp;
            _optionCount++;
            return null;
        }

        private void HelpOnOption_Emulation()
        {
            Help.EmulationOption();
        }

        OptionParser DetailedHelpParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return DetailedHelpParser;
                }

                _help = ShowHelp;
                _optionCount++;
                return null;
            }

            if (_parsers.ContainsKey(Param))
            {
                _parsers.TryGetValue(Param, out OptionDescriptor od);
                _help = od?.Help;
            }
            else
            {
                Console.WriteLine($"No Help found for option {Param}");
                _help = ShowHelp;
            }

            _optionCount++;
            return null;
        }


        
        
            ICLParser clparser = new Parser(_parsers);
            string[] unprocessedParams = clparser.ParseCommandLine(_params);

            if (HelpRequested())
            {
                return;
            }

            Type factory;
            if (!_testRunnerFactories1.TryGetValue(_emulationType, out factory))
            {
                Console.WriteLine($"Unknown emulation type: {_emulationType}");
                return;
            }
            TestRunner o = (TestRunner)Activator.CreateInstance(factory);
            o.Run(unprocessedParams);
        */

        #endregion
    }
}
