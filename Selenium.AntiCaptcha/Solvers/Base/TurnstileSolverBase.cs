using System.Text.RegularExpressions;
using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.Solvers.Base;

public abstract class TurnstileSolverBase<TRequest> : Solver <TRequest, TurnstileSolution>
    where TRequest : ICaptchaRequest<TurnstileSolution>
{
    
    protected TurnstileSolverBase(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
    {
    }
    
    
    protected override string GetSiteKey()
    {
        var turnstileFrame = GetTurnstileIFrame();
        var src = turnstileFrame?.GetAttribute("src");
            
        if (!string.IsNullOrEmpty(src))
        {
            var regex = new Regex("[a-fA-F0-9]x[a-fA-F0-9]{22}");
            var siteKey = regex.Match(src).Value;

            if (!string.IsNullOrEmpty(siteKey))
                return siteKey;
        }

        return string.Empty;
    }

    private IWebElement? GetTurnstileIFrame()
    {
        return Driver.FindByXPathAllFrames("//iframe[contains(@src, 'turnstile')]");
    }
}