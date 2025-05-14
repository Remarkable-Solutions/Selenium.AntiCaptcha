namespace Selenium.AntiCaptcha.Models;

/// <summary>
/// Exception thrown when CAPTCHA solving times out
/// </summary>
public class CaptchaSolverTimeoutException : CaptchaSolverException
{
    public CaptchaSolverTimeoutException(string message) : base(message)
    {
    }
}
