using System;
using Common;
namespace DotNetAPITest
{
    class Program
    {
        static void Main(string[] args)
        {
            IRun app = new App();
            app.Run(args);
        }
    }
}
