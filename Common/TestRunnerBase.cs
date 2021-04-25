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
                Console.WriteLine("Detailed help on option 's' is coming");
                return;
            }
            Console.WriteLine("\ts <SessionFile>\t- Session file to use. Must specify an emulation with /e <Type>");
        }

        void HelpOnOption_Test(HelpType Type, string Option)
        {
            if (Type == HelpType.Detailed)
            {
                Console.WriteLine("Detailed help on option 't | test' is coming");
                return;
            }

            if (Option.ToLower().Equals("t"))
            {
                Console.WriteLine("\tt <Test>\t- A <Test> on a specific emulation. Must specify an emulation with /e <Type>");
            }
            else
            {
                Console.WriteLine("\ttest <Test>\t- Equivalent to option 't'");
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