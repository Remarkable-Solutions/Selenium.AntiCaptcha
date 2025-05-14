using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class ReCaptchaV2ProxylessSolver : RecaptchaSolverBase <IRecaptchaV2ProxylessRequest>
    {
        protected override IRecaptchaV2ProxylessRequest BuildRequest(SolverArguments arguments)
        {
            return new RecaptchaV2ProxylessRequest(arguments);
        }

        public ReCaptchaV2ProxylessSolver(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
        {
        }
    }
}
