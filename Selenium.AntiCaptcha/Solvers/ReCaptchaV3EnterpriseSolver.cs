using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class ReCaptchaV3EnterpriseSolver : RecaptchaEnterpriseSolverBase<IRecaptchaV3EnterpriseRequest>
    {
        protected override IRecaptchaV3EnterpriseRequest BuildRequest(SolverArguments arguments)
        {
            return new RecaptchaV3EnterpriseRequest(arguments);
        }

        protected override async Task<SolverArguments> FillMissingSolverArguments(
            SolverArguments solverArguments)
        {
            return await base.FillMissingSolverArguments(solverArguments) with
            {
                IsEnterprise = solverArguments.IsEnterprise ?? true
            };
        }

        public ReCaptchaV3EnterpriseSolver(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
        {
        }
    }
}
