using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    public class FunCaptchaProxylessSolver : FunCaptchaSolverBase<IFunCaptchaProxylessRequest>
    {
        protected override IFunCaptchaProxylessRequest BuildRequest(SolverArguments arguments)
        {
            return new FunCaptchaProxylessRequest(arguments);
        }


        public FunCaptchaProxylessSolver(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
        {
        }
    }
}
