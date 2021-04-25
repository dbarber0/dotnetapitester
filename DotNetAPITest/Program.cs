using System;
using Common;
namespace DotNetAPITest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            IApp app = new App();
            app.RunApp(args);
        }
    }
}
