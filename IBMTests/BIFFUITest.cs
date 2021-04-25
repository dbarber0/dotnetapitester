using System;
using Attachmate.Reflection.Emulation.IbmHosts;

namespace IBMTests
{
    public class BIFFUITest : IBMTest
    {
        public BIFFUITest(IIbmTerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "BIFFUITest";
        }

        protected override void Command_Run(string[] CommandLine)
        {
            /*
            IFileTransfer ift = _terminal.FileTransfer;
            bool test = _terminal.FileTransfer.Xfr400SaveColumnHeader;
            Console.WriteLine("ColumnHeader = {0}", test);

            AS400ReceiveConversionOption opt = _terminal.FileTransfer.Xfr400ReceiveConversion;
            Console.WriteLine("Receive Conversion Option = {0}", opt);

            _terminal.FileTransfer.Xfr400ReceiveConversion = AS400ReceiveConversionOption.Biff;
            Console.WriteLine("Receive Conversion Option = {0}", _terminal.FileTransfer.Xfr400ReceiveConversion);

            _terminal.FileTransfer.Xfr400ReceiveConversion = opt;
            Console.WriteLine("Receive Conversion Option = {0}", _terminal.FileTransfer.Xfr400ReceiveConversion);
            */
        }
    }
}