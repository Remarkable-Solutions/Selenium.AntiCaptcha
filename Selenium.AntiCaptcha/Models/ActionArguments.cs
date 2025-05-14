using OpenQA.Selenium;

namespace Selenium.AntiCaptcha.Models;

/// <summary>
/// Arguments for specifying how to handle CAPTCHA solutions in the browser
/// </summary>
public record ActionArguments
{
    /// <summary>
    /// Element where the CAPTCHA solution should be entered
    /// </summary>
    public IWebElement? ResponseElement { get; init; }
    
    /// <summary>
    /// Form submit button or other element to click after solving
    /// </summary>
    public IWebElement? SubmitElement { get; init; }
    
    /// <summary>
    /// Whether to automatically find and fill response elements on the page
    /// </summary>
    public bool ShouldFindAndFillAccordingResponseElements { get; init; } = true;
    
    /// <summary>
    /// Whether to clear cookies before adding new ones from the solution
    /// </summary>
    public bool ShouldResetCookiesBeforeAdd { get; init; } = false;
    
    /// <summary>
    /// Whether to automatically click the submit element after solving
    /// </summary>
    public bool ShouldSubmit { get; init; } = true;
    
    /// <summary>
    /// Number of retries to attempt in case of failure
    /// </summary>
    public int MaxRetries { get; init; } = 0;
}