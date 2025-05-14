using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class TurnstileCaptchaSolver : TurnstileSolverBase <ITurnstileCaptchaRequest>
    {
        protected override ITurnstileCaptchaRequest BuildRequest(SolverArguments arguments)
        {
            return new TurnstileCaptchaRequest(arguments);
        }

        public TurnstileCaptchaSolver(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
        {
        }
    }
}
