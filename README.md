## NativoPlusStudio.Encryption

This an attempt at a very simple way of encrypting text with minimal configuration. The code was written based on the following Symmetric Encryption example: https://damienbod.com/2020/08/19/symmetric-and-asymmetric-encryption-in-net-core/

### How to get started
Add the NuGet package to your project file

```
<PackageReference Include="NativoPlusStudio.Encryption" Version="1.0.0" />
```
The NuGet package has the option to configure the encryption key through dependency injection. In a console application initialize the package as follows:

```csharp
IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();
var serviceProvider = new ServiceCollection()
    .AddEncryptionServices(Configuration)
    .BuildServiceProvider();

```

Or in an ASP.NET Core application use the Startup ConfigureServices method to initialize the package.

```csharp
public IConfiguration Configuration { get; }

public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddEncryptionServices(Configuration);
}
```
Make sure you set the "EncryptionConfiguration:MyPrivateKey" key/value pair in the Configuration object. 

### Symmetric Encryption

This package only supports Symmetric Encryption at the moment. If you registered the AddEncryptionServices method you can Inject the ISymmetricEncryption interface into a constructor and use the methods Encrypt and Decrypt.

```csharp
public interface IEncryption
    {
        /// <summary>
        /// Decrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="encryptedText">Text to be decrypted</param>
        /// <param name="ivBase64">This is the base 64 initiated vector that was used to encrypt the text</param>
        /// <param name="keyBase64">This is the base 64 key that was used to encrypt the text</param>
        /// <returns></returns>
        string Decrypt(string encryptedText, Func<string, string, (string, string)> encryptionKeyGenerator = null);
        /// <summary>
        /// Encrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="text">Text to be encrypted</param>
        /// <param name="ivBase64">This is the base 64 initiated vector to be used to encrypt the text</param>
        /// <param name="keyBase64">This is the base 64 key to be used to encrypt the text</param>
        /// <returns></returns>
        string Encrypt(string text, Func<string, string, (string, string)> encryptionKeyGenerator = null);
    }
```

### Usage

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.Encryption;
using NativoPlusStudio.Encryption.Configuration;
using NativoPlusStudio.Encryption.Interfaces;
using NativoPlusStudio.Encryption.Services;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static IServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(new Dictionary<string, string>() 
             { 
                 { "EncryptionConfiguration:PrimaryPrivateKey", "somekey" }
                 //{ "EncryptionConfiguration:SecondaryPrivateKey", "optionalkey" }
            })
             .Build();

            serviceProvider = new ServiceCollection()
                .AddEncryptionServices(configuration)
                .BuildServiceProvider();

            IEncryption symmetricEncryption = serviceProvider.GetRequiredService<IEncryption>();
            // or if you didnt register the AddEncryptionServices() method
            //ISymmetricEncryption symmetricEncryption = new SymmetricEncryptionService(encryptionConfiguration: new EncryptionConfiguration { PrimaryPrivateKey = "somekey" });

            var text = "I like this!";

            Console.WriteLine("**** Symmetric Encryption and Decryption ****");
            Console.WriteLine("");

            var encryptedText = symmetricEncryption.Encrypt(text);

            Console.WriteLine("Encrypted Text: "+encryptedText);

            var decryptedText = symmetricEncryption.Decrypt(encryptedText);

            Console.WriteLine("Decrypted Text: "+decryptedText);
        }
    }
}
```
