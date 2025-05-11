using Selenium.AntiCaptcha.Enums;

namespace Selenium.AntiCaptcha.Exceptions;

public class UnidentifiableCaptchaException : ArgumentException
{
    public UnidentifiableCaptchaException(IReadOnlyList<CaptchaType> identifiedCaptchaTypes) : base(GetExceptionMessage(identifiedCaptchaTypes))
    {
        
    }

    private static string GetExceptionMessage(IReadOnlyList<CaptchaType> identifiedCaptchaTypes)
    {
        if (!identifiedCaptchaTypes.Any())
        {
            return "Unable to identify captcha. Did not find any captcha on current page.";
        }
        
        return $"Unable to identify captcha. Found multiple matching captcha types: " +
               $"{string.Join(',', identifiedCaptchaTypes.Select(x => x.ToString()))}";
    }
}