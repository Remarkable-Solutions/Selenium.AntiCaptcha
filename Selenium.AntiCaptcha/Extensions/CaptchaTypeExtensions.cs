using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Constants;
using Selenium.AntiCaptcha.Enums;

namespace Selenium.AntiCaptcha.Extensions;

public static class CaptchaTypeExtensions
{
    public static bool IsProperlyDefined(this CaptchaType type)
    {
        return CaptchaTypeGroups.ProxyCaptchaTypes.Contains(type) || CaptchaTypeGroups.ProxylessCaptchaTypes.Contains(type);
    }

    public static bool IsRecaptcha(this CaptchaType type)
    {
        return CaptchaTypeGroups.ReCaptchaTypes.Contains(type);
    }
    
    public static CaptchaType ToProxyType(this CaptchaType type)
    {
        return type switch
        {
            CaptchaType.ReCaptchaV2Proxyless => CaptchaType.ReCaptchaV2,
            CaptchaType.ReCaptchaV2EnterpriseProxyless => CaptchaType.ReCaptchaV2Enterprise,
            CaptchaType.ReCaptchaV2Enterprise => CaptchaType.ReCaptchaV2Enterprise,
            CaptchaType.ReCaptchaV3 => CaptchaType.ReCaptchaV3,
            CaptchaType.ReCaptchaV2 => CaptchaType.ReCaptchaV2,
            CaptchaType.ImageToCoordinates => CaptchaType.ImageToCoordinates,
            CaptchaType.FunCaptcha => CaptchaType.FunCaptcha,
            CaptchaType.FunCaptchaProxyless => CaptchaType.FunCaptcha,
            CaptchaType.ImageToText => CaptchaType.ImageToText,
            CaptchaType.GeeTestV3 => CaptchaType.GeeTestV3,
            CaptchaType.GeeTestV4 => CaptchaType.GeeTestV4,
            CaptchaType.GeeTestV3Proxyless => CaptchaType.GeeTestV3,
            CaptchaType.GeeTestV4Proxyless => CaptchaType.GeeTestV4,
            CaptchaType.AntiGate => CaptchaType.AntiGate,
            CaptchaType.ReCaptchaV3Enterprise => CaptchaType.ReCaptchaV3Enterprise,
            CaptchaType.Turnstile => CaptchaType.Turnstile,
            CaptchaType.TurnstileProxyless => CaptchaType.Turnstile,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static bool IsProxyType(this CaptchaType captchaType)
    {
        return CaptchaTypeGroups.ProxyCaptchaTypes.Contains(captchaType);
    }
    
    public static bool IsProxylessType(this CaptchaType captchaType)
    {
        return CaptchaTypeGroups.ProxylessCaptchaTypes.Contains(captchaType);
    }
    
    public static Type GetSolutionType(this CaptchaType captchaType)
    {
        return captchaType switch
        {
            CaptchaType.ReCaptchaV2 => typeof(RecaptchaSolution),
            CaptchaType.ReCaptchaV2Proxyless => typeof(RecaptchaSolution),
            CaptchaType.ReCaptchaV2Enterprise => typeof(RecaptchaSolution),
            CaptchaType.ReCaptchaV2EnterpriseProxyless => typeof(RecaptchaSolution),
            CaptchaType.ReCaptchaV3 => typeof(RecaptchaSolution),
            CaptchaType.ReCaptchaV3Enterprise => typeof(RecaptchaSolution),
            CaptchaType.FunCaptcha => typeof(FunCaptchaSolution),
            CaptchaType.FunCaptchaProxyless => typeof(FunCaptchaSolution),
            CaptchaType.ImageToText => typeof(ImageToTextSolution),
            CaptchaType.GeeTestV3 => typeof(GeeTestV3Solution),
            CaptchaType.GeeTestV3Proxyless => typeof(GeeTestV3Solution),
            CaptchaType.GeeTestV4 => typeof(GeeTestV4Solution),
            CaptchaType.GeeTestV4Proxyless => typeof(GeeTestV4Solution),
            CaptchaType.AntiGate => typeof(AntiGateSolution),
            CaptchaType.Turnstile => typeof(TurnstileSolution),
            CaptchaType.TurnstileProxyless => typeof(TurnstileSolution),
            CaptchaType.ImageToCoordinates => typeof(ImageToCoordinatesSolution),
            _ => throw new ArgumentOutOfRangeException(nameof(captchaType), captchaType, null)
        };
    }

}