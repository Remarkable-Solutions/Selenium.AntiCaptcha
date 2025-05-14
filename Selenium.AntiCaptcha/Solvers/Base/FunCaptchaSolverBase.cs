using System.Text.RegularExpressions;
using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Extensions;
using Selenium.AntiCaptcha.Models;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.Solvers.Base;

public abstract class FunCaptchaSolverBase<TRequest> : Solver<TRequest, FunCaptchaSolution>
    where TRequest : ICaptchaRequest<FunCaptchaSolution>
{
    protected override string GetSiteKey()
    {
        return Driver.FindFunCaptchaSiteKey();
    }

    protected override async Task<SolverArguments> FillMissingSolverArguments(SolverArguments solverArguments)
    {
        return (await base.FillMissingSolverArguments(solverArguments)) with
        {
            FunCaptchaApiJsSubdomain = solverArguments.FunCaptchaApiJsSubdomain ?? await AcquireFunCaptchaJsSubdomain(),
            Data = solverArguments.Data ?? await AcquireBlobData(),
        };
    }


    private async Task<string?> AcquireFunCaptchaJsSubdomain()
    {
        var scriptSources = Driver.FindManyValuesByXPathAllFrames("src", "//script[contains(@src, 'arkose')]")
            .Distinct()
            .ToList();
        var subdomainRegexPattern = @"(?:https?:)?\/\/((?!client\-api).+\.arkoselabs\.com)(?:.+funcaptcha_api\.js)";
        var regex = new Regex(subdomainRegexPattern);
        var subdomainMatches = scriptSources.Select(src => regex.Match(src)).Where(m => m.Success).Distinct().ToList();
        return subdomainMatches.Any() ? subdomainMatches.First().Groups[1].Value : null;
    }

    private async Task<string?> AcquireBlobData()
    {
        var iframeSources = Driver.FindManyValuesByXPathAllFrames("src", "//iframe[contains(@src, 'arkose')]")
            .Distinct()
            .ToList();
        
        var fieldsRegexPattern = @"(?:(?:(\w+)=([\.\%\w-]+)))";
        var regex = new Regex(fieldsRegexPattern);
        
        foreach (var frameSource in iframeSources)
        {
            var matches = regex.Matches(frameSource);

            foreach (var match in matches.Where(m => m.Success))
            {
                var matchValue = match.Value.Split("=");
                var key = matchValue[0];
                var value = matchValue[1];

                if (key.ToLower().Contains("blob"))
                {
                    return JsonConvert.SerializeObject(new Dictionary<string, string>()
                    {
                        {"blob", value}
                    });
                }
            }

        }

        return null;
    }
    
    protected override async Task FillResponseElement(FunCaptchaSolution solution, ActionArguments actionArguments)
    {
        if (actionArguments.ResponseElement != null)
        {
            actionArguments.ResponseElement.SendKeys(solution.Token);
        }
        else if(actionArguments.ShouldFindAndFillAccordingResponseElements)
        {
            try
            {
                var recaptchaElementIds = Driver
                    .FindManyValuesByXPathAllFrames(
                        "id",
                        "//input[@id='fc-token' or @id='FunCaptcha-Token' or @id='verification-token']") // verification-token
                    .Distinct()
                    .ToList();


                foreach (var elementId in recaptchaElementIds)
                {
                    try
                    {
                        await Driver.SetValueForElementWithIdInAllFrames(elementId, solution.Token);
                    }
                    catch (Exception)
                    {
                        // ignore
                    }
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }

    protected FunCaptchaSolverBase(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
    {
    }
}