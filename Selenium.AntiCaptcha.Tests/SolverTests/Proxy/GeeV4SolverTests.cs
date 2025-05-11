using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxy;

public class GeeV4SolverTests : GeeV4SolverTestBase
{
    protected override CaptchaType CaptchaType { get; set; } = CaptchaType.GeeTestV4;

    public GeeV4SolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }
}