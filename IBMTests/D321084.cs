using System;
using Attachmate.Reflection;
using Attachmate.Reflection.Emulation.IbmHosts;

namespace IBMTests
{
    public class D321084 : IBMTest
    {
        public D321084(IIbmTerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "D321084";
        }

        protected override void Command_Run(string[] CommandLine)
        {
            DisplayTestInfo();
            string t = ReadLine("Enter row to begin search for next field: ");
            int row = Convert.ToInt32(t);
            t = ReadLine("And column: ");
            int column = Convert.ToInt32(t);
            var hf = _screen.FindField(row, column, FindOption.Forward);
            Pause(string.Format("GetText returned: '{0}'", _screen.GetText(hf.StartRow, hf.StartColumn, hf.Length)));
            Pause(string.Format("FindField returned: '{0}'", hf.Text));
        }
    }
}
