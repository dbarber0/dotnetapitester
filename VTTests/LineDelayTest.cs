using System;
using Attachmate.Reflection.Emulation.OpenSystems;

namespace VTTests
{
    public class LineDelayTest : VTTest
    {
        public LineDelayTest()
        {
            SetTestName();
        }

        public LineDelayTest(ITerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();
        }

        protected override void Command_Run(string[] CommandLine)
        {
            Console.WriteLine($"Test '{_testName}' on emulation: {_emulation}");

            int t = ((IConnectionSettingsModem)_terminal.ConnectionSettings).LineDelay;
            Console.WriteLine($"LineDelay = {t}\n");
            ((IConnectionSettingsModem)_terminal.ConnectionSettings).LineDelay = t + 55;
            Console.WriteLine($"AutoConnect = {((IConnectionSettingsModem)_terminal.ConnectionSettings).LineDelay}");
        }

        private void SetTestName()
        {
            _testName = "LineDelay";
        }

        /*
        public override void Run(string[] Params)
        {
        }
        */
    }
}