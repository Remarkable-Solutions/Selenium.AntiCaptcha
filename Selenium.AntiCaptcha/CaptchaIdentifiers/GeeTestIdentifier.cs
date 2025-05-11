using AntiCaptchaApi.Net.Models;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Constants;
using Selenium.AntiCaptcha.Enums;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.CaptchaIdentifiers;

public class GeeTestIdentifier : ProxyCaptchaIdentifier
{
    public GeeTestIdentifier()
    {
        IdentifiableTypes.AddRange(CaptchaTypeGroups.GeeTestTypes);
    }

    public override async Task<CaptchaType?> IdentifyInCurrentFrameAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken)
    {
        try
        {
            var pageSource = driver.GetAllPageSource();
            if (pageSource.Contains("https://static.geetest.com/v4/gt.js"))
            {
                return await base.SpecifyCaptcha(CaptchaType.GeeTestV4Proxyless, driver, imageElement, proxyConfig, cancellationToken);
            }
            if (pageSource.Contains("https://static.geetest.com/static/gt.js"))
            {
                return await base.SpecifyCaptcha(CaptchaType.GeeTestV3Proxyless, driver, imageElement, proxyConfig, cancellationToken);
            }
            
            // Check for iframes (commonly used in v3)
            var iframes = driver.FindElements(By.TagName("iframe"));
            foreach (var iframe in iframes)
            {
                var src = iframe.GetAttribute("src");
                if (src != null && src.Contains("geetest"))
                {
                    return await base.SpecifyCaptcha(CaptchaType.GeeTestV3Proxyless, driver, imageElement, proxyConfig, cancellationToken);
                }
            }

            // Check for v4 container elements
            var v4Elements = driver.FindElements(By.CssSelector("div.geetest-captcha-container"));
            if (v4Elements.Count > 0)
            {
                return await base.SpecifyCaptcha(CaptchaType.GeeTestV4Proxyless, driver, imageElement, proxyConfig, cancellationToken);
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public override Task<CaptchaType?> SpecifyCaptcha(CaptchaType originalType, IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
    {
        return IdentifyInCurrentFrameAsync(driver, imageElement, proxyConfig, cancellationToken);
    }

    private static IWebElement? GetGeeScriptElement(IWebDriver driver)
    {
        return driver.FindByXPathAllFrames("//script[contains(@src, 'geetest.com') and contains(@src, 'challenge')]");
    }

}