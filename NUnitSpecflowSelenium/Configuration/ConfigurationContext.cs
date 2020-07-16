using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnitSpecflowSelenium.Test.Core;
using Serilog;

namespace NUnitSpecflowSelenium.Test.Configuration
{
    public class ConfigurationContext
    {
        public IServiceProvider Provider { get; set; }
        public IServiceCollection Services { get; set; }
        private readonly IConfiguration Configuration;

        public ConfigurationContext()
        {
            ConfigureServices();
            BuildServiceProvider();
            Configuration = Provider.GetService<IConfiguration>();
        }
        private void ConfigureServices()
        {
            Services = new ServiceCollection();
            var configuration = LoadConfiguration();
            Services.AddSingleton(configuration);
            Services.AddSingleton(LoadLogger());
            var target = configuration.GetValue<string>("AppSettings:TargetEnvironment");
            Services.Configure<MyBussinessEnvironment>(configuration.GetSection($"Environments:{target}"));
        }

        private void BuildServiceProvider()
        {
            Provider = Services.BuildServiceProvider();
        }

        private IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        private ILogger LoadLogger()
        {
            //TODO Review logging to file or console
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public string AppSetting(string key)
        {
            return AppSettings($"AppSettings:{key}");
        }

        public MyBussinessEnvironment Environment { get { return Provider.GetRequiredService<IOptions<MyBussinessEnvironment>>().Value; } }
        public ScreenshotConfiguration ScreenshotConfiguration
        {
            //TODO move this to initial configuration so this is not executed every time it's needed
            get
            {
                var screenshotConfig = AppSetting("Take_Screenshot_On");
                return (ScreenshotConfiguration)Enum.Parse(typeof(ScreenshotConfiguration), screenshotConfig);
            }
        }

        private string AppSettings(string key)
        {
            var strValue = Configuration.GetValue<string>(key);

            if (string.IsNullOrEmpty(strValue))
            {
                Console.WriteLine($"Warning: variable: {key} is requested but not declared on appsettings.json");
            }
            return strValue;
        }

    }
}
