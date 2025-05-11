using AntiCaptchaApi.Net.Models;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Constants;
using Selenium.AntiCaptcha.Enums;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.CaptchaIdentifiers;

public class TurnstileCaptchaIdentifier  : ProxyCaptchaIdentifier
{
    public TurnstileCaptchaIdentifier()
    {
        IdentifiableTypes.AddRange(CaptchaTypeGroups.TurnstileTypes);
    }

    public override async Task<CaptchaType?> IdentifyInCurrentFrameAsync(IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken)
    {
        try
        {
            var turnstileFrame = GetTurnstileIFrame(driver);

            if (turnstileFrame == null)
            {
                return null;
            }

            return await base.SpecifyCaptcha(CaptchaType.TurnstileProxyless, driver, imageElement, proxyConfig, cancellationToken);
        }
        catch (Exception)
        {
            return null;
        }
    }
    

    public override Task<CaptchaType?> SpecifyCaptcha(CaptchaType originalType, IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
    {
        return IdentifyInCurrentFrameAsync(driver, imageElement, proxyConfig, cancellationToken);
    }

    private static IWebElement? GetTurnstileIFrame(IWebDriver driver)
    {
        return driver.FindByXPathInCurrentFrame("//iframe[contains(@src, 'turnstile')]");
    }

}