using Selenium.AntiCaptcha.Enums;

namespace Selenium.AntiCaptcha.Constants;

public static class CaptchaTypeGroups
{
    public static readonly IReadOnlyList<CaptchaType> ProxyCaptchaTypes = new List<CaptchaType>
    {
        CaptchaType.FunCaptcha,
        CaptchaType.GeeTestV3,
        CaptchaType.GeeTestV4,
        CaptchaType.ReCaptchaV2,
        CaptchaType.ReCaptchaV2Enterprise,
        CaptchaType.Turnstile
    };

    public static readonly IReadOnlyList<CaptchaType> ProxylessCaptchaTypes = new List<CaptchaType>
    {
        CaptchaType.ReCaptchaV2Proxyless,
        CaptchaType.ReCaptchaV2EnterpriseProxyless,
        CaptchaType.ReCaptchaV3,
        CaptchaType.ReCaptchaV3Enterprise,
        CaptchaType.FunCaptchaProxyless,
        CaptchaType.ImageToText,
        CaptchaType.GeeTestV3Proxyless,
        CaptchaType.GeeTestV4Proxyless,
        CaptchaType.AntiGate,
        CaptchaType.TurnstileProxyless,
        CaptchaType.ImageToCoordinates
    };
    
    public static readonly IReadOnlyList<CaptchaType> ReCaptchaTypes = new List<CaptchaType>
    {
        CaptchaType.ReCaptchaV2,
        CaptchaType.ReCaptchaV2Proxyless,
        CaptchaType.ReCaptchaV2EnterpriseProxyless,
        CaptchaType.ReCaptchaV2Enterprise,
        CaptchaType.ReCaptchaV3,
        CaptchaType.ReCaptchaV3Enterprise,
    };
    
    public static readonly IReadOnlyList<CaptchaType> TurnstileTypes = new List<CaptchaType>
    {
        CaptchaType.Turnstile,
        CaptchaType.TurnstileProxyless,
    };
    
    public static readonly IReadOnlyList<CaptchaType> GeeTestTypes = new List<CaptchaType>
    {
        CaptchaType.GeeTestV3,
        CaptchaType.GeeTestV3Proxyless,
        CaptchaType.GeeTestV4,
        CaptchaType.GeeTestV4Proxyless,
    };
    
    public static readonly IReadOnlyList<CaptchaType> FunCaptchaTypes = new List<CaptchaType>
    {
        CaptchaType.FunCaptcha, 
        CaptchaType.FunCaptchaProxyless
    };
}