using Attachmate.Reflection.Emulation.OpenSystems;
using Common;

namespace VTTests
{
    public abstract class VTTest : Test
    {
        protected ITerminal _terminal;
        protected IScreen _screen;
    }
}