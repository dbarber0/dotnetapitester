using System;
using System.Collections.Generic;
using ALCTests;
using CLParser;
using T27Tests;
using UTSTests;

using Common;
using IBMTests;
using VTTests;

namespace DotNetAPITest
{
    delegate IRun TestRunnerFactory();

    public class App : IRun
    {
        string _emulationType;
        private string[] _params;

        private readonly Dictionary<string, OptionDescriptor> _parsers;
        private int _optionCount;
        private OptionHelp _help;
        private TestRunnerFactory _factory;

        private readonly Dictionary<string, TestRunnerFactory> _testRunnerFactories;

        #region Construction

        public App()
        {
            _parsers = new Dictionary<string, OptionDescriptor>();

            OptionDescriptor od = new OptionDescriptor(BasicHelpParser, ShowHelp);
            _parsers.Add("?", od);

            od = new OptionDescriptor(DetailedHelpParser, ShowHelp);
            _parsers.Add("help", od);

            od = new OptionDescriptor(EmulationParser, HelpOnOption_Emulation);
            _parsers.Add("e", od);

            _testRunnerFactories = new Dictionary<string, TestRunnerFactory>();
            _testRunnerFactories.Add("ALC", ALCTestRunnerFactory);
            _testRunnerFactories.Add("T27", T27TestRunnerFactory);
            _testRunnerFactories.Add("UTS", UTSTestRunnerFactory);
            _testRunnerFactories.Add("IBM", IBMTestRunnerFactory);
            _testRunnerFactories.Add("VT", VTTestRunnerFactory);
        }

        #endregion Construction

        #region Factories

        IRun ALCTestRunnerFactory()
        {
            return new ALCTestRunner();
        }

        IRun T27TestRunnerFactory()
        {
            return new T27TestRunner();
        }

        IRun UTSTestRunnerFactory()
        {
            return new UTSTestRunner();
        }

        IRun IBMTestRunnerFactory()
        {
            return new IBMTestRunner();
        }

        IRun VTTestRunnerFactory()
        {
            return new VTTestRunner();
        }

        private void GetTestRunnerFactory()
        {
            if (!_testRunnerFactories.TryGetValue(_emulationType, out _factory))
            {
                throw new Exception($"Unknown emulation type: {_emulationType}");
            }

        }

        #endregion Factories

        #region IApp

        public void Run(string[] Params)
        {
            _params = Params;
            ICLParser clparser = new Parser(_parsers);
            string[] unprocessedParams = clparser.ParseCommandLine(_params);

            if (HelpRequested())
            {
                return;
            }

            if (!_testRunnerFactories.TryGetValue(_emulationType, out _factory))
            {
                Console.WriteLine($"Unknown emulation type: {_emulationType}");
                return;
            }
            _factory().Run(unprocessedParams);
        }

        #endregion IApp

        #region Parsers

        OptionParser BasicHelpParser(string Param)
        {
            _help = ShowHelp;
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

            if (_parsers.ContainsKey(Param))
            {
                _parsers.TryGetValue(Param, out OptionDescriptor od);
                _help = od?.Help;
            }
            else
            {
                Console.WriteLine($"No Help found for option {Param}");
                _help = ShowHelp;
            }

            _optionCount++;
            return null;
        }

        OptionParser EmulationParser(string Param)
        {
            if (string.IsNullOrEmpty(Param))
            {
                if (Param != null)
                {
                    return EmulationParser;
                }

                Console.WriteLine("EmulationTypeParser - Expected value");
                return null;
            }

            _emulationType = Param.ToUpper();
            _optionCount++;
            return null;
        }

        #endregion Parsers

        #region Help

        bool HelpRequested()
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

        private void ShowHelp()
        {
            Help.ShowHelp();
        }

        private void HelpOnOption_Emulation()
        {
            Help.EmulationOption();
        }

        #endregion Help

        #region DeleteMe

        /*
        public void Run0()
        {
            ICLParser clparser = new Parser(_parsers);
            string[] unprocessedParams = clparser.ParseCommandLine(_params);

            if (DisplayHelp())
            {
                return;
            }

            try
            {
                GetTestRunnerFactory();
                _factory().Run(unprocessedParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        */

        #endregion
    }
}
