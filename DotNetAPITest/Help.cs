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
            Console.WriteLine("");
            Console.WriteLine(" DESCRIPTION:\tExercise RIC Desktop's .NET API");
            Console.WriteLine(" USAGE:\t\tDotNetAPITest Command [Item ...], where:");
            Console.WriteLine("");
            Console.WriteLine(" Commands:");
            ListCommands();
            Console.WriteLine("");
            Console.WriteLine(" Options:");
            ListOptions();
            Console.WriteLine("");
            Console.WriteLine("   Run 'DotNetAPITest help optionspec' for details on passing options");
            Console.WriteLine("");
            Console.WriteLine(" Examples:");
            Console.WriteLine("   DotNetAPITest run /e ALC /s session.ialc /t 123456");
            Console.WriteLine("   DotNetAPITest help run");
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

            //  Include options that are maintained in the base TestRunner class
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
                Console.WriteLine("");
                Console.WriteLine(" DESCRIPTION:\tRun a test for a specified emulation and session");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest run -e <EmuType> -s <Session> /t <test> [<TestOption> ...]");
                Console.WriteLine("");
                Console.WriteLine(" EXAMPLE:\tDotNetAPITest run -e IBM -s session1.rd3x -t SomeIBMTest");
                Console.WriteLine("");
                Console.WriteLine("\t\tRun DotNetAPITest help -e for help on Emulation Types");
                Console.WriteLine("\t\tRun DotNetAPITest tests -e <EmuType> for a list of available tests for <EmuType>");
                Console.WriteLine("\t\tRun DotNetAPITest help -e <EmuType> /t <Test> for help on a specific Test, including options");
            }
            else
            {
                Console.WriteLine("   Run <EmuType> <Session> <Test> [<TestOption> ...]\t- Run a command/test specific to an emulation");
                Console.WriteLine("\t\t\t\t\t\t\t  using the specified session file");
            }
        }

        void HelpForCommand_Tests(HelpType Type, string Command)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(" DESCRIPTION:\tList Tests available for an emulation");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest tests -e <EmuType>");
                Console.WriteLine("");
                Console.WriteLine(" EXAMPLE:\tDotNetAPITest tests -e IBM");
                Console.WriteLine("");
                Console.WriteLine("\t\tRun DotNetAPITest help -e for help on Emulation Types");
                Console.WriteLine("\t\tRun DotNetAPITest help -e <EmuType> /t <Test> for help on a specific Test, including options");
            }
            else
            {
                Console.WriteLine("   Tests <EmuType>\t\t\t\t\t- List tests available for a specific emulation");
            }
        }

        void HelpForCommand_Help(HelpType Type, string Command)
        {
            if (Type == HelpType.Detailed)
            {
                GeneralHelp();
            }
            else
            {
                Console.WriteLine("   Help [<Command> | <Option>]\t\t\t\t- Without an optional parameter, this Help");
                Console.WriteLine("\t\t\t\t\t\t\t  otherwise, help on a specific command or option");
            }
        }

        void HelpForCommand_Dummy(HelpType Type, string Command)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(" Really, you need help doing something dumb?");
            }
            else
            {
                Console.WriteLine("   Dummy\t\t\t\t\t\t- Do something dumb");
            }
        }

        #endregion

        #region App Options Help

        public void HelpForOption_Emulation(HelpType Help, string Option)
        {
            if (Help == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(" DESCRIPTION:\tSpecify the emulation type for a command");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest <Command> -e <EmuType> [<Option> ...], where:");
                Console.WriteLine("");
                Console.WriteLine("\t\tCommand\t- The command to run using emulation type <EmuType>");
                Console.WriteLine("\t\tEmuType\t- One of ALC, T27, UTS, IBM, VT");
                Console.WriteLine("\t\tOption\t- An option for the test");
                Console.WriteLine("");
                Console.WriteLine("\t\tRun 'DotNetAPITest help' for available commands");
                Console.WriteLine("\t\tRun 'DotNetAPITest help /e <EmuType>' for options/tests specific to an <EmuType>");
                Console.WriteLine("\t\tRun 'DotNetAPITest help optionspec' for details on passing options");
                Console.WriteLine("");
                Console.WriteLine(" Examples:");
                Console.WriteLine("   DotNetAPITest help -e ALC\t\t\t\t\t- Basic help for ALC");
                Console.WriteLine("   DotNetAPITest run -e UTS /s=session1.iuts -t SomeTest\t- Run 'SomeTest' using a UTS session");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("   e <EmuType>\t\t- Emulation Type, where <EmuType> is one of ALC, T27, UTS, IBM, VT");
            }
        }

        public void HelpForOption_Dummy(HelpType Help, string Option)
        {
            if (Help == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(" DESCRIPTION:\tModify the dummy command");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest dummy -d");
                Console.WriteLine("");
                return;
            }
            Console.WriteLine("   d\t\t\t- When being a dummy, be a really big one");
        }

        #endregion App Options Help

        void MiscHelp_OptionSpec()
        {
            Console.WriteLine("");
            Console.WriteLine(" OptionSpec: <Flag><Option>[<Separator><Param>]");
            Console.WriteLine("   Flag\t\t- <'-' | '/'>");
            Console.WriteLine("   Option\t- Case-insensitive, see below");
            Console.WriteLine("   Separator\t- <'=' | ' '>");
            Console.WriteLine("   Param\t- Case-insensitive");
            Console.WriteLine("");
            Console.WriteLine(" Examples:");
            Console.WriteLine("   /e ALC");
            Console.WriteLine("   -s=session1.iuts");
        }

        #region Helpers

        bool ShowHelpForThisMiscHelpItem(string Item)
        {
            try
            {
                _miscHelpItems.TryGetValue(Item.ToLower(), out D help);
                help();
                return true;
            }
            catch (NullReferenceException)
            {
            }
            return false;
        }

        bool ShowHelpForThisCommonOption(string item)
        {
            TestRunnerBase b = new TestRunnerBase();
            if (b.HelpOnOption(item))
            {
                return true;
            }
            return false;
        }

        bool ShowHelpForThisAppOption(string Option)
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

        bool ShowHelpForThisCommand(string Command)
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