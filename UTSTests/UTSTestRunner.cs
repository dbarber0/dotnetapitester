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
            _session = @"D:\users\dab\documents\Micro Focus\Sessions\int1-0.iuts";
            _emulationType = "UTS";

            _tests.Add("US199046".ToUpper(), US199046);
        }

        #region ITest

        protected override void RunInternal()
        {
            bool newControl = GetControlObject(typeof(IUtsTerminal));
            _terminal = (IUtsTerminal)_control;
            _terminal.AfterConnect += TerminalAfterConnect;

            IView sessionView = CreateView(newControl, _terminal);
            if (sessionView == null)
            {
                throw new Exception("Failed to create the view.");
            }

            if (!_terminal.IsConnected)
            {
                Console.WriteLine("Terminal not connected - call Connect()");
                _terminal.Connect();
                WaitForConnection();
            }

            _screen = _terminal.Screen;
            _screen.MouseClick += MouseClickHandler;

            //  Call specific test

            _testMethod();
        }

        #endregion ITest

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

        protected override int CursorRow
        {
            get { return _screen.CursorRow; }
            set { _screen.CursorRow = value; }
        }

        protected override int CursorColumn
        {
            get { return _screen.CursorColumn; }
            set { _screen.CursorColumn = value; }
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
            var temp = _screen.AutoCopyOnSelect;
            Console.WriteLine("AutoCopyOnSelect = {0}", temp);
            _screen.AutoCopyOnSelect = !temp;
            Console.WriteLine("AutoCopyOnSelect = {0}", _screen.AutoCopyOnSelect);
            _screen.AutoCopyOnSelect = temp;
            Console.WriteLine("AutoCopyOnSelect = {0}", _screen.AutoCopyOnSelect);

            temp = _screen.CursorFollowsSelection;
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
            Console.WriteLine("");
            Console.WriteLine("UTS Basic Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }

        protected override void HelpOnOption_Test()
        {
            Console.WriteLine("");
            Console.WriteLine("UTS Run Test Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }

        #endregion Help

        #region DeleteMe

        #endregion
    }
}