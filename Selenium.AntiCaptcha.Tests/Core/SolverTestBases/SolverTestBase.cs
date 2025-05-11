using System.Diagnostics;
using System.Text;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Responses;
using AntiCaptchaApi.Net.Responses.Abstractions;
using Selenium.AntiCaptcha;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;
using Selenium.AntiCaptcha.Models;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core.Models;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.Core.SolverTestBases;

[Collection("Sequential")]
public abstract class SolverTestBase<TSolution> : WebDriverBasedTestBase
    where TSolution : BaseSolution, new()
{
    protected abstract string TestedUri { get; set; }
    protected abstract CaptchaType CaptchaType { get; set; }
    protected SolverArguments SolverArgumentsWithoutCaptchaType { get; set; }
    protected SolverArguments SolverArgumentsWithCaptchaType { get; set; }

    protected Stopwatch _stopwatch = new Stopwatch();

    protected SolverTestBase(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        ValidateCaptchaUri();
        
        SolverArgumentsWithoutCaptchaType = new SolverArguments();
        SolverArgumentsWithCaptchaType = new SolverArguments(CaptchaType);
        
        if (CaptchaType.IsProxyType())
        {
            SolverArgumentsWithoutCaptchaType.ProxyConfig = TestEnvironment.GetCurrentTestProxyConfig();
            SolverArgumentsWithCaptchaType.ProxyConfig = TestEnvironment.GetCurrentTestProxyConfig();
        }
    }

    protected virtual async Task BeforeTestAction()
    {
        
    }

    protected virtual async Task AfterTestAction()
    {
        
    }
    

    [Fact]
    public virtual async Task Solve_Generic_WithoutCaptchaTypeSpecified()
    {
        await Solve_Generic(SolverArgumentsWithoutCaptchaType);
    }

    [Fact]
    public virtual async Task Solve_Generic_WithCaptchaTypeSpecified()
    {
        await Solve_Generic(SolverArgumentsWithCaptchaType);
    }

    [Fact]
    public virtual async Task Solve_NonGeneric_WithoutCaptchaTypeSpecified()
    {
        await Solve_NonGeneric(SolverArgumentsWithoutCaptchaType);
    }
    [Fact]
    public virtual async Task Solve_NonGeneric_WithCaptchaTypeSpecified()
    {
        await Solve_NonGeneric(SolverArgumentsWithCaptchaType);
    }

    private async Task Solve_Generic(SolverArguments solverArguments)
    {
        _stopwatch.Start();
        await SetDriverUrl(TestedUri);
        DumpElapsedTimeToOutputAndRestartTimer(nameof(SetDriverUrl));
        await BeforeTestAction();
        DumpElapsedTimeToOutputAndRestartTimer(nameof(BeforeTestAction));
        var result = await Driver.SolveCaptchaAsync<TSolution>(ClientKey, solverArguments,  solverConfig: new DefaultSolverConfig());
        DumpElapsedTimeToOutputAndRestartTimer("Generic - SolveCaptchaAsync");
        AssertSolveCaptchaResult(result, expectedCaptchaType: CaptchaType);
        DumpElapsedTimeToOutputAndRestartTimer(nameof(AssertSolveCaptchaResult));
        await AfterTestAction();
        DumpElapsedTimeToOutputAndRestartTimer(nameof(AfterTestAction));
    }

    private void DumpElapsedTimeToOutputAndRestartTimer(string functionName)
    {
        _output.WriteLine("{0} took {1} ms", functionName, _stopwatch.ElapsedMilliseconds);
        _stopwatch.Restart();
    }

    private async Task Solve_NonGeneric(SolverArguments solverArguments)
    {
        _stopwatch.Start();
        await SetDriverUrl(TestedUri);
        DumpElapsedTimeToOutputAndRestartTimer(nameof(SetDriverUrl));
        await BeforeTestAction();
        DumpElapsedTimeToOutputAndRestartTimer(nameof(BeforeTestAction));
        var result = await Driver.SolveCaptchaAsync(ClientKey, solverArguments);
        DumpElapsedTimeToOutputAndRestartTimer("NonGeneric - SolveCaptchaAsync");
        AssertSolveCaptchaResult(result, expectedCaptchaType: CaptchaType);
        DumpElapsedTimeToOutputAndRestartTimer(nameof(AssertSolveCaptchaResult));
        await AfterTestAction();
        DumpElapsedTimeToOutputAndRestartTimer(nameof(AfterTestAction));
    }

    private void ValidateCaptchaUri()
    {
        var testUri = TestUris.Uris().FirstOrDefault(x => x.FirstOrDefault(y =>((CaptchaUri)y).Uri == TestedUri) != null)?.FirstOrDefault();

        if (testUri == null)
        {
            Assert.Fail("Tested Uri not found in the Testable Uris.");
        }

        var captchaUri = (CaptchaUri)testUri!;
        var expectedType = CaptchaType.IsProxyType() ? captchaUri.ExpectedType.ToProxyType() : captchaUri.ExpectedType;
        
        if (expectedType != CaptchaType)
        {
            Assert.Fail("Tested captcha uri expected captcha type do not match expected tested captcha type.");
        }
    }
    protected static void AssertSolveCaptchaResult<TSolution>(TaskResultResponse<TSolution>? result, CaptchaType expectedCaptchaType)
        where TSolution : BaseSolution, new()
    {
        Assert.NotNull(result);
        if (!string.IsNullOrEmpty(result!.ErrorDescription))
        {
            Assert.Fail(BuildErrorMessage(result));
        }
        
        Assert.True(result.Solution.IsValid());
        var expectedCaptchaTypeText = expectedCaptchaType.ToString();
        //TODO! There's task in the name. so it does not find it.
        // Assert.Contains($"\"{expectedCaptchaType.ToString()}\"", result.CreateTaskResponse.RawRequestPayload);
    }

    private static string BuildErrorMessage<TSolution>(TaskResultResponse<TSolution> result) where TSolution : BaseSolution, new()
    {
        var stringBuilder = new StringBuilder();

        AppendTitleWithValue(stringBuilder, nameof(TaskResultResponse<TSolution>.ErrorDescription), result.ErrorDescription);
        AppendTitleWithValue(stringBuilder, "Task result - " + nameof(TaskResultResponse<TSolution>.RawPayload), result.RawPayload);
        AppendTitleWithValue(stringBuilder, "Task result - " + nameof(TaskResultResponse<TSolution>.RawResponse), result.RawResponse);
        AppendTitleWithValue(stringBuilder, "Create Task - " + nameof(TaskResultResponse<TSolution>.CreateTaskResponse.RawPayload), result.CreateTaskResponse?.RawPayload);
        AppendTitleWithValue(stringBuilder, "Create Task - " + nameof(TaskResultResponse<TSolution>.CreateTaskResponse) + nameof(TaskResultResponse<TSolution>.CreateTaskResponse.RawResponse), result.CreateTaskResponse?.RawResponse);
        
        return stringBuilder.ToString();
    }

    private static void AppendTitleWithValue(StringBuilder stringBuilder, string? title, string? value)
    {
        if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value))
        {
            AppendTitle(stringBuilder, title);
            AppendWithNewLine(stringBuilder, value);   
        }
    }
    
    private static void AppendTitle(StringBuilder stringBuilder, string title)
    {
        AppendWithNewLine(stringBuilder, $"---------- {title} ----------");
    }
    
    private static void AppendWithNewLine(StringBuilder stringBuilder, string value)
    {
        stringBuilder.Append($"{Environment.NewLine}{value}{Environment.NewLine}");
    }

    protected static void AssertSolveCaptchaResult(BaseResponse result, CaptchaType expectedCaptchaType)
    {
        if (result is TaskResultResponse<RecaptchaSolution>)
            AssertSolveCaptchaResult((TaskResultResponse<RecaptchaSolution>?)result, expectedCaptchaType);
        if (result is TaskResultResponse<FunCaptchaSolution>)
            AssertSolveCaptchaResult((TaskResultResponse<FunCaptchaSolution>?)result, expectedCaptchaType);
        if (result is TaskResultResponse<GeeTestV3Solution>)
            AssertSolveCaptchaResult((TaskResultResponse<GeeTestV3Solution>?)result, expectedCaptchaType);
        if (result is TaskResultResponse<GeeTestV4Solution>)
            AssertSolveCaptchaResult((TaskResultResponse<GeeTestV4Solution>?)result, expectedCaptchaType);
        if (result is TaskResultResponse<AntiGateSolution>)
            AssertSolveCaptchaResult((TaskResultResponse<AntiGateSolution>?)result, expectedCaptchaType);
        if (result is TaskResultResponse<ImageToTextSolution>)
            AssertSolveCaptchaResult((TaskResultResponse<ImageToTextSolution>?)result, expectedCaptchaType);
        if (result is TaskResultResponse<TurnstileSolution>)
            AssertSolveCaptchaResult((TaskResultResponse<TurnstileSolution>?)result, expectedCaptchaType);
        if (result is TaskResultResponse<ImageToCoordinatesSolution>)
            AssertSolveCaptchaResult((TaskResultResponse<ImageToCoordinatesSolution>?)result, expectedCaptchaType);
    }
}