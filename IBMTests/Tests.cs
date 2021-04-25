using System;
using Attachmate.Reflection.Emulation.IbmHosts;
using Attachmate.Reflection.UserInterface;

namespace IBMTests
{
    //  Obsolete
    public partial class IBMTestRunner
    {
        protected void BIFFUITest()
        {
            /*
            bool test = _terminal.FileTransfer.Xfr400SaveColumnHeader;
            Console.WriteLine("ColumnHeader = {0}", test);
            */
        }

        protected void US200197()
        {
            if (_terminal != null)
            {
                /*
                for (int i = 0; i < 100; i++)
                {
                    bool apl = _terminal.Apl;
                    Console.WriteLine($"APL = {_terminal.Apl}");
                    _terminal.Apl = !apl;
                    Console.WriteLine($"APL = {_terminal.Apl}");
                    _terminal.Apl = apl;
                    Console.WriteLine($"APL = {_terminal.Apl}");
                    System.Threading.Thread.SpinWait(1000000);
                }
                */
                bool apl = _terminal.Apl;
                Console.WriteLine($"APL = {_terminal.Apl}");
                Pause();
                _terminal.Apl = !apl;
                Console.WriteLine($"APL = {_terminal.Apl}");
                Pause();
                _terminal.Apl = apl;
                Console.WriteLine($"APL = {_terminal.Apl}");
                Pause();
            }
        }
        protected void TLSGetSetVersion()
        {
            var originalTLSVersion = GetTLSVersion();
            Console.WriteLine("Original TLS Version = {0}", originalTLSVersion);
            Pause();

            _terminal.Disconnect();
            SetTLSVersion(TLSSSLVersionOption.TLS_V1_3);
            _terminal.Connect();

            var temp = GetTLSVersion();
            Console.WriteLine("Updated TLS Version = {0}", temp);
            Pause();

            _terminal.Disconnect();
            SetTLSVersion(originalTLSVersion);
            _terminal.Connect();

            temp = GetTLSVersion();
            Console.WriteLine("Restored TLS Version = {0}", temp);
        }

        protected TLSSSLVersionOption GetTLSVersion()
        {
            return _terminal.TLS_SSLVersion;
        }

        protected void SetTLSVersion(TLSSSLVersionOption Version)
        {
            _terminal.TLS_SSLVersion = Version;
            _terminal.Save();
        }
    }
}