using AntiCaptchaApi.Net.Models;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Responses;
using AntiCaptchaApi.Net.Responses.Abstractions;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Exceptions;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;

namespace Selenium.AntiCaptcha
{
    public static class IWebDriverExtensions
    {
        public static async Task<BaseResponse> SolveCaptchaAsync(this IWebDriver driver,
            string clientKey,
            SolverArguments? solverArguments = null,
            ActionArguments? actionArguments = null,
            SolverConfig? solverConfig = null,
            CancellationToken cancellationToken = default)
        {
            solverArguments ??= new SolverArguments();
            var captchaType = solverArguments.CaptchaType ?? await IdentifyCaptcha(driver, solverArguments, cancellationToken);
            dynamic solver = SolverFactory.GetSolver(driver, clientKey, captchaType, solverConfig ?? new DefaultSolverConfig());
            return await solver.SolveAsync(solverArguments, actionArguments ?? new ActionArguments(), cancellationToken);
        }

        private static async Task<CaptchaType> IdentifyCaptcha(IWebDriver driver, SolverArguments arguments, CancellationToken cancellationToken = default)
        {
            var identifiedCaptchaTypes = await driver.IdentifyCaptchaAsync(arguments.ImageElement, arguments.ProxyConfig, cancellationToken);

            if (identifiedCaptchaTypes.Count != 1)
                throw new UnidentifiableCaptchaException(identifiedCaptchaTypes);

            return identifiedCaptchaTypes.First();
        }

        public static async Task<TaskResultResponse<TSolution>> SolveCaptchaAsync<TSolution>(
            this IWebDriver driver,
            string clientKey,
            SolverArguments? solverArguments = null,
            ActionArguments? actionArguments = null,
            SolverConfig? solverConfig = null,
            CancellationToken cancellationToken = default) where TSolution : BaseSolution, new()
        {
            solverArguments ??= new SolverArguments();
            var captchaType = solverArguments.CaptchaType ?? await driver.IdentifyCaptchaAsync<TSolution>(solverArguments.ImageElement, solverArguments.ProxyConfig, cancellationToken);

            if (!captchaType.HasValue)
            {
                throw new InsufficientSolverArgumentsException("Could not identify the captcha type from arguments. Please provide captchaType or better solver arguments to enable identification of captcha on site.");
            }

            ValidateSolutionOutputToCaptchaType<TSolution>(captchaType.Value);
            var solver = SolverFactory.GetSolver<TSolution>(driver, clientKey, captchaType.Value, solverConfig ?? new DefaultSolverConfig());
            return await solver.SolveAsync(solverArguments, actionArguments: actionArguments ?? new ActionArguments(), cancellationToken);
        }

        private static void ValidateSolutionOutputToCaptchaType<TSolution>(CaptchaType captchaType)
            where TSolution : BaseSolution, new()
        {
            var wrongSolutionTypeMessage = $"Wrong solution type. Was trying to create solution of type: {typeof(TSolution)} with captcha type: {captchaType}";
            var captchaSolutionType = captchaType.GetSolutionType();

            if (typeof(TSolution) != captchaSolutionType)
            {
                throw new ArgumentException(wrongSolutionTypeMessage);
            }
        }

        public static async Task<CaptchaType?> IdentifyCaptchaAsync<TSolution>(this IWebDriver driver, IWebElement? imageElement = default, ProxyConfig? proxyConfig = default, CancellationToken cancellationToken = default)
            where TSolution : BaseSolution, new()
        {
            return await CaptchaIdentifier.IdentifyCaptchaAsync<TSolution>(driver, imageElement, proxyConfig, cancellationToken);
        }

        public static async Task<List<CaptchaType>> IdentifyCaptchaAsync(this IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig,
            CancellationToken cancellationToken)
        {
            return await CaptchaIdentifier.IdentifyCaptchaAsync(driver, imageElement, proxyConfig, cancellationToken);
        }
    }
}