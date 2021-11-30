using System;
using System.Linq;
using CLParser;
using Common;

namespace DotNetAPITest
{
    public partial class App
    {
        private void GeneralHelp()
        {
            //  Not Major, not Captain, General
            Console.WriteLine("");
            Console.WriteLine(" DESCRIPTION:\tExercise RIC Desktop's .NET API");
            Console.WriteLine(" USAGE:\t\tDotNetAPITest Command [Item ...], where:");
            Console.WriteLine("");
            Console.WriteLine(" Command:");
            ListCommands();
            Console.WriteLine("");
            Console.WriteLine(" OptionSpec: <Flag><Option>[<Separator><Param>]");
            Console.WriteLine("\tFlag\t\t- <'-' | '/'>");
            Console.WriteLine("\tOption\t\t- Case-insensitive, see below");
            Console.WriteLine("\tSeparator\t- <'=' | ' '>");
            Console.WriteLine("\tParam\t\t- Case-insensitive");
            Console.WriteLine("");
            Console.WriteLine(" Option:");
            ListOptions();
            //Console.WriteLine("\ts <SessionFile>\t- Session file to use. Must specify an emulation with /e <Type>");
            Console.WriteLine("");
            Console.WriteLine(" HelpItem:");
            Console.WriteLine("\tAn option or command");
            Console.WriteLine("");
            Console.WriteLine(" e.g., DotNetAPITest run /e ALC /s session.ialc /t 123456");
            Console.WriteLine("       DotNetAPITest help run");
            Console.WriteLine("");
        }

        void ListOptions()
        {
            var keys = _appOptions.Keys.ToList();
            keys.Sort();
            foreach (var option in keys)
            {
                try
                {
                    _appOptions[option].Help(HelpType.Basic, option);
                }
                catch
                {

                }
            }

            TestRunnerBase trb = new TestRunnerBase();
            trb.ListCommonOptions();
        }

        void ListCommands()
        {
            var keys = _commands.Keys.ToList();
            keys.Sort();
            foreach (var command in keys)
            {
                try
                {
                    _commands[command].Help(HelpType.Basic, command);
                }
                catch
                {
                }
            }
        }

        #region Command Help

        void HelpForCommand_Run(HelpType Type, string Command)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("Detailed help on command 'Run' under construction");
                return;
            }
            Console.WriteLine("\tRun <EmuOption> <TestOption> [<OptionSpec> ...]\t- Run a command/test specific to an emulation");
        }

        void HelpForCommand_Tests(HelpType Type, string Command)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("Detailed help on command 'Tests' under construction");
                return;
            }
            Console.WriteLine("\tTests <EmuOption>\t\t\t\t- List tests available for a specific emulation");
        }

        void HelpForCommand_Help(HelpType Type, string Command)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("Detailed help on command 'Help' under construction");
                return;
            }
            Console.WriteLine("\tHelp [<Command> | <Option>]\t\t\t- Without an optional parameter, general Help");
            Console.WriteLine("\t\t\t\t\t\t\t  otherwise, help on a specific command or option");
        }

        #endregion

        #region App Options Help

        public void HelpForOption_Emulation(HelpType Help, string Option)
        {
            if (Help == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(
                    " DESCRIPTION:\tFor a specified emulation type, run tests, display type-specific help, etc.");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest run -e <Type> [<OptionSpec> ...], where:");
                Console.WriteLine("");
                Console.WriteLine("\t\t<Type>\t\t- One of ALC, T27, UTS, IBM, VT");
                Console.WriteLine("\t\t\t\t  With neither <EmuOptionSpec>[s] nor -emuhelp");
                Console.WriteLine("\t\t\t\t  specified, shows basic help for <Type>");
                Console.WriteLine("\t\tOptionSpec\t- <Flag><Option>[<Separator><Param>]");
                Console.WriteLine("\t\t\t\t  Run 'DotNetAPITest -?' for details on 'OptionSpec'");
                Console.WriteLine("\t\tOption\t\t- An option specific to <Type>");
                Console.WriteLine("");
                Console.WriteLine("\t\te.g.:");
                Console.WriteLine("");
                Console.WriteLine("\t\tDotNetAPITest run -e ALC\t\t- Basic help for ALC");
                Console.WriteLine("\t\tDotNetAPITest run -e ALC -t <SomeTest>");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("\te <Type>\t- Emulation Type, where <Type> is one of ALC, T27, UTS, IBM, VT");
            }
        }

        void HelpForOption_BasicHelp(HelpType Type, string Command)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("Detailed help on command '?' under construction");
                return;
            }
            Console.WriteLine("\t?\t\t- Solo, general Help, otherwise ignored");
        }

        public void HelpForOption_Dummy(HelpType Help, string Option)
        {
            if (Help == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(" DESCRIPTION:\tTest Dummy option");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest run -d");
                Console.WriteLine("");
                return;
            }
            Console.WriteLine("\td\t\t- Do something dumb");
        }

        #endregion App Options Help

        #region Helpers

        bool IsThereHelpForThisCommonOption(string item)
        {
            TestRunnerBase b = new TestRunnerBase();
            if (b.HelpOnOption(item))
            {
                return true;
            }
            return false;
        }

        bool IsThereHelpForThisAppOption(string Option)
        {
            try
            {
                _appOptions.TryGetValue(Option, out OptionDescriptor od);
                od.Help(HelpType.Detailed, Option);
                return true;
            }
            catch (NullReferenceException)
            {
            }
            return false;
        }

        bool IsThereHelpForThisCommand(string Command)
        {
            try
            {
                _commands.TryGetValue(Command, out CommandDescriptor cd);
                cd.Help(HelpType.Detailed, Command);
                return true;
            }
            catch (NullReferenceException)
            {
            }
            return false;
        }

        #endregion Helpers

    }
}