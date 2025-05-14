using System.Text.RegularExpressions;
using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.Solvers.Base;

internal abstract class RecaptchaSolverBase<TRequest> : Solver <TRequest, RecaptchaSolution>
    where TRequest : ICaptchaRequest<RecaptchaSolution>
{
    protected override string GetSiteKey()
    {
        try
        {
            return Driver.FindElement(By.ClassName("g-recaptcha")).GetAttribute("data-sitekey");
        }
        catch (Exception)
        {
            // ignored
        }

        
        var recaptchaFrameSrc = Driver.FindByXPathAllFrames("//iframe[contains(@src, 'recaptcha')]")?.GetAttribute("src");


        if (!string.IsNullOrEmpty(recaptchaFrameSrc))
        {
            var regex = new Regex("k=(.*?)&");
            var siteKey = regex.Match(recaptchaFrameSrc).Groups[1].Value;

            if (!string.IsNullOrEmpty(siteKey))
                return siteKey;
        }

        return string.Empty;
    }
    
    
    protected override async Task FillResponseElement(RecaptchaSolution solution, ActionArguments actionArguments)
    {
        if (actionArguments.ResponseElement != null)
        {
            actionArguments.ResponseElement.SendKeys(solution.GRecaptchaResponse);
        }
        else if(actionArguments.ShouldFindAndFillAccordingResponseElements)
        {
            try
            {
                var recaptchaElementIds = Driver
                    .FindManyValuesByXPathAllFrames(
                        "id",
                        "//textarea[contains(@id, 'g-recaptcha-response')]")
                    .Distinct()
                    .ToList();


                foreach (var elementId in recaptchaElementIds)
                {
                    try
                    {
                        await Driver.SetValueForElementWithIdInAllFrames(elementId, solution.GRecaptchaResponse);
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

    protected RecaptchaSolverBase(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
    {
    }
}