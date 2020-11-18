using Attachmate.Reflection.Emulation.UTS;
using Common;

namespace UTSTests
{
    public abstract class UTSTest : Test
    {
        //  Added on branch Main and separate from branch Refactor. Rebase learning experience.
        protected IUtsTerminal _terminal;
        protected IUtsScreen _screen;

        protected virtual int CursorRow
        {
            get { return _screen.CursorRow; }
            set { _screen.CursorRow = value; }
        }

        protected virtual int CursorColumn
        {
            get { return _screen.CursorColumn; }
            set { _screen.CursorColumn = value; }
        }

    }
}