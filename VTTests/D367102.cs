using System;
using Attachmate.Reflection.Emulation.OpenSystems;

namespace VTTests
{
    public class D367102 : VTTest
    {
        public D367102()
        {
            SetTestName();
        }

        public D367102(ITerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();
        }

        protected override void Command_Run(string[] CommandLine)
        {
            Console.WriteLine($"Test '{_testName}' on emulation: {_emulation}");

            _screen.SetSelectionStartPos(2, 1);
            _screen.ExtendSelection(2, 17);
            ScreenRegion sel = _screen.Selection;
            Console.WriteLine($"D367102 - sr = {sel.StartRow}, sc = {sel.StartColumn}, er = {sel.EndRow}, ec = {sel.EndColumn}, Mode = {Enum.Format(typeof(SelectionModeOption), sel.SelectionMode, "g")}");
        }

        private void SetTestName()
        {
            _testName = "D367102";
        }

    }
}