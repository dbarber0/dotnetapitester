﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Attachmate.Reflection.Framework;
using Attachmate.Reflection.UserInterface;
using CLParser;
using Microsoft.Win32;

namespace Common
{
    public abstract class TestRunner : IRun
    {
        protected volatile bool _connected;
        protected Application _app;
        protected IFrame _frame;
        protected object _control;

        protected readonly Dictionary<string, OptionDescriptor> _parsers;
        protected readonly Dictionary<string, TestDelegate> _tests;

        protected int _optionCount;

        protected OptionHelp _help;
        protected OptionHelp _listTests;

        protected string _session;
        protected string _extension;
        protected string _testName;
        protected string _emulationType;

        protected delegate void TestDelegate();
        protected TestDelegate _testMethod;

        #region ITest

        public void Run(string[] Params)
        {
            ICLParser clparser = new Parser(_parsers);
            clparser.ParseCommandLine(Params);

            if (HelpRequested())
            {
                return;
            }

            try
            {
                VerifySessionParameter();
                VerifyTestParameter();
                GetAppObject();
                GetFrameObject();

                RunInternal();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion ITest

        #region Construction

        protected TestRunner()
        {
            _parsers = new Dictionary<string, OptionDescriptor>();
            _tests = new Dictionary<string, TestDelegate>();
            OptionDescriptor od = new OptionDescriptor(DetailedHelpParser, ShowHelp);
            _parsers.Add("emuhelp", od);

            od = new OptionDescriptor(SessionFileParser, HelpOnOption_SessionFile);
            _parsers.Add("s", od);

            od = new OptionDescriptor(TestsParser, HelpOnOption_Tests);
            _parsers.Add("tests", od);

            od = new OptionDescriptor(RunTestParser, HelpOnOption_Test);
            _parsers.Add("test", od);
            _parsers.Add("t", od);
        }

        #endregion Construction

        #region DotNetAPI Helpers

        protected void GetAppObject()
        {
            _app = MyReflection.ActiveApplication;
            if (_app == null)
            {
                Guid id = MyReflection.Start();
                _app = MyReflection.CreateApplication();
                if (_app == null)
                {
                    throw new Exception("Unable to create the Application object");
                }
            }
        }

        protected void GetFrameObject()
        {
            _frame = (IFrame)_app.GetObject("Frame");
            if (_frame == null)
            {
                throw new Exception("Unable to get the Frame object");
            }
        }

        protected bool GetControlObject(Type T)
        {
            object[] controls = _app.GetControlsByFilePath(_session);
            if (controls.Length != 0)
            {
                _control = controls[0];
                Type[] ta = _control.GetType().GetInterfaces();
                foreach (Type t in ta)
                {
                    if (t == T)
                    {
                        Console.WriteLine("Types match");
                        return false;
                    }
                }

                throw new Exception($"GetControlObject: control is not of type {T}");
            }
            _control = _app.CreateControl(_session);
            return true;
        }

        protected IView CreateView(bool Create, object Terminal)
        {
            if (Create)
            {
                return _frame.CreateView(Terminal);
            }
            IView[] views = _frame.GetViewsByFilePath(_session);
            if (views.Length != 0)
            {
                return views[0];
            }
            return null;
        }

        #endregion DotNetAPI Helpers

        #region Helpers

        private void VerifyTestParameter()
        {
            if (string.IsNullOrEmpty(_testName))
            {
                throw new Exception("No test parameter specified");
            }
            if (!_tests.TryGetValue(_testName, out _testMethod))
            {
                throw new Exception($"Unknown test {_testName}");
            }

        }
        private void VerifySessionParameter()
        {
            if (string.IsNullOrEmpty(_session))
            {
                throw new Exception("No session specified");
            }

            if (!Path.IsPathRooted(_session))
            {
                CreateFullPathToSessionFile();
            }

            if(!File.Exists(_session))
            {
                throw new Exception($"Session File {_session} doesn't exist");
            }
        }

        private void CreateFullPathToSessionFile()
        {
            _session = GetMyDocsFolder() + GetInstalledProduct() + _session;
        }

        private string GetInstalledProduct()
        {
            string product = "";
            string reflection = @"Reflection\";
            string ics = @"InfoConnect\";

            string reg = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\WRQReflection\ReflectionWorkspace", "InstallDir", "");
            if (string.IsNullOrEmpty(reg))
            {
                throw new Exception("Neither Reflection nor InfoConnect appear to be installed");
            }

            if (reg.Contains(reflection))
            {
                product = reflection;
            }
            else if (reg.Contains(ics))
            {
                product = ics;
            }
            else
            {
                throw new Exception("Neither Reflection nor InfoConnect appear to be installed");
            }
            return product;
        }

        private string GetMyDocsFolder()
        {
            string md = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (md[md.Length - 1] != Path.DirectorySeparatorChar)
            {
                md += Path.DirectorySeparatorChar;
            }

            return md + "Micro Focus\\";
        }

        protected void WaitForConnection()
        {
            int count = 0;
            while (_connected == false)
            {
                Thread.Sleep(100);
                if (count > 5000)
                {
                    break;
                }
                count += 100;
            }
            if (!_connected)
            {
                throw new Exception("Failed to Connect.");
            }
        }

        protected void UpdateCursor(int L)
        {
            int r;
            int c;
            int nsp = RowColumnToScreenPosition(CursorRow, CursorColumn) + L;
            ScreenPositionToRowColumn(nsp, out r, out c);
            CursorRow = r;
            CursorColumn = c;
        }

        protected int RowColumnToScreenPosition(int R, int C)
        {
            return ((R - 1) * Columns) + C;
        }

        protected void ScreenPositionToRowColumn(int SP, out int R, out int C)
        {
            if (SP > Rows * Columns)
            {
                R = Rows;
                C = Columns;
                return;
            }
            if (SP < 1)
            {
                R = 1;
                C = 1;
                return;
            }
            R = (SP / Columns) + 1;
            C = (SP % Columns);
        }

        protected void ListTests()
        {
            Console.WriteLine("");
            Console.WriteLine($" Available tests for emulation type {_emulationType}:");
            Console.WriteLine("");
            foreach (var test in _tests)
            {
                Console.WriteLine($"\t{test.Key}");
            }
            Console.WriteLine("");
        }

        protected void Pause()
        {
            Console.WriteLine("Hit 'Enter' to continue");
            Console.ReadLine();
        }

        protected void Pause(string Message)
        {
            Console.WriteLine(Message);
            Console.ReadLine();
        }

        #endregion Helpers

        #region Parsers

        private OptionParser SessionFileParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return SessionFileParser;
                }
                Console.WriteLine("Ignoring -s option - no session file specified");
                return null;
            }

