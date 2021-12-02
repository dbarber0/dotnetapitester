using System;
using System.Collections.Generic;
using System.Linq;
using CLParser;

namespace Common
{
    public class TestRunnerBase
    {
        protected string _session;
        protected string _testName;

        protected Dictionary<string, OptionDescriptor> _commonOptions;

        public TestRunnerBase()
        {
            InitializeCommonOptions();
        }

        void InitializeCommonOptions()
        {
            _commonOptions = new Dictionary<string, OptionDescriptor>();
            OptionDescriptor od = new OptionDescriptor(SessionFileParser, HelpOnOption_SessionFile);
            _commonOptions.Add("s", od);

            od = new OptionDescriptor(TestParser, HelpOnOption_Test);
            _commonOptions.Add("test", od);
            _commonOptions.Add("t", od);
        }

        #region Parsers

        private OptionParser SessionFileParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return SessionFileParser;
                }
                Console.WriteLine("Ignoring -s option - no session file specified");
                return null;
            }

            _session = Param;
            return null;
        }

        OptionParser TestParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return TestParser;
                }
                Console.WriteLine("No test was specified");
                return null;
            }

            _testName = Param.ToUpper();
            return null;
        }

        #endregion

        #region Helpers

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

        #endregion

        #region Help

        void HelpOnOption_SessionFile(HelpType Type, string Option)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(" DESCRIPTION:\tSpecify the Session file to use for a command");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest <Command> -e <EmuType> -s <SessionFile> [<Option> ...], where:");
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
                Console.WriteLine("   DotNetAPITest run -e ALC -s Session1.ialc /t SomeTest\t- Run 'SomeTest' using 'Session1.ialc'");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("   s <SessionFile>\t- Session file to use. Must specify an emulation with /e <EmuType>");
            }
        }

        void HelpOnOption_Test(HelpType Type, string Option)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("");
                Console.WriteLine(" DESCRIPTION:\tSpecify the Test to run");
                Console.WriteLine(" USAGE:\t\tDotNetAPITest run -e <EmuType> -s <SessionFile> -t <Test> [<Option> ...], where:");
                Console.WriteLine("");
                Console.WriteLine("\t\tEmuType\t\t- One of ALC, T27, UTS, IBM, VT");
                Console.WriteLine("\t\tSessionFile\t- The session to use when runnuing the specified test");
                Console.WriteLine("\t\tOption\t\t- An option for the test");
                Console.WriteLine("");
                Console.WriteLine("\t\tRun 'DotNetAPITest help /e <EmuType>' for options/tests specific to an <EmuType>");
                Console.WriteLine("\t\tRun 'DotNetAPITest help optionspec' for details on passing options");
                Console.WriteLine("");
                Console.WriteLine(" Examples:");
                Console.WriteLine("   DotNetAPITest run -e IBM -s Session1.rd3x /t SomeTest\t- Run 'SomeTest' using 'Session1.rd3x'");
                Console.WriteLine("");
                return;
            }

            if (Option.ToLower().Equals("t"))
            {
                Console.WriteLine("   t <Test>\t\t- A <Test> on a specific emulation. Must specify an emulation with /e <EmuType>");
            }
            else
            {
                //Console.WriteLine("\ttest <Test>\t- Equivalent to option 't'");
                Console.WriteLine("   test <Test>");
            }
        }

        public void ListCommonOptions()
        {
            var options = _commonOptions.Keys.ToList();
            options.Sort();
            foreach (var option in options)
            {
                try
                {
                    _commonOptions[option].Help(HelpType.Basic, option);
                }
                catch
                {

                }
            }
        }

        public bool HelpOnOption(string Option)
        {
            try
            {
                _commonOptions[Option].Help(HelpType.Detailed, Option);
                return true;
            }
            catch
            {

            }
            return false;
        }

        #endregion
    }
}