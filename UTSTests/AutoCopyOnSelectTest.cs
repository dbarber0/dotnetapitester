using System;
using Attachmate.Reflection.Emulation.UTS;
using Common;
using CLParser;

namespace UTSTests
{
    public class AutoCopyOnSelectTest : UTSTest
    {
        public AutoCopyOnSelectTest()
        {
            SetTestName();
        }

        public AutoCopyOnSelectTest(IUtsTerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "AutoCopyOnSelectTest";
        }

        protected override void Command_Run(string[] CommandLine)
        {
            Console.WriteLine($" CursorRow = {CursorRow}");
            CursorRow = CursorRow + 1;

            var temp = _screen.AutoCopyOnSelect;
            Console.WriteLine("AutoCopyOnSelect = {0}", temp);
            _screen.AutoCopyOnSelect = !temp;
            Console.WriteLine("AutoCopyOnSelect = {0}", _screen.AutoCopyOnSelect);
            _screen.AutoCopyOnSelect = temp;
            Console.WriteLine("AutoCopyOnSelect = {0}", _screen.AutoCopyOnSelect);
        }

        protected override void Command_Help(string[] CommandLine)
        {
            Console.WriteLine($"\n Generic Help - Consider creating help for {_testName}");
        }

    }
}