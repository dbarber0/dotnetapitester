using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Attachmate.Reflection.Framework;
using Attachmate.Reflection.UserInterface;
using CLParser;
using Microsoft.Win32;

namespace Common
{
    public abstract class TestRunner : TestRunnerBase, ICommand
    {
        protected volatile bool _connected;
        protected Application _app;
        protected IFrame _frame;
        protected IView _view;

        protected object _control;

        protected readonly Dictionary<string, OptionDescriptor> _options;
        protected readonly Dictionary<Commands, CommandDelegate> _commands;
        protected readonly Dictionary<string, Type> _tests;

        protected string _extension;
        protected string _emulationType;

        protected Type _testType;

        protected string[] _unprocessedParams;

        #region Construction

        protected TestRunner()
        {
            _options = new Dictionary<string, OptionDescriptor>();
            _tests = new Dictionary<string, Type>();
            _commands = new Dictionary<Commands, CommandDelegate>();

            _commands.Add(Commands.Run, Command_Run);
            _commands.Add(Commands.Help, Command_Help);
            _commands.Add(Commands.Tests, Command_Tests);
        }

        #endregion Construction

        #region ICommand

        public void RunCommand(Commands Command, string[] Params)
        {
            try
            {
                ICLParser clparser = new Parser(_options);
                _unprocessedParams = clparser.ParseCommandLine(Params);
                clparser = new Parser(_commonOptions);
                _unprocessedParams = clparser.ParseCommandLine(_unprocessedParams);
                _commands[Command](_unprocessedParams);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"TestRunner: Ooops - no command '{Command}'");
            }
        }

        #endregion ICommand

        #region Commands

        protected void Command_Run(string[] Params)
        {
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

        protected void Command_Help(string[] CommandLine)
        {
            if (string.IsNullOrEmpty(_testName))
            {
                string item = string.Empty;
                try
                {
                    item = CommandLine[0];
                    if (ShowHelpForThisOption(item))
                    {
                        return;
                    }
                    Console.WriteLine($"\n {_emulationType} Tests: No Help for item '{item}'");
                    Console.WriteLine($" If {item} is a test, rerun the command and add '/t {item}'");
                }
                catch
                {
                    ShowHelp();
                }
            }
            else
            {
                try
                {
                    VerifyTestParameter();
                    Test o = (Test)Activator.CreateInstance(_testType, new object[] { });
                    o.RunCommand(Commands.Help, _unprocessedParams);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        void Command_Tests(string[] CommandLine)
        {
            ListTests();
        }

        #endregion Commands

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
                        //  Don't need to create a control of our type, one exists
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
            throw new Exception("Failed to create the view.");
        }

        #endregion DotNetAPI Helpers

        #region Private Helpers

        private bool ShowHelpForThisOption(string Option)
        {
            try
            {
                _options.TryGetValue(Option, out OptionDescriptor od);
                od.Help(HelpType.Detailed, Option);
                return true;
            }
            catch (NullReferenceException)
            {
            }
            return false;
        }

        private void VerifyTestParameter()
        {
            if (string.IsNullOrEmpty(_testName))
            {
                throw new Exception("No test parameter specified");
            }
            if (!_tests.TryGetValue(_testName, out _testType))
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
            string reflection = @"Reflection" + Path.DirectorySeparatorChar;
            string ics = @"InfoConnect" + Path.DirectorySeparatorChar;

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

            
            return md + @"Micro Focus" + Path.DirectorySeparatorChar;
        }

        protected void ListTests()
        {
            Console.WriteLine("");
            Console.WriteLine($" Available tests for emulation type {_emulationType}:");

            foreach (var test in _tests)
            {
                Console.WriteLine($"   {test.Key}");
            }
            Console.WriteLine("");
        }

        #endregion Private Helpers

        #region Protected Helpers

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

        #endregion Protected Helpers

        #region Virtual Methods

        protected virtual void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine($" DESCRIPTION:\tDo 'things' specific to {_emulationType} sessions");
            Console.WriteLine("");
            Console.WriteLine(" Examples:");
            Console.WriteLine($"   DotNetAPITest run -e {_emulationType} -s <SessionFile> -t <SomeTest>\t- Run <SomeTest> using <SessionFile>");
            Console.WriteLine($"   DotNetAPITest help -e {_emulationType} -t <SomeTest>\t\t\t- Show Help for <SomeTest>");
            ListTests();
        }

        protected virtual void PutText(string Data, int Row, int Column)
        {
        }

        protected virtual int CursorRow { get; set; }
        protected virtual int CursorColumn { get; set; }
        protected virtual int Rows { get; }
        protected virtual int Columns { get; }

        protected abstract void RunInternal();

        #endregion Virtual Methods

        #region Help

        #endregion Help

        #region DeleteMe

        /*
        */

        #endregion

    }
}