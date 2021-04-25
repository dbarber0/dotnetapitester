using System;
using System.Windows.Forms;
using Attachmate.Reflection.Emulation.UTS;
using Attachmate.Reflection.UserInterface;

namespace UTSTests
{
    public class MapperScreenMethodsTest263 : UTSTest
    {
        public MapperScreenMethodsTest263()
        {
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "MapperScreenMethodsTest263";
        }

        public MapperScreenMethodsTest263(IUtsTerminal Terminal, IView View, string Emulation) : base(Terminal, View, Emulation)
        {
            //_emulation = Emulation;
            SetTestName();
        }


        protected override void Command_Run(string[] CommandLine)
        {
            Console.WriteLine($"{_testName}");
            Clipboard.SetData(DataFormats.Text, "This is so");
            _screen.MoveCursorTo(13, 36);
            _screen.Paste();
        }
    }
}