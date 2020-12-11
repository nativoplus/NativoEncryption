using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.Encryption.Configuration;
using NativoPlusStudio.Encryption.Interfaces;
using NativoPlusStudio.Encryption.Services;
using System;

namespace NativoPlusStudio.Encryption
{
    public static class EncryptionServiceExtension
    {
        public static IServiceCollection AddEncryptionServices(this IServiceCollection services, IConfiguration configuration)
        {
            if(services == null)
            {
                services = new ServiceCollection();
            }

            services.AddSingleton(BuildEncryptionConfiguration(configuration));
            services.AddTransient<ISymmetricEncryption, SymmetricEncryptionService>();
            return services;
        }

        private static Func<IServiceProvider, EncryptionConfiguration> BuildEncryptionConfiguration(IConfiguration configuration)
        {
            return x => new EncryptionConfiguration
            {
                MyPrivateKey = configuration["EncryptionConfiguration:MyPrivateKey"]
            };
        }
    }
}
