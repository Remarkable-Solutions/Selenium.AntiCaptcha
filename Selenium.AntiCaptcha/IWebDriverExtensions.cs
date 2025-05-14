using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Responses;
using AntiCaptchaApi.Net.Responses.Abstractions;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Exceptions;
using Selenium.AntiCaptcha.Models;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Selenium.AntiCaptcha
{
    /// <summary>
    /// Extension methods for Selenium WebDriver to solve various types of CAPTCHAs
    /// </summary>
    public static class IWebDriverExtensions
    {
        /// <summary>
        /// Automatically solves any detected CAPTCHA on the current page
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="anticaptchaClient">The Anti-Captcha client with API key</param>
        /// <param name="solverArguments">Optional arguments for the solver</param>
        /// <param name="actionArguments">Optional arguments for post-solving actions</param>
        /// <param name="solverConfig">Optional configuration for the solver</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The response from Anti-Captcha service</returns>
        public static async Task<BaseResponse> SolveCaptchaAsync(this IWebDriver driver,
            IAnticaptchaClient anticaptchaClient,
            SolverArguments? solverArguments = null,
            ActionArguments? actionArguments = null,
            SolverConfig? solverConfig = null,
            CancellationToken cancellationToken = default)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));
            if (anticaptchaClient == null) throw new ArgumentNullException(nameof(anticaptchaClient));
            
            solverArguments ??= new SolverArguments();
            actionArguments ??= new ActionArguments();
            solverConfig ??= new DefaultSolverConfig();
            
            int retryCount = 0;
            int maxRetries = actionArguments.MaxRetries;

            while (true)
            {
                try
                {
                    var captchaType = solverArguments.CaptchaType ?? await IdentifyCaptcha(driver, solverArguments, cancellationToken);
                    dynamic solver = SolverFactory.GetSolver(driver, anticaptchaClient, captchaType, solverConfig);
                    return await solver.SolveAsync(solverArguments, actionArguments, cancellationToken);
                }
                catch (Exception ex) when (!(ex is OperationCanceledException) && retryCount < maxRetries)
                {
                    retryCount++;
                    await Task.Delay(1000 * retryCount, cancellationToken); // Progressive backoff
                }
            }
        }

        private static async Task<CaptchaType> IdentifyCaptcha(IWebDriver driver, SolverArguments arguments, CancellationToken cancellationToken = default)
        {
            var identifiedCaptchaTypes = await driver.IdentifyCaptchaAsync(arguments.ImageElement, arguments.ProxyConfig, cancellationToken);

            if (identifiedCaptchaTypes.Count == 0)
                throw new UnidentifiableCaptchaException("No CAPTCHA could be identified on the page");
                
            if (identifiedCaptchaTypes.Count > 1)
                throw new UnidentifiableCaptchaException(identifiedCaptchaTypes, "Multiple CAPTCHA types detected. Please specify the exact type.");

            return identifiedCaptchaTypes.First();
        }

        /// <summary>
        /// Solves a specific type of CAPTCHA with strongly typed solution
        /// </summary>
        /// <typeparam name="TSolution">The type of solution expected</typeparam>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="anticaptchaClient">The Anti-Captcha client with API key</param>
        /// <param name="solverArguments">Optional arguments for the solver</param>
        /// <param name="actionArguments">Optional arguments for post-solving actions</param>
        /// <param name="solverConfig">Optional configuration for the solver</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The task result with the solution</returns>
        public static async Task<TaskResultResponse<TSolution>> SolveCaptchaAsync<TSolution>(
            this IWebDriver driver,
            IAnticaptchaClient anticaptchaClient,
            SolverArguments? solverArguments = null,
            ActionArguments? actionArguments = null,
            SolverConfig? solverConfig = null,
            CancellationToken cancellationToken = default) where TSolution : BaseSolution, new()
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));
            if (anticaptchaClient == null) throw new ArgumentNullException(nameof(anticaptchaClient));
            
            solverArguments ??= new SolverArguments();
            actionArguments ??= new ActionArguments();
            solverConfig ??= new DefaultSolverConfig();
            
            int retryCount = 0;
            int maxRetries = actionArguments.MaxRetries;

            while (true)
            {
                try
                {
                    var captchaType = solverArguments.CaptchaType ?? 
                        await driver.IdentifyCaptchaAsync<TSolution>(solverArguments.ImageElement, solverArguments.ProxyConfig, cancellationToken);

                    if (!captchaType.HasValue)
                    {
                        throw new InsufficientSolverArgumentsException(
                            "Could not identify the captcha type. Please provide CaptchaType explicitly or ensure the page contains a recognizable CAPTCHA.");
                    }

                    ValidateSolutionOutputToCaptchaType<TSolution>(captchaType.Value);
                    var solver = SolverFactory.GetSolver<TSolution>(driver, anticaptchaClient, captchaType.Value, solverConfig);
                    return await solver.SolveAsync(solverArguments, actionArguments, cancellationToken);
                }
                catch (CaptchaSolverTimeoutException) when (retryCount < maxRetries)
                {
                    // Retry on timeout
                    retryCount++;
                    await Task.Delay(1000 * retryCount, cancellationToken);
                }
                catch (Exception ex) when (!(ex is OperationCanceledException || ex is ArgumentException || ex is InsufficientSolverArgumentsException) && retryCount < maxRetries)
                {
                    // Retry on other exceptions except for validation errors
                    retryCount++;
                    await Task.Delay(1000 * retryCount, cancellationToken);
                }
                
                // Break the loop if we've reached max retries or if an exception that shouldn't be retried was thrown
                if (retryCount >= maxRetries)
                    break;
            }
            
            // If we get here, all retries failed - last exception will be propagated
            var finalCaptchaType = solverArguments.CaptchaType ?? 
                await driver.IdentifyCaptchaAsync<TSolution>(solverArguments.ImageElement, solverArguments.ProxyConfig, cancellationToken);
                
            if (!finalCaptchaType.HasValue)
            {
                throw new InsufficientSolverArgumentsException(
                    "Could not identify the captcha type after multiple retries. Please provide CaptchaType explicitly.");
            }

            ValidateSolutionOutputToCaptchaType<TSolution>(finalCaptchaType.Value);
            var finalSolver = SolverFactory.GetSolver<TSolution>(driver, anticaptchaClient, finalCaptchaType.Value, solverConfig);
            return await finalSolver.SolveAsync(solverArguments, actionArguments, cancellationToken);
        }

        private static void ValidateSolutionOutputToCaptchaType<TSolution>(CaptchaType captchaType)
            where TSolution : BaseSolution, new()
        {
            var captchaSolutionType = captchaType.GetSolutionType();

            if (typeof(TSolution) != captchaSolutionType)
            {
                throw new ArgumentException(
                    $"Solution type mismatch. The CAPTCHA type {captchaType} requires solution type {captchaSolutionType.Name}, but {typeof(TSolution).Name} was requested.");
            }
        }

        /// <summary>
        /// Identifies the CAPTCHA type for a specific solution type
        /// </summary>
        /// <typeparam name="TSolution">The type of solution expected</typeparam>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="imageElement">Optional image element for image-based CAPTCHAs</param>
        /// <param name="proxyConfig">Optional proxy configuration</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The identified CAPTCHA type, or null if not identified</returns>
        public static async Task<CaptchaType?> IdentifyCaptchaAsync<TSolution>(
            this IWebDriver driver, 
            IWebElement? imageElement = default, 
            ProxyConfig? proxyConfig = default, 
            CancellationToken cancellationToken = default)
            where TSolution : BaseSolution, new()
        {
            return await CaptchaIdentifier.IdentifyCaptchaAsync<TSolution>(driver, imageElement, proxyConfig, cancellationToken);
        }

        /// <summary>
        /// Identifies all CAPTCHA types on the current page
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="imageElement">Optional image element for image-based CAPTCHAs</param>
        /// <param name="proxyConfig">Optional proxy configuration</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of identified CAPTCHA types</returns>
        public static async Task<List<CaptchaType>> IdentifyCaptchaAsync(
            this IWebDriver driver, 
            IWebElement? imageElement, 
            ProxyConfig? proxyConfig,
            CancellationToken cancellationToken)
        {
            return await CaptchaIdentifier.IdentifyCaptchaAsync(driver, imageElement, proxyConfig, cancellationToken);
        }
    }
}