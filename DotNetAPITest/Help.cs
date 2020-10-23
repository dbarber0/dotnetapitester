using System;

namespace DotNetAPITest
{
    public class Help
    {
       public static void ShowHelp()
       {
           Console.WriteLine("");
           Console.WriteLine(" DESCRIPTION:\tExercise RIC Desktop's .NET API");
           Console.WriteLine(" USAGE:\t\tDotNetAPITest [<OptionSpec> ...], where:");
           Console.WriteLine("");
           Console.WriteLine("\t\tOptionSpec\t- <Flag><Option>[<Separator><Param>]");
           Console.WriteLine("\t\tFlag\t\t- <'-' | '/'>");
           Console.WriteLine("\t\tOption\t\t- Case-insensitive, see below");
           Console.WriteLine("\t\tSeparator\t- <'=' | ' '>");
           Console.WriteLine("\t\tParam\t\t- Case-insensitive");
           Console.WriteLine("");
           Console.WriteLine("\t\tOption/Param:");
           Console.WriteLine("");
           Console.WriteLine("\t\t?\t\t- Basic help, same as running with no options");
           Console.WriteLine("\t\thelp\t\t");
           Console.WriteLine("\t\thelp <Option>\t- Detailed help on <Option>");
           Console.WriteLine("\t\te <Type>\t- Emulation Type, where <Type> is one of ALC, T27, UTS, IBM, VT");
           Console.WriteLine("");
       }

       public static void EmulationOption()
       {
           Console.WriteLine("");
           Console.WriteLine(" DESCRIPTION:\tFor a specified emulation type, run tests, display type-specific help, etc.");
           Console.WriteLine(" USAGE:\t\tDotNetAPITest -e <Type> [<EmuOptionSpec> ... | -emuhelp [<EmuOption]], where:");
           Console.WriteLine("");
           Console.WriteLine("\t\t<Type>\t\t\t- One of ALC, T27, UTS, IBM, VT");
           Console.WriteLine("\t\t\t\t\t  With neither <EmuOptionSpec>[s] nor -emuhelp");
           Console.WriteLine("\t\t\t\t\t  specified, shows basic help for <Type>");
           Console.WriteLine("\t\tEmuOptionSpec\t\t- <Flag><EmuOption>[<Separator><Param>]");
           Console.WriteLine("\t\t\t\t\t  Run 'DotNetAPITest -?' for details on 'EmuOptionSpec'");
           Console.WriteLine("\t\tEmuOption\t\t- An option specific to <Type>");
           Console.WriteLine("\t\temuhelp\t\t\t- Shows detailed help for emulation <Type>");
           Console.WriteLine("\t\temuhelp <EmuOption>\t- Shows detailed help for an option specific to <Type>");
           Console.WriteLine("");
           Console.WriteLine("\t\te.g.:");
           Console.WriteLine("");
           Console.WriteLine("\t\tDotNetAPITest -e ALC\t\t\t- Basic help for ALC");
           Console.WriteLine("\t\tDotNetAPITest -e ALC -emuhelp\t\t- Detailed help for ALC");
           Console.WriteLine("\t\tDotNetAPITest -e ALC -emuhelp <Option>\t- Detailed help for an <Option>");
           Console.WriteLine("\t\t\t\t\t\t\t  specific to ALC");
           Console.WriteLine("");
       }
    }
}