            //_session = Param.ToUpper();
            _session = Param;
            _optionCount++;
            return null;
        }

        protected OptionParser TestsParser(string Param)
        {
            _help = ListTests;
            _optionCount++;
            return null;
        }

        OptionParser DetailedHelpParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return DetailedHelpParser;
                }

                _help = ShowHelp;
                _optionCount++;
                return null;
            }

            _help = ShowHelp;
            if (_parsers.ContainsKey(Param))
            {
                _parsers.TryGetValue(Param, out OptionDescriptor od);
                _help = od?.Help;
            }
            else
            {
                Console.WriteLine($"No Help found for option {Param}");
            }

            _optionCount++;
            return null;
        }

        OptionParser RunTestParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return RunTestParser;
                }
                Console.WriteLine("No test was specified");
                return null;
            }

            _testName = Param.ToUpper();
            _optionCount++;
            return null;
        }

        #endregion Parsers

        #region Virtual Methods

        protected virtual void PutText(string Data, int Row, int Column)
        {
        }

        protected virtual int CursorRow { get; set; }
        protected virtual int CursorColumn { get; set; }
        protected virtual int Rows { get; }
        protected virtual int Columns { get; }

        protected abstract void RunInternal();

        protected virtual void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine($"DESCRIPTION:\tDo 'things' specific to {_emulationType} emulation");
            Console.WriteLine("");
            Console.WriteLine($"USAGE:\t\tDotNetAPITest -e {_emulationType} [<EmuOptionSpec> ...] where:");
            Console.WriteLine("");
            Console.WriteLine("\t\tEmuOptionSpec\t\t- <Flag><EmuOption>[<Separator><Param>]");
            Console.WriteLine("\t\t\t\t\t  Run 'DotNetAPITest -?' for details on 'OptionSpec'");
            Console.WriteLine("");
            Console.WriteLine("\t\tEmuOption/Param (case-insensitive:");
            Console.WriteLine("");
            Console.WriteLine($"\t\temuhelp\t\t\t- Help on {_emulationType}-specific options");
            Console.WriteLine("\t\temuhelp <EmuOption>\t- Help on <EmuOption>");
            Console.WriteLine("\t\ttests\t\t\t- List available tests");
            Console.WriteLine("\t\tt <Test>\t\t- Run <Test>");
        }

        protected virtual void HelpOnOption_Test()
        {
            Console.WriteLine("");
            Console.WriteLine($"DESCRIPTION:\tRun a test using {_emulationType} emulation");
            Console.WriteLine("");
            Console.WriteLine($"USAGE:\t\tDotNetAPITest -e {_emulationType} -t <Test> where:");
            Console.WriteLine("");
            Console.WriteLine("\t\t<Test>\t- The test to run");
            Console.WriteLine("");
            Console.WriteLine("\t\te.g:");
            Console.WriteLine("");
            Console.WriteLine($"\t\tDotNetAPITest -e {_emulationType} -t SomeTest");
            Console.WriteLine("");
        }

        #endregion Virtual Methods

        #region Help

        private void HelpOnOption_SessionFile()
        {
            Console.WriteLine("");
            Console.WriteLine($"DESCRIPTION:\tUse session <SessionFile> when running a test");
            Console.WriteLine("");
            Console.WriteLine($"USAGE:\t\tDotNetAPITest -e {_emulationType} -s <SessionFile>  [<OptionSpec>], where:");
            Console.WriteLine("");
            Console.WriteLine("\t\tSessionFile\t- The session file to use");
            Console.WriteLine("\t\tOptionsSpec\t- Additional (optional) settings");
            Console.WriteLine("");
            Console.WriteLine($"\t\te.g: DotNetAPITest -e {_emulationType} -t SomeTest -s SomeSession{_extension}");
            Console.WriteLine("");
            Console.WriteLine($"\t\t");
        }

        protected virtual void HelpOnOption_Tests()
        {
            Console.WriteLine("");
            Console.WriteLine($" Description:\tShow the tests available for {_emulationType} emulation");
            Console.WriteLine("");
            Console.WriteLine($" Usage:\t\tDotNetAPITest -e {_emulationType} -tests");
        }

        protected bool HelpRequested()
        {
            if (_optionCount == 0)
            {
                ShowHelp();
                return true;
            }
            if (_help != null && _optionCount == 1)
            {
                _help();
                return true;
            }
            return false;
        }

        #endregion Help

        #region DeleteMe

        /*
        public void Run(string[] Params)
        {
            ICLParser clparser = new Parser(_parsers);
            clparser.ParseCommandLine(Params);

            if (DisplayHelp())
            {
                return;
            }

            try
            {
                if (!VerifySessionName())
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (string.IsNullOrEmpty(_testName))
            {
                Console.WriteLine("No test specified");
                ShowHelp();
                return;
            }
            if (!_tests.TryGetValue(_testName, out _testMethod))
            {
                throw new Exception($"Unknown test {_testName}");
            }

            GetAppObject();
            GetFrameObject();

            RunInternal();
        }

        protected bool CreateControl(out object Control, Type T)
        {
            object[] controls = _app.GetControlsByFilePath(_session);
            if (controls.Length != 0)
            {
                Control = controls[0];
                Type[] ta = Control.GetType().GetInterfaces();
                foreach (Type t in ta)
                {
                    if (t == T)
                    {
                        Console.WriteLine("Types match");
                        return false;
                    }
                }

                throw new Exception($"CreateControl: control is not of type {T}");
            }
            Control = _app.CreateControl(_session);
            return true;
        }
        */

        #endregion

    }
}