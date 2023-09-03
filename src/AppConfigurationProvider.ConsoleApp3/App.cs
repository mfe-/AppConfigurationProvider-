using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurationProvider.ConsoleApp3
{
    public class App
    {
        public App(IOptionsMonitor<AppConfiguration> configuration)
        {

        }

        public void Run()
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
