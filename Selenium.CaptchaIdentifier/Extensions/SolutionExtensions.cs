﻿using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.CaptchaIdentifier.Enums;

namespace Selenium.CaptchaIdentifier.Extensions;

internal static class SolutionExtensions
{
    public static CaptchaType GetCaptchaType<TSolution>(this TSolution solution)
        where TSolution : BaseSolution
    {            
        return solution.GetType().Name switch
        {
            nameof(GeeTestV4Solution) => CaptchaType.GeeTestV4Proxyless,
            nameof(GeeTestV3Solution) => CaptchaType.GeeTestV3Proxyless,
            nameof(RecaptchaSolution) => CaptchaType.ReCaptchaV2Proxyless,
            nameof(ImageToTextSolution) => CaptchaType.ImageToText,
            nameof(ImageToCoordinatesSolution) => CaptchaType.ImageToCoordinates,
            nameof(AntiGateSolution) => CaptchaType.AntiGate,
            nameof(FunCaptchaSolution) => CaptchaType.FunCaptchaProxyless,
            nameof(TurnstileSolution) => CaptchaType.TurnstileProxyless,
            _ => throw new ArgumentOutOfRangeException(solution.GetType().Name),
        };
    }
}