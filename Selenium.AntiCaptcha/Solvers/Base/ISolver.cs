using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Responses;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Models;

namespace Selenium.AntiCaptcha.Solvers.Base;


public interface ISolver<TSolution> : ISolver
    where TSolution : BaseSolution, new()
{
    public Task<TaskResultResponse<TSolution>> SolveAsync(SolverArguments arguments, ActionArguments actionArguments,
        CancellationToken cancellationToken);
    public void Configure(IWebDriver driver, SolverConfig solverConfig);
}

public interface ISolver { }
