using System;
using Attachmate.Reflection.Emulation.UTS;
using Attachmate.Reflection.UserInterface;

namespace UTSTests
{
    public class GenericTest : UTSTest
    {
        public GenericTest()
        {
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "GenericTest";
        }

        public GenericTest(IUtsTerminal Terminal, IView View, string Emulation) : base(Terminal, View, Emulation)
        {
            //_emulation = Emulation;
            SetTestName();
            _screen.MouseClick += MouseClickHandler;
        }


        protected override void Command_Run(string[] CommandLine)
        {
            Console.WriteLine($"{_testName}");
            _screen.MoveCursorTo(13, 36);
            Pause();
        }

        protected override void MouseClickHandler(object sender, MouseEventArgsEx args)
        {
            Console.Write("GenericTest.");
            base.MouseClickHandler(sender, args);
        }
    }
}