using System;
using Attachmate.Reflection.Emulation.OpenSystems;
using Attachmate.Reflection.UserInterface;
using Common;

namespace VTTests
{
    public class VTTestRunner : TestRunner
    {
        ITerminal _terminal;
        IScreen _screen;

        public VTTestRunner()
        {
            //_session = @"d:\Users\dab\documents\Micro Focus\Sessions\mermit-dab-ssh.rdox";
            //_session = @"d:\Users\dab\documents\Micro Focus\InfoConnect\dragontail1.rdox";
            //_session = @"D:\users\dab\documents\Micro Focus\InfoConnect\18.0\kona.rdox";

            _emulationType = "VT";

            //_tests.Add("TLSGetSetVersion".ToUpper(), TLSGetSetVersion);
            //_tests.Add("GetColorRGB".ToUpper(), GetColorRGB);
            //_tests.Add("SetColorRGB".ToUpper(), SetColorRGB);
            //_tests.Add("SetHostName".ToUpper(), SetHostName);

            _tests1.Add("SetHostName".ToUpper(), typeof(SetHostNameTest));
            _tests1.Add("AutoConnectProperty".ToUpper(), typeof(AutoConnectPropertyTest));
            _tests1.Add("LineDelayProperty".ToUpper(), typeof(LineDelayTest));
            _tests1.Add("TimeoutProperty".ToUpper(), typeof(TimeoutTest));
        }

        #region ITest

        protected override void RunInternal()
        {
            bool newControl = GetControlObject(typeof(ITerminal));
            _terminal = (ITerminal)_control;
            _terminal.Connected += TerminalConnected;

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

        protected void TLSGetSetVersion()
        {
            //  Use dragaontail1.rdox for this test.
            var originalTLSVersion = GetTLSVersion();
            Console.WriteLine("Original TLS Version = {0}", originalTLSVersion);
            Pause("Hit any key to continue");
            //Console.WriteLine("Hit any key to continue");
            //Console.ReadLine();
            _terminal.Disconnect();
            SetTLSVersion(TLSVersionOption.TLSVersion1_3);
            _terminal.Connect();
            var temp = GetTLSVersion();
            Console.WriteLine("Updated TLS Version = {0}", temp);
            Console.WriteLine("Hit any key to continue");
            Console.ReadLine();
            _terminal.Disconnect();
            SetTLSVersion(originalTLSVersion);
            _terminal.Connect();
            temp = GetTLSVersion();
            Console.WriteLine("Restored TLS Version = {0}", temp);
        }

        protected void SetTLSVersion(TLSVersionOption Version)
        {
            _terminal.ConnectionSettings.TLSVersion = Version;
            _terminal.Save();
        }

        protected TLSVersionOption GetTLSVersion()
        {
            return _terminal.ConnectionSettings.TLSVersion;
        }

        void TerminalConnected(object sender, EventArgs e)
        {
            Console.WriteLine("TerminalConnected");
            _connected = true;
        }

        void GetColorRGB()
        {

            //Get integers that are mapped to the background and foreground colors
            int bgColor = _terminal.Theme.Color.GetBackgroundColorAsInt(TextColorMappingAttribute.Plain);
            int fgColor = _terminal.Theme.Color.GetForegroundColorAsInt(TextColorMappingAttribute.Plain);

            fgColor = 8;

            var color = _terminal.Theme.Color.GetColorRGB(fgColor);

            string[] sa = color.Split(',');
            int red = Convert.ToInt32(sa[0]);
            int green = Convert.ToInt32(sa[1]);
            int blue = Convert.ToInt32(sa[2]);

            Console.WriteLine($"fgColor = {fgColor}, Red = {red}, Green = {green}, Blue = {blue}");
            Console.WriteLine("fgColor = {0:X}, Red = 0x{1:X}, Green = 0x{2:X}, Blue = 0x{3:X}", fgColor, red, green, blue);
        }

        void SetColorRGB()
        {
            /*
            var color = _terminal.Theme.Color.GetColorRGB(ColorEnum.Blue);
            string[] sa = color.Split(',');
            int red = Convert.ToInt32(sa[0]);
            int green = Convert.ToInt32(sa[1]);
            int blue = Convert.ToInt32(sa[2]);

            Console.WriteLine($"Red = {red}, Green = {green}, Blue = {blue}");
            Console.WriteLine($"Original: {ColorEnum.Blue} is {color}");
            _terminal.Theme.Color.SetColorRGB(ColorEnum.Blue, 0, 0, 42);
            color = _terminal.Theme.Color.GetColorRGB(ColorEnum.Blue);
            Console.WriteLine($"Updated: {ColorEnum.Blue} is now {color}", ColorEnum.Blue.ToString());
            //Console.WriteLine("Hit any key to continue");
            //Console.ReadLine();
            Pause("Check Theme setting, then hit any key to continue");
            _terminal.Theme.Color.SetColorRGB(ColorEnum.Blue, 0, 0, 255);
            color = _terminal.Theme.Color.GetColorRGB(ColorEnum.Blue);
            Console.WriteLine($"Restored: {ColorEnum.Blue} is {color}", ColorEnum.Blue.ToString());
            Console.WriteLine("Check Theme setting");
            */
        }

        void SetHostName()
        {
            if (_terminal.IsConnected)
            {
                _terminal.Disconnect();
            }
            //((IConnectionSettingsTelnet)_terminal.ConnectionSettings).HostAddress = "sylvester";
            ((IConnectionSettingsTelnet)_terminal.ConnectionSettings).HostAddress = "kona";
            _terminal.Connect();
        }

        void DAB()
        {
            var sa = new []{"bob", "dab"};
            Test o = (Test)Activator.CreateInstance(typeof(SetHostNameTest), new object[] {_terminal, _screen});
            o.Run(sa);
        }

        protected void RunTest()
        {
            Test o = (Test)Activator.CreateInstance(_testType, new object[] { _terminal, _emulationType });
            o.Run(_params);
        }

        #region Help

        protected override void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("VT Basic Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }

        protected override void HelpOnOption_Test()
        {
            Console.WriteLine("");
            Console.WriteLine("VT Run Test Help:");
            Console.WriteLine("");
            Console.WriteLine("\t Under construction");
            Console.WriteLine("");
        }

        #endregion Help

    }
}