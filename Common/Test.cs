using System;
using System.Collections.Generic;
using CLParser;

namespace Common
{
    public delegate void TestDelegate();
    public delegate void TestDelegate1(string[] Params);

    public abstract class Test : ITest /*IRun*/
    {
        protected string _emulation;
        protected string _testName = "Unnamed";
        protected readonly string _testOption0Text = "'-to0' (Test Option0)";

        protected readonly Dictionary<string, OptionDescriptor> _parsers;

        protected Test()
        {
            _parsers = new Dictionary<string, OptionDescriptor>();

            //OptionDescriptor od = new OptionDescriptor(TestOption0Parser, TestOptionHelp);
            //_parsers.Add("to0", od);
        }

        public virtual void Run(string[] Params)
        {
            ICLParser clparser = new Parser(_parsers);
            clparser.ParseCommandLine(Params);
        }

        public virtual void Help()
        {
            Console.WriteLine($"Help for test {_testName} under construction");
        }

        //protected virtual OptionParser TestOption0Parser(string Param)
        //{
        //    Console.WriteLine($"Option {_testOption0Text} not supported for test {_testName}");
        //    return null;
        //}

        protected virtual void TestOptionHelp()
        {
            Console.WriteLine("KMACYOYO - how'd you get here?");
        }
    }

    public class TestDescriptor
    {
        public TestDescriptor(TestDelegate Test, OptionHelp Help)
        {
            this.Test = Test;
            this.Help = Help;
        }

        public TestDelegate Test { get; set; }

        public OptionHelp Help { get; set; }
    }
}