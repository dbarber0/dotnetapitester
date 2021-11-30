using System;
using System.Collections.Generic;
using System.Threading;
using Attachmate.Reflection.Emulation.UTS;
using Attachmate.Reflection.Framework;
using Attachmate.Reflection.UserInterface;
using Common;

namespace UTSTests
{
    public class UTSTestRunner : TestRunner
    {
        private IUtsTerminal _terminal;
        private IUtsScreen _screen;

        public UTSTestRunner()
        {
            _emulationType = "UTS";

            _tests.Add("AutoCopyOnSelect".ToUpper(), typeof(AutoCopyOnSelectTest));
            _tests.Add("GenericTest".ToUpper(), typeof(GenericTest));
        }

        #region ITest

        protected override void RunInternal()
        {
            bool newControl = GetControlObject(typeof(IUtsTerminal));
            _terminal = (IUtsTerminal)_control;
            _terminal.AfterConnect += TerminalAfterConnect;

            _view = CreateView(newControl, _terminal);
            if (!_terminal.IsConnected)
            {
                Console.WriteLine("Terminal not connected - call Connect()");
                _terminal.Connect();
                WaitForConnection();
            }

            _screen = _terminal.Screen;
            //_screen.MouseClick += MouseClickHandler;

            //  Call specific test
            RunTest();
        }

        #endregion ITest

        protected void RunTest()
        {
            //Test o = (Test)Activator.CreateInstance(_testType, new object[] { _terminal, _emulationType });
            Test o = (Test)Activator.CreateInstance(_testType, new object[] { _terminal, _view, _emulationType });
            o.RunCommand(Commands.Run, _unprocessedParams);
        }

        void TerminalAfterConnect(object sender, EventArgs e)
        {
            Console.WriteLine("TerminalAfterConnect");
            _connected = true;
        }

        #region Help

        protected override void ShowHelp()
        {
            Console.WriteLine("\n UTSTestRunner help");
        }

        #endregion Help

        #region DeleteMe

        /*
        */
        #endregion
    }
}