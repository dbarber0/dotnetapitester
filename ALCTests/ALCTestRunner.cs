using System;
using Attachmate.Reflection.Emulation.ALC;
using Attachmate.Reflection.UserInterface;
using CLParser;
using Common;

namespace ALCTests
{
    public class ALCTestRunner : TestRunner
    {
        private IAlcTerminal _terminal;
        private IAlcScreen _screen;

        public ALCTestRunner()
        {
            _session = "d:\\Users\\dab\\Documents\\Micro Focus\\Sessions\\udpfrad-0.ialc";
            _extension = ".ialc";
            _emulationType = "ALC";

            //_tests.Add("US199046".ToUpper(), US199046);
        }

        protected override void RunInternal()
        {
            bool newControl = GetControlObject(typeof(IAlcTerminal));
            _terminal = (IAlcTerminal)_control;
            _terminal.AfterConnect += TerminalAfterConnect;

            IView sessionView = CreateView(newControl, _terminal);
            if (sessionView == null)
            {
                throw new Exception("Failed to create the view.");
            }

            if (!_terminal.IsConnected)
            {
                Console.WriteLine("Terminal isn't connected - connecting");
                _terminal.Connect();
                WaitForConnection();
            }

            _screen = _terminal.Screen;

            //_testMethod();
            RunTest();
        }

        protected void RunTest()
        {
            Test o = (Test)Activator.CreateInstance(_testType, new object[] { _terminal, _emulationType });
            o.RunCommand(Commands.Run, _unprocessedParams);
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

        #region Parsers

        #endregion Parsers

        #region Help

        #endregion Help

        #region Tests

        void US199046()
        {
            Console.WriteLine("ALCTestRunner");
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

        #endregion Tests

        #region EventHandlers

        void TerminalAfterConnect(object sender, EventArgs e)
        {
            Console.WriteLine("TerminalAfterConnect");
            _connected = true;
        }

        #endregion EventHandlers

        #region DeleteMe

        /*
        protected override void ShowHelp()
        {
            base.ShowHelp();
            Console.WriteLine("\t\ts <Session>\t\t- Use session file <Session>");
            Console.WriteLine("");
        }

        */

        #endregion DeleteMe
    }
}