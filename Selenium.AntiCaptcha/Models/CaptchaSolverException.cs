using System;

namespace Selenium.AntiCaptcha.Models;

/// <summary>
/// Base exception for all CAPTCHA solving errors
/// </summary>
public class CaptchaSolverException : Exception
{
    public CaptchaSolverException(string message) : base(message)
    {
    }

    public CaptchaSolverException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
