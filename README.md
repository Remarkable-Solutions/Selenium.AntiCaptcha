# Selenium.AntiCaptcha
# Selenium.AntiCaptcha

A powerful .NET library that integrates Selenium WebDriver with Anti-Captcha.com's API to automatically solve various types of CAPTCHAs on web pages.

[![NuGet](https://img.shields.io/nuget/v/Selenium.AntiCaptcha.svg)](https://www.nuget.org/packages/Selenium.AntiCaptcha/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- 🔍 **Automatic CAPTCHA detection** - Identifies CAPTCHAs on the page without manual configuration
- 🛡️ **Multi-CAPTCHA support** - Works with ReCaptcha v2/v3, FunCaptcha, GeeTest, Turnstile, and more
- 🔄 **Proxy support** - Works with or without proxies
- 🚀 **Performance optimized** - Caches site keys and optimizes requests
- ⚙️ **Highly configurable** - Customize timeouts, retries, and behavior
- 🧩 **Easy integration** - Simple extension methods for Selenium WebDriver

## Supported CAPTCHA Types

- ReCaptcha v2 (with and without proxy)
- ReCaptcha v2 Enterprise (with and without proxy)
- ReCaptcha v3 (with and without proxy)
- ReCaptcha v3 Enterprise
- FunCaptcha (with and without proxy)
- GeeTest v3/v4 (with and without proxy)
- Image to Text captcha
- Turnstile captcha (with and without proxy)
- Image to Coordinates

## Installation
A .NET library designed to seamlessly integrate [Anti-Captcha.com](https://anti-captcha.com/) services with Selenium WebDriver, enabling automated solving of various captcha types during your web automation tasks.

This library leverages the `AntiCaptchaApi.Net` library for communication with the Anti-Captcha API and provides convenient extension methods for `IWebDriver`.

## Features

Supports a wide range of captcha types, including (but not limited to):

*   ReCaptcha V2 (Normal and Invisible)
*   ReCaptcha V2 Enterprise
*   ReCaptcha V3
*   FunCaptcha
*   GeeTest (V3 and V4)
*   ImageToText
*   Turnstile

## Installation

1.  **Install via NuGet Package Manager:**
    ```powershell
    Install-Package Selenium.AntiCaptcha
    ```
    This will also install `AntiCaptchaApi.Net` as a dependency.

## Configuration and Usage (Dependency Injection)

This library is designed to be used with .NET's built-in Dependency Injection (DI) system.

**1. Register `IAnticaptchaClient`**

In your application's startup code (e.g., `Program.cs` for .NET 6+ or `Startup.cs` for older .NET Core versions), register the `IAnticaptchaClient` service provided by `AntiCaptchaApi.Net`:

```csharp
// Program.cs (.NET 6+)
using AntiCaptchaApi.Net.Extensions; // Required for AddAnticaptcha

var builder = WebApplication.CreateBuilder(args);

// Add other services...

builder.Services.AddAnticaptcha("YOUR_ANTI_CAPTCHA_API_KEY", clientConfig =>
{
    // Optional: Configure ClientConfig properties if needed
    // clientConfig.MaxHttpRequestTimeMs = 45000; 
    // clientConfig.DefaultTimeout = TimeSpan.FromSeconds(120);
});

var app = builder.Build();

// ...
```

Replace `"YOUR_ANTI_CAPTCHA_API_KEY"` with your actual API key from Anti-Captcha.com.

**2. Inject `IAnticaptchaClient`**

Inject the `IAnticaptchaClient` into any service where you need to solve captchas:

```csharp
using AntiCaptchaApi.Net;
using OpenQA.Selenium;
using Selenium.AntiCaptcha; // Required for IWebDriver extensions
using Selenium.AntiCaptcha.Models;

public class MyAutomatedTaskService
{
    private readonly IWebDriver _driver;
    private readonly IAnticaptchaClient _anticaptchaClient;

    public MyAutomatedTaskService(IWebDriver webDriver, IAnticaptchaClient anticaptchaClient)
    {
        _driver = webDriver;
        _anticaptchaClient = anticaptchaClient;
    }

    public async Task PerformTaskWithCaptcha()
    {
        _driver.Navigate().GoToUrl("your_target_website_with_captcha");

        // Prepare solver arguments
        var solverArgs = new SolverArguments
        {
            WebsiteUrl = _driver.Url,
            // WebsiteKey = "your_recaptcha_site_key", // If known, otherwise it might be auto-detected for some types
            // CaptchaType = CaptchaType.ReCaptchaV2, // Specify if known, otherwise auto-detection will be attempted
            // ProxyConfig = new ProxyConfig { /* ... if using a proxy for solving ... */ }
        };
        
        var actionArgs = new ActionArguments
        {
            // Specify elements to interact with after solving, if needed
            // ResponseElement = _driver.FindElement(By.Id("g-recaptcha-response")),
            // SubmitElement = _driver.FindElement(By.Id("submit-button"))
        };

        try
        {
            // Call the SolveCaptchaAsync extension method
            var solutionResponse = await _driver.SolveCaptchaAsync(_anticaptchaClient, solverArgs, actionArgs);

            if (solutionResponse.ErrorId > 0)
            {
                Console.WriteLine($"Anti-Captcha Error: {solutionResponse.ErrorDescription}");
                // Handle error
            }
            else
            {
                // For generic response, you might need to cast or check the specific solution type
                // if (solutionResponse is TaskResultResponse<RecaptchaSolution> recaptchaSolution)
                // {
                //     Console.WriteLine($"Captcha solved! Token: {recaptchaSolution.Solution.GRecaptchaResponse}");
                // }
                Console.WriteLine("Captcha solved successfully (details depend on captcha type).");
                // Proceed with automated task
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            // Handle exceptions (e.g., UnidentifiableCaptchaException, InsufficientSolverArgumentsException)
        }
    }
    
    // Example for a specific solution type
    public async Task SolveReCaptchaV2Specifically()
    {
        _driver.Navigate().GoToUrl("your_recaptcha_v2_page");

        var solverArgs = new SolverArguments(_driver.Url, "your_recaptcha_site_key", CaptchaType.ReCaptchaV2);
        
        var result = await _driver.SolveCaptchaAsync<AntiCaptchaApi.Net.Models.Solutions.RecaptchaSolution>(_anticaptchaClient, solverArgs);

        if (result.ErrorId == 0)
        {
            Console.WriteLine($"ReCaptcha V2 solved! Token: {result.Solution.GRecaptchaResponse}");
            // Use the token (e.g., set it in a hidden field)
        }
        else
        {
            Console.WriteLine($"Error solving ReCaptcha V2: {result.ErrorDescription}");
        }
    }
}
```

**3. Solver Arguments (`SolverArguments`)**

The `SolverArguments` class allows you to provide necessary details for the captcha solving task:

*   `CaptchaType` (optional): Explicitly specify the `Selenium.AntiCaptcha.Enums.CaptchaType`. If not provided, the library will attempt to identify it.
*   `WebsiteUrl` (often required): The URL of the page where the captcha is present.
*   `WebsiteKey` (often required for ReCaptcha, FunCaptcha, etc.): The site key associated with the captcha.
*   `ImageElement` (for ImageToText): The `IWebElement` pointing to the captcha image.
*   `ImageFilePath` (for ImageToText): Path to a local image file.
*   `ProxyConfig` (optional): If the captcha needs to be solved using a specific proxy, configure it here.
    *   `ProxyType`
    *   `ProxyAddress`
    *   `ProxyPort`
    *   `ProxyLogin` (optional)
    *   `ProxyPassword` (optional)
*   Other captcha-specific parameters (e.g., `MinScoreForRecaptchaV3`, `GeeTestChallenge`, `FunCaptchaApiJsSubdomain`).

**4. Action Arguments (`ActionArguments`)**

The `ActionArguments` class allows you to specify Selenium elements to interact with after a successful solve:

*   `ResponseElement`: An `IWebElement` (e.g., a textarea) where the captcha solution token should be placed.
*   `SubmitElement`: An `IWebElement` (e.g., a submit button) to be clicked after the token is placed.
*   `ShouldFindAndFillAccordingResponseElements`: If true, the library will attempt to find common response elements for certain captcha types.
*   `ShouldResetCookiesBeforeAdd`: If true, cookies returned by Anti-Captcha (e.g., for AntiGate) will clear existing cookies before being added.

## Migration from Older Versions

If you were using a version of this library that accepted a `clientKey` string directly in the `SolveCaptchaAsync` methods, please see the [MIGRATION.md](MIGRATION.md) guide for details on updating your code.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
