using AntiCaptchaApi.Net.Models;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Constants;
using Selenium.AntiCaptcha.Enums;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.CaptchaIdentifiers;

public class RecaptchaIdentifier  : ProxyCaptchaIdentifier
{
    public RecaptchaIdentifier()
    {
        IdentifiableTypes.AddRange(CaptchaTypeGroups.ReCaptchaTypes);
    }

    public override async Task<CaptchaType?> IdentifyInCurrentFrameAsync(IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken)
    {
        try
        {
            var pageSource = driver.GetAllPageSource();
            var isEnterprise = IsRecaptchaEnterprise(pageSource);
            var isInvisibleRecaptcha = IsInvisibleRecaptcha(driver);

            var isV3Recaptcha = pageSource.DoesContainRegex(
                @"https:\/\/www\.google\.com\/recaptcha\/(enterprise|api)\.js\?render=");
            var isV2Recaptcha = pageSource.DoesContainRegex(@"<div class=""g-recaptcha"" data-sitekey=");

            if (isInvisibleRecaptcha && isV2Recaptcha)
            {
                isV3Recaptcha = false; // Invisible reCAPTCHA with sitekey indicates v2.
            }

            CaptchaType result;
            if (isV2Recaptcha)
            {
                result = isEnterprise ? CaptchaType.ReCaptchaV2EnterpriseProxyless : CaptchaType.ReCaptchaV2Proxyless;
                return await base.SpecifyCaptcha(result, driver, imageElement, proxyConfig, cancellationToken);
            }

            if (isV3Recaptcha)
            {
                result = isEnterprise ? CaptchaType.ReCaptchaV3Enterprise : CaptchaType.ReCaptchaV3;
                return await base.SpecifyCaptcha(result, driver, imageElement, proxyConfig, cancellationToken);
            }

            return null; // If neither v2 nor v3 detected.
            
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool HasInteractableButtonWithSiteKey(IWebDriver driver)
    {
        var button = driver.FindByXPathAllFrames(
            "//button[string-length(@data-sitekey) > 0]",
            "//button[string-length(@sitekey) > 0]");

        return button != null;
    }

    private bool IsInvisibleRecaptcha(IWebDriver driver)
    {
        var recaptchaV3ElementPaths = new List<string>()
        {
            "//div[@class='rc-anchor-invisible-text']"
        };
        return driver.FindByXPathAllFrames(recaptchaV3ElementPaths.ToArray()) != null;
    }
    

    public override Task<CaptchaType?> SpecifyCaptcha(CaptchaType originalType, IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
    {
        return IdentifyInCurrentFrameAsync(driver, imageElement, proxyConfig, cancellationToken);
    }

    private static IWebElement? GetRecaptchaIFrame(IWebDriver driver)
    {
        return driver.FindByXPathInCurrentFrame("//iframe[contains(@src, 'recaptcha') and not(contains(@src, 'hcaptcha'))]");
    }

    private static bool IsV2(IWebDriver driver)
    {
        var recaptchaV2ElementPaths = new List<string>()
        {
            "//div[@class='rc-anchor-content']",
            
            //"//div[contains(@class, 'rc-anchor-invisible-text')]"
        };

        return driver.DoesAtLeastOneOfTheElementsExist(recaptchaV2ElementPaths);
    }
    
    private static bool IsV3(IWebDriver driver)
    {
        var recaptchaV3ElementPaths = new List<string>()
        {
            "//div[@class='rc-anchor-invisible-text']"
        };
        return driver.DoesAtLeastOneOfTheElementsExist(recaptchaV3ElementPaths);
    }

    private static bool IsRecaptchaEnterprise(string pageSource)
    {
        return pageSource.DoesContainRegex( 
            @"https:\/\/www\.google\.com\/recaptcha\/enterprise\.js");
    }

}