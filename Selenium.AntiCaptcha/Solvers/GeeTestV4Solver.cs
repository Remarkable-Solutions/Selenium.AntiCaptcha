using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class GeeTestV4Solver : GeeTestV4SolverBase<IGeeTestV4Request>
    {
        protected override IGeeTestV4Request BuildRequest(SolverArguments arguments)
        {
            return new GeeTestV4Request(arguments);
        }
        
        public GeeTestV4Solver(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
        {
        }
    }
}
