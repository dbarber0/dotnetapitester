using System;
using Attachmate.Reflection.Emulation.UTS;
using Attachmate.Reflection.UserInterface;
using Common;

namespace UTSTests
{
    public abstract class UTSTest : Test
    {
        //  Added on branch Main and separate from branch Refactor. Rebase learning experience.
        protected IUtsTerminal _terminal;
        protected IUtsScreen _screen;
        protected IView _view;

        //  Invoked when querying for help on a test
        protected UTSTest()
        {

        }

        //  Invoked when running a test
        protected UTSTest(IUtsTerminal Terminal, IView View, string Emulation)
        {
            _emulation = Emulation;
            _view = View;
            _terminal = Terminal;
            _screen = Terminal.Screen;
        }

        #region Protected

        #region Screen

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

        protected virtual void SendKeys(string Text)
        {
            _screen.SendKeys(Text);
        }

        protected virtual int Rows
        {
            get { return _screen.Rows; }
        }

        protected virtual int Columns
        {
            get { return _screen.Columns; }
        }

        protected virtual void PutText(string Data, int Row, int Column)
        {
            _screen.PutText(Data, Row, Column);
        }

        #endregion

        protected virtual void MouseClickHandler(object sender, MouseEventArgsEx args)
        {
            Console.WriteLine(string.Format("UTSMouseClickEvent - m = {0:X}", args.WindowMessage));
        }

        #endregion Protected
    }
}