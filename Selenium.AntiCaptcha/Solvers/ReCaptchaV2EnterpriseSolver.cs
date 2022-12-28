﻿using AntiCaptchaApi.Net.Requests;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Solvers.Base;

namespace Selenium.AntiCaptcha.Solvers
{
    internal class ReCaptchaV2EnterpriseSolver : RecaptchaSolverBase <IRecaptchaV2EnterpriseRequest>
    {
        protected override IRecaptchaV2EnterpriseRequest BuildRequest(SolverArguments arguments)
        {
            return new RecaptchaV2EnterpriseRequest(arguments);
        }

        public ReCaptchaV2EnterpriseSolver(string clientKey, IWebDriver driver, SolverConfig solverConfig) : base(clientKey, driver, solverConfig)
        {
        }
    }
}
