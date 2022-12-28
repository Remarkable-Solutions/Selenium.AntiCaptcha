﻿using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class FunCaptchaSolver : FunCaptchaSolverBase<IFunCaptchaRequest>
    {
        protected override IFunCaptchaRequest BuildRequest(SolverArguments arguments)
        {
            return new FunCaptchaRequest(arguments);
        }

        public FunCaptchaSolver(string clientKey, IWebDriver driver, SolverConfig solverConfig) : base(clientKey, driver, solverConfig)
        {
        }
    }
}
