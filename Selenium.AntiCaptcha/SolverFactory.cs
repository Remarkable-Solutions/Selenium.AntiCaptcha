using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha;

internal static class SolverFactory
{
    public static ISolver<TSolution> GetSolver<TSolution>(IWebDriver webDriver, IAnticaptchaClient anticaptchaClient, CaptchaType captchaType, SolverConfig solverConfig)
        where TSolution : BaseSolution, new()
    {
        var solutionType = captchaType.GetSolutionType();

        if (typeof(TSolution) != solutionType)
        {
            throw new ArgumentException("Wrong solution type chosen to captcha type.");
        }
        return (GetSolver(webDriver, anticaptchaClient, captchaType, solverConfig) as ISolver<TSolution>)!;
    }
    internal static ISolver GetSolver(IWebDriver webDriver, IAnticaptchaClient anticaptchaClient, CaptchaType captchaType, SolverConfig solverConfig)
    {
        return captchaType switch
        {
            CaptchaType.ReCaptchaV2 => new ReCaptchaV2Solver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.ReCaptchaV2Proxyless => new ReCaptchaV2ProxylessSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.ReCaptchaV2Enterprise => new ReCaptchaV2EnterpriseSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.ReCaptchaV2EnterpriseProxyless => new ReCaptchaV2EnterpriseProxylessSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.ReCaptchaV3 => new RecaptchaV3Solver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.ReCaptchaV3Enterprise => new ReCaptchaV3EnterpriseSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.FunCaptcha => new FunCaptchaSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.FunCaptchaProxyless => new FunCaptchaProxylessSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.ImageToText => new ImageToTextSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.GeeTestV3 => new GeeTestV3Solver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.GeeTestV3Proxyless => new GeeTestV3ProxylessSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.GeeTestV4 => new GeeTestV4Solver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.GeeTestV4Proxyless => new GeeTestV4ProxylessSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.Turnstile => new TurnstileCaptchaSolver(anticaptchaClient, webDriver, solverConfig),
            CaptchaType.TurnstileProxyless => new TurnstileProxylessCaptchaSolver(anticaptchaClient, webDriver, solverConfig),
            _ => throw new ArgumentOutOfRangeException(nameof(captchaType), captchaType, null)
        };
    }
}