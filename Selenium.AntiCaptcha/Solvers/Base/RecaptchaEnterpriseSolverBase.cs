using System.Text.RegularExpressions;
using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.Solvers.Base;

internal abstract class RecaptchaEnterpriseSolverBase<TRequest> : RecaptchaSolverBase <TRequest>
    where TRequest : ICaptchaRequest<RecaptchaSolution>
{
    private readonly string[] _reservedFieldNames = { "size", "theme" };
    protected RecaptchaEnterpriseSolverBase(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
    {
    }

    protected override async Task<SolverArguments> FillMissingSolverArguments(SolverArguments solverArguments)
    {
        return (await base.FillMissingSolverArguments(solverArguments)) with
        {
            EnterprisePayload = solverArguments.EnterprisePayload ?? GetEnterprisePayload()
        };
    }

    private Dictionary<string, string> GetEnterprisePayload()
    {
        var result = new Dictionary<string, string>();
        var recaptchaFramesSources = Driver
            .FindManyByXPathAllFrames("//iframe[contains(@src, 'recaptcha') and contains(@src, 'enterprise')]")
            .Select(x => x.GetAttribute("src"))
            .ToList();

        var regex = new Regex(@"(?:(?:(\w+)=([\w-]+)))");
        foreach (var recaptchaFrameSource in recaptchaFramesSources)
        {
            var matches = regex.Matches(recaptchaFrameSource);

            foreach (var match in matches.Where(m => m.Success))
            {
                var matchValue = match.Value.Split("=");
                var key = matchValue[0];
                var value = matchValue[1];
                if (!_reservedFieldNames.Contains(key) && !result.ContainsKey(key))
                {
                    result.Add(key, value);
                }
            }

        }
        return result;

    }

}