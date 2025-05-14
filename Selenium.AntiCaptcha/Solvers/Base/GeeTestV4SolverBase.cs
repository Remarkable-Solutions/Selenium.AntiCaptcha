using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;

namespace Selenium.AntiCaptcha.Solvers.Base;

public abstract class GeeTestV4SolverBase<TRequest> : GeeSolverBase <TRequest, GeeTestV4Solution>
    where TRequest : ICaptchaRequest<GeeTestV4Solution>
{
    protected GeeTestV4SolverBase(IAnticaptchaClient anticaptchaClient, IWebDriver driver, SolverConfig solverConfig) : base(anticaptchaClient, driver, solverConfig)
    {
    }
}