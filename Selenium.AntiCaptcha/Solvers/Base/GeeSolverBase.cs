using System.Text.RegularExpressions;
using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.Solvers.Base;

public abstract class GeeSolverBase<TRequest, TSolution> : Solver <TRequest, TSolution>
    where TRequest: ICaptchaRequest<TSolution>
    where TSolution: BaseSolution, new()
{
    protected GeeSolverBase(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig)
        : base(anticaptchaClient, driver, solverConfig)
    {
    }
    
    protected override async Task<SolverArguments> FillMissingSolverArguments(
        SolverArguments solverArguments)
    {
        return await base.FillMissingSolverArguments(solverArguments)
            with
            {
                Gt = solverArguments.Gt ?? await AcquireGt(),
                Challenge = solverArguments.Challenge ?? GetChallenge(Driver)
            };
    }

    private string GetGt()
    {   
        var pageSource = Driver.GetAllPageSource();

        var patterns = new List<string>
        {
            "gt=(\\w{32}?)",
            "\"gt\"\\W+\"(\\w{32})\"",
            "captcha_id=(\\w{32}?)",
            "\"captcha_id\"\\W+\"(\\w{32})\"",
        };

        var result = pageSource.GetFirstRegexThatFits(true, patterns.ToArray());
        return result != null ? result.Groups[1].Value : string.Empty;
    }

    private async Task<string> AcquireGt()
    {
        return await AcquireElementValue(GetGt);
    }
        
    private string GetChallenge(IWebDriver driver)
    {
        var regex = new Regex("challenge=(.*?)&");
        return regex.Match(driver.GetAllPageSource()).Groups[1].Value;
    }

}