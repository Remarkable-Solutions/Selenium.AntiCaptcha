using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class ReCaptchaV2Solver : RecaptchaSolverBase <IRecaptchaV2Request>
    {
        protected override IRecaptchaV2Request BuildRequest(SolverArguments arguments)
        {
            return new RecaptchaV2Request(arguments);
        }

        public ReCaptchaV2Solver(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
        {
        }
    }
}
