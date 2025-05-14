using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Models;
using Selenium.Anticaptcha.Tests.Core;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.FunctionalityTests;

public class WebDriverExtensionsTests : WebDriverBasedTestBase
{
    public WebDriverExtensionsTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output) {}
    [Fact]
    public async Task ShouldThrowException_WhenSolutionTypeAndCaptchaTypeDoNotMatch()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => Driver.SolveCaptchaAsync<GeeTestV3Solution>(AnticaptchaClient, new SolverArguments(CaptchaType: CaptchaType.ReCaptchaV2)));
    }


}