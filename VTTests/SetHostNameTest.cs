using System;
using Attachmate.Reflection.Emulation.OpenSystems;
using CLParser;

namespace VTTests
{
    public class SetHostNameTest : VTTest
    {
        private string _hostName;

        public SetHostNameTest()
        {
            SetTestName();
        }

        public SetHostNameTest(ITerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();

            OptionDescriptor od = new OptionDescriptor(HostNameParser, null);
            _parsers.Add("hn", od);

        }

        private void SetTestName()
        {
            _testName = "SetHostName";
        }

        public override void Run(string[] Params)
        {
            ICLParser clparser = new Parser(_parsers);
            clparser.ParseCommandLine(Params);

            Console.WriteLine($"Test '{_testName}' on emulation: {_emulation}\n");

            if (string.IsNullOrEmpty(_hostName))
            {
                Console.WriteLine("No Host Name specified");
                return;
            }
            Console.WriteLine($"Current Host Name = {((IConnectionSettingsTelnet)_terminal.ConnectionSettings).HostAddress}");
            if (_terminal.IsConnected)
            {
                _terminal.Disconnect();
            }

            ((IConnectionSettingsTelnet) _terminal.ConnectionSettings).HostAddress = _hostName;
            _terminal.Connect();
            Console.WriteLine($"Updated Host Name = {((IConnectionSettingsTelnet)_terminal.ConnectionSettings).HostAddress}");
        }

        protected OptionParser HostNameParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return HostNameParser;
                }
                return null;
            }

            //_session = Param.ToUpper();
            _hostName = Param;
            return null;
        }

        public override void Help()
        {
           Console.WriteLine("");
           Console.WriteLine(" TEST:\t\tSetHostName");
           Console.WriteLine(" DESCRIPTION:\tUpdate the Host Name connection setting");
           Console.WriteLine(" USAGE:\t\tDotNetAPITest -e VT -s <SessionFile> -t SETHOSTNAME -hn <NewHostName>");
           Console.WriteLine("");
        }
    }
}