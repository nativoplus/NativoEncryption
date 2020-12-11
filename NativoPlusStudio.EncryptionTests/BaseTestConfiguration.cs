using System;
using System.Collections.Generic;
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
             .AddInMemoryCollection(new Dictionary<string, string>() { { "EncryptionConfiguration:MyPrivateKey", "somekey"} })
             .Build();

            serviceProvider = new ServiceCollection()
                .AddEncryptionServices(configuration)
                .BuildServiceProvider();
        }
    }
}
