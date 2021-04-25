using System;
using Attachmate.Reflection.Emulation.UTS;
using Attachmate.Reflection.UserInterface;
using Common;
using CLParser;

namespace UTSTests
{
    public class AutoCopyOnSelectTest : UTSTest
    {
        //  Invoked when querying for help on a test
        public AutoCopyOnSelectTest()
        {
            SetTestName();
        }

        //  Invoked when running a test
        public AutoCopyOnSelectTest(IUtsTerminal Terminal, IView View, string Emulation) : base(Terminal, View, Emulation)
        {
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "AutoCopyOnSelectTest";
        }

        protected override void Command_Run(string[] CommandLine)
        {
            Console.WriteLine(_testName);

            var temp = _screen.AutoCopyOnSelect;
            Console.WriteLine($"AutoCopyOnSelect = {temp}");
            _screen.AutoCopyOnSelect = !temp;
            Console.WriteLine("AutoCopyOnSelect = {0}", _screen.AutoCopyOnSelect);
            _screen.AutoCopyOnSelect = temp;
            Console.WriteLine("AutoCopyOnSelect = {0}", _screen.AutoCopyOnSelect);

            temp = _screen.StripTrailingBlankLines;
            Console.WriteLine($"\nStripTrailingBlankLines = {temp}");
            _screen.StripTrailingBlankLines = !temp;
            Console.WriteLine("StripTrailingBlankLines = {0}", _screen.StripTrailingBlankLines);
            _screen.StripTrailingBlankLines = temp;
            Console.WriteLine("StripTrailingBlankLines = {0}", _screen.StripTrailingBlankLines);
        }

        /*
        protected override void Command_Help(string[] CommandLine)
        {
            Console.WriteLine($"\n Generic Help - Consider creating help for {_testName}");
        }
        */

        protected override void HelpOnTest()
        {
            Console.WriteLine($"Help for {_testName} under construction");
        }
    }
}