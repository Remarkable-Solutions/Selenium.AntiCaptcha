using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxy;

public class GeeV3SolverTests : SolverTestBase <GeeTestV3Solution>
{
    protected override string TestedUri { get; set; } = TestUris.GeeTest.V3.GeeTestV3Demo;
    protected override CaptchaType CaptchaType { get; set; } = CaptchaType.GeeTestV3;


    public GeeV3SolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }
}