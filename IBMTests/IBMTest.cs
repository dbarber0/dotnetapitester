using Attachmate.Reflection.Emulation.IbmHosts;
using Common;

namespace IBMTests
{
    public abstract class IBMTest : Test
    {
        protected IIbmTerminal _terminal;
        protected IIbmScreen _screen;
    }
}