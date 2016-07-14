using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.POC.ConsoleLogGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var template = "[{Timestamp:HH:mm:ss} {Level} {Message}{NewLine}{Exception}]";
            //var snk = LoggerSinkConfiguration  //.LiterateConsole(Events.LogEventLevel.Verbose, template, null);
            var log = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile("\\Logs\\Activity-{Date}.txt")
                .CreateLogger();

            log.Information("Test");
            Console.ReadKey();
        }
    }
}
