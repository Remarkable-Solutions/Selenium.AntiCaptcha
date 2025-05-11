using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;

namespace Selenium.Anticaptcha.Tests.FunctionalityTests;

public class CaptchaExtensionsTests
{    
    [Fact]
    public void ShouldBeAbleToCallAllMethodsForAllCaptchaTypes()
    {
        foreach (var enumValue in Enum.GetValues(typeof(CaptchaType)))
        {
            var captcha = (CaptchaType)enumValue;
            var proxyType = captcha.ToProxyType();
            var solutionType = captcha.GetSolutionType();
            var isProxyless = captcha.IsProxylessType();
            var isProxy = captcha.IsProxyType();
            Assert.True(isProxyless != isProxy);
            Assert.True(captcha.IsProperlyDefined());
        }
    }
}