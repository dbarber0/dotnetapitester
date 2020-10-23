using System;
using Attachmate.Reflection.Emulation.OpenSystems;
using CLParser;

namespace VTTests
{
    public class TimeoutTest : VTTest
    {
        int _timeout;

        public TimeoutTest()
        {
            SetTestName();
        }

        public TimeoutTest(ITerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();

            OptionDescriptor od = new OptionDescriptor(TimeoutParser, null);
            _parsers.Add("to", od);
        }

        private void SetTestName()
        {
            _testName = "TimeoutTest";
        }

        public override void Run(string[] Params)
        {
            base.Run(Params);

            Console.WriteLine($"Test '{_testName}' on emulation: {_emulation}");

            int t = ((IConnectionSettingsTelnet)_terminal.ConnectionSettings).Timeout;
            Console.WriteLine($"Timeout = {t}\n");
            if (_terminal.IsConnected)
            {
                _terminal.Disconnect();
            }
            ((IConnectionSettingsTelnet)_terminal.ConnectionSettings).Timeout = _timeout;
            Console.WriteLine($"Timeout = {((IConnectionSettingsTelnet)_terminal.ConnectionSettings).Timeout}");

            _terminal.Connect();
        }

        protected OptionParser TimeoutParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return TimeoutParser;
                }
                Console.WriteLine("No test was specified");
                return null;
            }

            try
            {
                _timeout = Convert.ToInt32(Param);
                if (_timeout < 0 || _timeout > 0xFFFF)
                {
                    _timeout = 0;
                }
            }
            catch
            {
                _timeout = 0;
            }
            return null;
        }

    }
}