using System;
using Attachmate.Reflection.Emulation.IbmHosts;
using Attachmate.Reflection.UserInterface;
using Common;

namespace IBMTests
{
    public partial class IBMTestRunner : TestRunner
    {
        IIbmTerminal _terminal;
        IIbmScreen _screen;

        public IBMTestRunner()
        {
            _session = @"d:\Users\dab\documents\Micro Focus\Sessions\dallas0.rd3x";
            _emulationType = "IBM";

            //_tests.Add("TLSGetSetVersion".ToUpper(), TLSGetSetVersion);
            //_tests.Add("US200197".ToUpper(), US200197);
            //_tests.Add("ScratchPadTest".ToUpper(), ScratchPadTest);
            //_tests.Add("PutText".ToUpper(), PutTextTest);
        }

        #region ITest

        protected override void RunInternal()
        {
            bool newControl = GetControlObject(typeof(IIbmTerminal));
            _terminal = (IIbmTerminal)_control;
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

            //  Call specific test
            //_testMethod();
            RunTest();
        }

        #endregion ITest

        protected void RunTest()
        {
            Test o = (Test)Activator.CreateInstance(_testType, new object[] { _terminal, _emulationType });
            o.Run(Commands.Run, _unprocessedParams);
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

        protected void ScratchPadTest()
        {
            string temp;

            if (_terminal != null)
            {
                temp = _terminal.Productivity.ScratchPadContents;
                Console.WriteLine("Set Scratchpad contents to a text string containing no RTF markup; initially Scratchpad is invisible");
                _terminal.Productivity.ScratchPadContents = "Hello non-RTF World";
                temp = _terminal.Productivity.ScratchPadContents;
                Console.Write("ScratchPadContents: ");
                Console.WriteLine(temp);
                temp = _terminal.Productivity.ScratchPadContentsAsText;
                Console.Write("ScratchPadContentsAsText: ");
                Console.WriteLine(temp);
                Console.WriteLine("Hit any key to continue");
                Console.ReadLine();

                Console.WriteLine("Set ScratchPad visible, no change to contents");
                _terminal.Productivity.ScratchPadPanelVisible = true;
                temp = _terminal.Productivity.ScratchPadContents;
                Console.Write("ScratchPadContents: ");
                Console.WriteLine(temp);
                temp = _terminal.Productivity.ScratchPadContentsAsText;
                Console.Write("ScratchPadContentsAsText: ");
                Console.WriteLine(temp);
                Console.WriteLine("Hit any key to continue");
                Console.ReadLine();

                Console.WriteLine("Set Scratchpad contents to text containing RTF markup");
                _terminal.Productivity.ScratchPadContents = "{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft Sans Serif;}}\r\n\\viewkind4\\uc1\\pard\\lang1033\\f0\\fs20 Hello RTF World\\par\r\n}\r\n";
                temp = _terminal.Productivity.ScratchPadContents;
                Console.Write("ScratchPadContents: ");
                Console.WriteLine(temp);
                temp = _terminal.Productivity.ScratchPadContentsAsText;
                Console.Write("ScratchPadContentsAsText: ");
                Console.WriteLine(temp);
                Console.WriteLine("Hit any key to continue");
                Console.ReadLine();
            }
        }

        protected void GetField()
        {
            HostField hf = _screen.GetField(1, 1);
            int i = hf.StartRow;
            i = hf.StartColumn;
            i = hf.Length;
            hf = _screen.GetField(1, 2);
            i = hf.StartRow;
        }

        protected void PutTextTest()
        {
            PutText("From DotNetAPI", 24, 43);
        }

        void TerminalAfterConnect(object sender, EventArgs e)
        {
            Console.WriteLine("TerminalAfterConnect");
            _connected = true;
        }

        #region Help

        #endregion Help

        #region DeleteMe

        /*
        protected override void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("IBM Basic Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }

        protected override void HelpOnOption_Test()
        {
            Console.WriteLine("");
            Console.WriteLine("IBM Run Test Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }
        */

        #endregion DeleteMe

    }
}