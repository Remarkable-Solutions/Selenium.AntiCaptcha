using AntiCaptchaApi.Net.Models;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.CaptchaIdentifiers;

public class ImageToTextIdentifier : ProxyCaptchaIdentifier
{
    public ImageToTextIdentifier()
    {
        IdentifiableTypes.Add(CaptchaType.ImageToText);
    }

    public override async Task<CaptchaType?> SpecifyCaptcha(CaptchaType originalType, IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
    {
        return originalType;
    }

    public override async Task<CaptchaType?> IdentifyInCurrentFrameAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken)
    {
        var base64 = imageElement?.DownloadSourceAsBase64String();
        
        if (!string.IsNullOrEmpty(base64))
        {
            return CaptchaType.ImageToText;
        }
        
        return DoesCaptchaImageElementExists(driver) ? CaptchaType.ImageToText : null;
    }

    private static bool DoesCaptchaImageElementExists(IWebDriver driver)
    {
        var possibleCaptchaImageSources = driver.FindSingleImageSourceForImageToText();
        return !string.IsNullOrEmpty(possibleCaptchaImageSources);

    }
    
    
}