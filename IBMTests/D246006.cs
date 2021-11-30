using Attachmate.Reflection;
using Attachmate.Reflection.Emulation.IbmHosts;

namespace IBMTests
{
    public class D246006 : IBMTest
    {
        public D246006(IIbmTerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "D246006";
        }

        protected override void Command_Run(string[] CommandLine)
        {
            Pause();
            _screen.PutTextMaskProtectedField = PutTextModeOption.LinearStream;
            ReturnCode rc = _screen.PutText("BillyBobBarker'sBarker", 5, 10);
            Pause($"ReturnCode = {rc}");
        }
    }
}