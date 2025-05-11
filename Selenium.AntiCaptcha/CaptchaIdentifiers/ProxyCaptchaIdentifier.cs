using AntiCaptchaApi.Net.Models;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.CaptchaIdentifiers;

public abstract class ProxyCaptchaIdentifier : ICaptchaIdentifier
{
    protected readonly List<CaptchaType> IdentifiableTypes = new();
    
    public bool CanIdentify(CaptchaType type)
    {
        return IdentifiableTypes.Contains(type);
    }

    public abstract Task<CaptchaType?> IdentifyInCurrentFrameAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken);

    public async Task<CaptchaType?> IdentifyInAllFramesAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken)
    {        
        var currentFrame = driver.GetCurrentFrame();
        try
        {
            return await IdentifyInCurrentFrameAsync(driver, imageElement, proxyConfig, cancellationToken);
        }
        catch
        {
            return null;
        }
        finally
        {
            driver.TryToSwitchToFrame(currentFrame);
        }
    }


    public virtual async Task<CaptchaType?> SpecifyCaptcha(CaptchaType originalType, IWebDriver driver,
        IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
    {
        if (proxyConfig == null || string.IsNullOrEmpty(proxyConfig.ProxyAddress))
        {
            return originalType;
        }

        return originalType.ToProxyType();
    }

}