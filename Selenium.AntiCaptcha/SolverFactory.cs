﻿using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Internal.Extensions;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha;

internal static class SolverFactory
{
    public static ISolver<CaptchaRequest<TSolution>, TSolution> GetSolver<TSolution>(IWebDriver webDriver, string clientKey, CaptchaType captchaType, SolverConfig solverConfig)
        where TSolution : BaseSolution, new()
    {
        var solutionType = captchaType.GetSolutionType();

        if (typeof(TSolution) != solutionType)
        {
            throw new ArgumentException("Wrong solution type chosen to captcha type.");
        }

        var solv = GetSolver(webDriver, clientKey, captchaType, solverConfig);
        
        return GetSolver(webDriver, clientKey, captchaType, solverConfig) as ISolver<CaptchaRequest<TSolution>, TSolution>;
    }
    internal static ISolver GetSolver(IWebDriver webDriver, string clientKey, CaptchaType captchaType, SolverConfig solverConfig)
    {
        return captchaType switch
        {
            CaptchaType.ReCaptchaV2 => new ReCaptchaV2Solver(clientKey, webDriver, solverConfig),
            CaptchaType.ReCaptchaV2Proxyless => new ReCaptchaV2ProxylessSolver(clientKey, webDriver, solverConfig),
            CaptchaType.ReCaptchaV2Enterprise => new ReCaptchaV2EnterpriseSolver(clientKey, webDriver, solverConfig),
            CaptchaType.ReCaptchaV2EnterpriseProxyless => new ReCaptchaV2EnterpriseProxylessSolver(clientKey, webDriver, solverConfig),
            CaptchaType.ReCaptchaV3 => new RecaptchaV3Solver(clientKey, webDriver, solverConfig),
            CaptchaType.ReCaptchaV3Enterprise => new ReCaptchaV3EnterpriseSolver(clientKey, webDriver, solverConfig),
            CaptchaType.HCaptcha => new HCaptchaSolver(clientKey, webDriver, solverConfig),
            CaptchaType.HCaptchaProxyless => new HCaptchaProxylessSolver(clientKey, webDriver, solverConfig),
            CaptchaType.FunCaptcha => new FunCaptchaSolver(clientKey, webDriver, solverConfig),
            CaptchaType.FunCaptchaProxyless => new FunCaptchaProxylessSolver(clientKey, webDriver, solverConfig),
            CaptchaType.ImageToText => new ImageToTextSolver(clientKey, webDriver, solverConfig),
            CaptchaType.GeeTestV3 => new GeeTestV3Solver(clientKey, webDriver, solverConfig),
            CaptchaType.GeeTestV3Proxyless => new GeeTestV3ProxylessSolver(clientKey, webDriver, solverConfig),
            CaptchaType.GeeTestV4 => new GeeTestV4Solver(clientKey, webDriver, solverConfig),
            CaptchaType.GeeTestV4Proxyless => new GeeTestV4ProxylessSolver(clientKey, webDriver, solverConfig),
            CaptchaType.AntiGate => new AntiGateSolver(clientKey, webDriver, solverConfig),
            CaptchaType.Turnstile => new TurnstileCaptchaSolver(clientKey, webDriver, solverConfig),
            CaptchaType.TurnstileProxyless => new TurnstileProxylessCaptchaSolver(clientKey, webDriver, solverConfig),
            _ => throw new ArgumentOutOfRangeException(nameof(captchaType), captchaType, null)
        };
    }
}