# Migration Guide: Updating Selenium.AntiCaptcha
# Migration Guide

This guide helps you migrate from older versions of Selenium.AntiCaptcha to newer versions.

## Migrating from v1.x to v2.x

Version 2.x introduces several breaking changes and new features to improve usability and maintainability.

### Breaking Changes

1. **Unified API**
   - The separate methods for different CAPTCHA types have been consolidated into a single `SolveCaptchaAsync` method
   - You now need to specify the CAPTCHA type via the `SolverArguments` parameter

2. **Namespace and Class Changes**
   - Some classes have been moved to different namespaces
   - The configuration model has been restructured

3. **Default Behavior Changes**
   - Auto-identification of CAPTCHA types is now the default behavior
   - Timeouts and retry logic have been improved

### Feature Additions

1. **Support for New CAPTCHA Types**
   - Added support for Turnstile CAPTCHA
   - Added support for GeeTest v4
   - Added support for Image to Coordinates

2. **Performance Improvements**
   - Site key caching
   - More efficient frame scanning
   - Reduced network overhead

3. **Better Error Handling**
   - Specific exception types for different error scenarios
   - Retry mechanism with backoff strategy

### Code Migration Examples

#### Example 1: Basic ReCaptcha v2 Solving

Before (v1.x):
This guide outlines the steps to migrate your existing code to use the newer versions of `Selenium.AntiCaptcha` that leverage Dependency Injection (DI) for managing the `IAnticaptchaClient`. This change aligns with the underlying `AntiCaptchaApi.Net` library's approach to client management and API key configuration.

## Key Change Summary

The most significant change is that the `SolveCaptchaAsync` extension methods on `IWebDriver` no longer accept your Anti-Captcha API key as a direct `string clientKey` parameter. Instead, they now require an instance of `IAnticaptchaClient`. This client instance is responsible for holding the API key and other client configurations and should be obtained through DI.

## Steps to Migrate

Follow these steps to update your application:

**1. Update NuGet Packages**

Ensure you have the latest version of `Selenium.AntiCaptcha` installed in your project. This will also bring in the corresponding version of its dependency, `AntiCaptchaApi.Net`.

You can update via the NuGet Package Manager Console:
```powershell
Update-Package Selenium.AntiCaptcha
```
Or through the Visual Studio NuGet Package Manager UI.

**2. Register `IAnticaptchaClient` in your DI Container**

Your Anti-Captcha API key is now configured when you register the `IAnticaptchaClient` service. This is typically done in your application's composition root (e.g., `Program.cs` for .NET 6+ or `Startup.cs` for older .NET Core/ASP.NET Core applications).

*Example for `Program.cs` (.NET 6+):*
```csharp
using AntiCaptchaApi.Net.Extensions; // Required for AddAnticaptcha

var builder = WebApplication.CreateBuilder(args);

// ... other service registrations ...

// Register IAnticaptchaClient
builder.Services.AddAnticaptcha("YOUR_ANTI_CAPTCHA_API_KEY", clientConfig =>
{
    // Optional: You can configure ClientConfig properties here if needed
    // clientConfig.MaxHttpRequestTimeMs = 45000; 
    // clientConfig.DefaultTimeout = TimeSpan.FromSeconds(120);
});

var app = builder.Build();
// ...
```

*Example for `Startup.cs` (`ConfigureServices` method):*
```csharp
using AntiCaptchaApi.Net.Extensions; // Required for AddAnticaptcha

public void ConfigureServices(IServiceCollection services)
{
    // ... other service registrations ...

    // Register IAnticaptchaClient
    services.AddAnticaptcha("YOUR_ANTI_CAPTCHA_API_KEY", clientConfig =>
    {
        // Optional: You can configure ClientConfig properties here if needed
        // clientConfig.MaxHttpRequestTimeMs = 45000; 
        // clientConfig.DefaultTimeout = TimeSpan.FromSeconds(120);
    });
    
    // ...
}
```
Replace `"YOUR_ANTI_CAPTCHA_API_KEY"` with your actual API key.

**3. Obtain `IAnticaptchaClient` via Dependency Injection**

In the classes or services where you perform captcha solving, inject `IAnticaptchaClient` through the constructor.

*Example Service:*
```csharp
using AntiCaptchaApi.Net;
using OpenQA.Selenium;

public class MyCaptchaAutomationService
{
    private readonly IWebDriver _driver;
    private readonly IAnticaptchaClient _anticaptchaClient; // Injected client

    public MyCaptchaAutomationService(IWebDriver webDriver, IAnticaptchaClient anticaptchaClient)
    {
        _driver = webDriver;
        _anticaptchaClient = anticaptchaClient;
    }

    // ... your methods that use SolveCaptchaAsync ...
}
```

**4. Update `SolveCaptchaAsync` Calls**

Modify your calls to `driver.SolveCaptchaAsync(...)` to pass the injected `IAnticaptchaClient` instance instead of the `clientKey` string.

*Old Code Example:*
```csharp
// Previously, you might have done this:
// string apiKey = "YOUR_ANTI_CAPTCHA_API_KEY"; 
// var solverArgs = new SolverArguments(...);
// var result = await driver.SolveCaptchaAsync(apiKey, solverArgs, ...); 
```

*New Code Example (using the injected `_anticaptchaClient` from the example above):*
```csharp
using Selenium.AntiCaptcha; // Required for IWebDriver extensions
using Selenium.AntiCaptcha.Models;

// ... inside your service method ...
var solverArgs = new SolverArguments { /* ... your arguments ... */ };
var actionArgs = new ActionArguments { /* ... your arguments ... */ };

// Pass the injected IAnticaptchaClient instance
var result = await _driver.SolveCaptchaAsync(_anticaptchaClient, solverArgs, actionArgs); 
```

If you are solving for a specific solution type:

*Old Code Example (specific solution):*
```csharp
// string apiKey = "YOUR_ANTI_CAPTCHA_API_KEY";
// var solverArgs = new SolverArguments(...);
// var result = await driver.SolveCaptchaAsync<RecaptchaSolution>(apiKey, solverArgs, ...);
```

*New Code Example (specific solution):*
```csharp
// var solverArgs = new SolverArguments(...);
// var result = await _driver.SolveCaptchaAsync<AntiCaptchaApi.Net.Models.Solutions.RecaptchaSolution>(_anticaptchaClient, solverArgs, ...);
```

## Summary

The core of the migration involves shifting from passing the API key directly to `SolveCaptchaAsync` to a DI-centric approach where `IAnticaptchaClient` (configured with your API key during registration) is injected and then passed to the solving methods. This aligns with modern .NET practices and provides better management of client configurations.