using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;

namespace Selenium.AntiCaptcha.Solvers.Base;

public abstract class GeeTestV3SolverBase<TRequest> : GeeSolverBase <TRequest, GeeTestV3Solution>
    where TRequest : ICaptchaRequest<GeeTestV3Solution>
{
    protected GeeTestV3SolverBase(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
    {
    }
}