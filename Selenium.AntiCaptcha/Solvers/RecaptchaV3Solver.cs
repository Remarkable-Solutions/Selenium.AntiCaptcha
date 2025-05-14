using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class RecaptchaV3Solver : RecaptchaSolverBase<IRecaptchaV3Request>
    {
        protected override IRecaptchaV3Request BuildRequest(SolverArguments arguments)
        {
            return new RecaptchaV3Request(arguments);
        }

        protected override async Task<SolverArguments> FillMissingSolverArguments(
            SolverArguments solverArguments)
        {
            return await base.FillMissingSolverArguments(solverArguments) with
            {
                IsEnterprise = solverArguments.IsEnterprise ?? false
            };
        }

        public RecaptchaV3Solver(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
        {
        }
    }
}
