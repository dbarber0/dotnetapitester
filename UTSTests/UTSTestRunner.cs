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
            _screen.MouseClick += MouseClickHandler;

            //  Call specific test
            RunTest();
        }

        #endregion ITest

        protected void RunTest()
        {
            Test o = (Test)Activator.CreateInstance(_testType, new object[] { _terminal, _emulationType });
            o.Run(Commands.Run, _unprocessedParams);
        }

        protected void MouseClickHandler(object sender, MouseEventArgsEx args)
        {
            Console.WriteLine(string.Format("MouseClickEvent - m = {0:X}", args.WindowMessage));
        }

        protected void SendKeys(string Text)
        {
            _screen.SendKeys(Text);
        }

        protected override void PutText(string Data, int Row, int Column)
        {
            _screen.PutText(Data, Row, Column);
        }

        protected override int Rows
        {
            get { return _screen.Rows; }
        }

        protected override int Columns
        {
            get { return _screen.Columns; }
        }

        void US199046()
        {
            Console.WriteLine("UTS");
            var temp = _screen.CursorFollowsSelection;
            Console.WriteLine("\nCursorFollowsSelection = {0}", temp);
            _screen.AutoCopyOnSelect = !temp;
            Console.WriteLine("CursorFollowsSelection = {0}", _screen.AutoCopyOnSelect);
            _screen.AutoCopyOnSelect = temp;
            Console.WriteLine("CursorFollowsSelection = {0}", _screen.AutoCopyOnSelect);

            temp = _screen.StripTrailingBlankLines;
            Console.WriteLine("\nStripTrailingBlankLines = {0}", temp);
            _screen.AutoCopyOnSelect = !temp;
            Console.WriteLine("StripTrailingBlankLines = {0}", _screen.AutoCopyOnSelect);
            _screen.AutoCopyOnSelect = temp;
            Console.WriteLine("StripTrailingBlankLines = {0}", _screen.AutoCopyOnSelect);
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

        #endregion
    }
}