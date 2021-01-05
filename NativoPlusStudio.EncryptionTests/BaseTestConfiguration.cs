using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.Encryption;

namespace NativoPlusStudio.EncryptionTests
{
    public abstract class BaseTestConfiguration
    {
        public readonly IServiceProvider serviceProvider;
        
        public BaseTestConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();

            serviceProvider = new ServiceCollection()
                .AddEncryptionServices(configuration)
                .BuildServiceProvider();
        }
    }
}
