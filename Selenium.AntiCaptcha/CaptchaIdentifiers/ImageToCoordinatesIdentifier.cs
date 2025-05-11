using AntiCaptchaApi.Net.Models;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.CaptchaIdentifiers;

public class ImageToCoordinatesIdentifier : ProxyCaptchaIdentifier
{
    public ImageToCoordinatesIdentifier()
    {
        IdentifiableTypes.Add(CaptchaType.ImageToCoordinates);
    }

    public override async Task<CaptchaType?> SpecifyCaptcha(CaptchaType originalType, IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
    {
        return originalType;
    }

    public override async Task<CaptchaType?> IdentifyInCurrentFrameAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken)
    {
        return null; //TODO.
        var base64 = imageElement?.DownloadSourceAsBase64String();
        
        if (!string.IsNullOrEmpty(base64))
        {
            return CaptchaType.ImageToCoordinates;
        }
        
        return DoesCaptchaImageElementExists(driver) ? CaptchaType.ImageToCoordinates : null;
    }

    private static bool DoesCaptchaImageElementExists(IWebDriver driver)
    {
        var possibleCaptchaImageSources = driver.FindSingleImageSourceForImageToText();
        return !string.IsNullOrEmpty(possibleCaptchaImageSources);

    }
    
    
}