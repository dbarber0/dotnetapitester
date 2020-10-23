﻿using System;
using Attachmate.Reflection.Emulation.OpenSystems;
using CLParser;

namespace VTTests
{
    public class AutoConnectPropertyTest : VTTest
    {
        public AutoConnectPropertyTest()
        {
            SetTestName();
        }

        public AutoConnectPropertyTest(ITerminal Terminal, string Emulation)
        {
            _terminal = Terminal;
            _screen = Terminal.Screen;
            _emulation = Emulation;
            SetTestName();
        }

        private void SetTestName()
        {
            _testName = "AutoConnectProperty";
        }

        public override void Run(string[] Params)
        {
            Console.WriteLine($"Test '{_testName}' on emulation: {_emulation}");

            bool t = _terminal.AutoConnect;
            Console.WriteLine($"AutoConnect = {t}\n");
            _terminal.AutoConnect = !t;
            Console.WriteLine($"AutoConnect = {_terminal.AutoConnect}");
        }

    }
}