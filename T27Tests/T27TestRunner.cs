using System;
using System.Threading;
using Attachmate.Reflection.UserInterface;
using Attachmate.Reflection.Emulation.T27;
using Common;


namespace T27Tests
{
    public class T27TestRunner : TestRunner
    {
        private IT27Terminal _terminal;
        private IT27Screen _screen;

        public T27TestRunner()
        {
            _session = "d:\\Users\\dab\\Documents\\Micro Focus\\Sessions\\tcpa-0.it27";
            _emulationType = "T27";

            _tests.Add("PutTextTest".ToUpper(), PutTextTest);
        }

        #region ITest

        protected override void RunInternal()
        {
            bool newControl = GetControlObject(typeof(IT27Terminal));
            _terminal = (IT27Terminal)_control;
            _terminal.AfterConnect += AfterConnect;

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

            _testMethod();
        }

        #endregion ITest

        protected void PutTextTest()
        {
            Console.WriteLine("PutTextTest");
            int count = 0;
            while (!_screen.ScreenSettleState)
            {
                Thread.Sleep(100);
                if (count > 10000)
                {
                    break;
                }
                count += 100;
            }

            if (_screen.ScreenSettleState)
            {
                _screen.SendKeys("dab");
            }
        }

        protected void AfterConnect(object sender, EventArgs e)
        {
            Console.WriteLine("AfterConnect");
            _connected = true;
        }

        #region Help

        protected override void ShowHelp()
        {
        Console.WriteLine("");
            Console.WriteLine("T27 Basic Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }

        protected override void HelpOnOption_Test()
        {
            Console.WriteLine("");
            Console.WriteLine("T27 Run Test Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }

        #endregion Help

    }
}
