using AntiCaptchaApi.Net.Models;
using AntiCaptchaApi.Net.Models.Solutions;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.CaptchaIdentifiers;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;

namespace Selenium.AntiCaptcha;

public static class CaptchaIdentifier
{
    private static readonly List<ICaptchaIdentifier> CaptchaIdentifiers = new()
    {
        new GeeTestIdentifier(),
        new RecaptchaIdentifier(),
        new ImageToTextIdentifier(),
        new AntiGateIdentifier(),
        new FunCaptchaIdentifier(),
        new TurnstileCaptchaIdentifier(),
        new ImageToCoordinatesIdentifier()
    };

    public static async Task<List<CaptchaType>> IdentifyCaptchaAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
    {
        var identifiedTypes = new List<CaptchaType>();
        foreach (var captchaIdentifier in CaptchaIdentifiers)
        {
            var identifiedCaptcha = await captchaIdentifier.IdentifyInAllFramesAsync(driver, imageElement, proxyConfig, cancellationToken);
            
            if (identifiedCaptcha != null)
            {
                identifiedTypes.Add(identifiedCaptcha.Value);
            }
        }
        
        return identifiedTypes;
    }

    public static RecaptchaIdentifier GetRecaptchaIdentifier()
    {
        return (RecaptchaIdentifier)CaptchaIdentifiers.First(x => x.GetType() == typeof(RecaptchaIdentifier));
    }

    public static ICaptchaIdentifier GetSpecificIdentifier(CaptchaType captchaType)
    {
        return CaptchaIdentifiers.First(x => x.CanIdentify(captchaType));
    }

    public static bool CanIdentifyCaptcha(CaptchaType captchaType)
    {
        return CaptchaIdentifiers.Any(x => x.CanIdentify(captchaType));
    }

    private static async Task<CaptchaType?> IdentifyCaptchaAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig, CaptchaType captchaType, CancellationToken cancellationToken)
    {
        foreach (var captchaIdentifier in CaptchaIdentifiers.Where(x => x.CanIdentify(captchaType)))
        {
            var result = await captchaIdentifier.SpecifyCaptcha(captchaType, driver, imageElement, proxyConfig, cancellationToken);
            if (result.HasValue)
            {
                return result.Value;
            }
        }
        if (proxyConfig == null || string.IsNullOrEmpty(proxyConfig.ProxyAddress))
        {
            return captchaType;
        }

        return captchaType.ToProxyType();
    }

    public static async Task<CaptchaType?> IdentifyCaptchaAsync<TSolution>(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig, CancellationToken cancellationToken)
        where TSolution : BaseSolution, new()
    {
        return await IdentifyCaptchaAsync(driver, imageElement, proxyConfig, new TSolution().GetCaptchaType(), cancellationToken); 
    }
}