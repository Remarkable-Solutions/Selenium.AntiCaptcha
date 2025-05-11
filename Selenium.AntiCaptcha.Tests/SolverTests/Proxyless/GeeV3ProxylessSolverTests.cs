using System.ComponentModel;
using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxyless;

[Category(TestCategories.Proxyless)]
public class GeeV3ProxylessSolverTests : SolverTestBase<GeeTestV3Solution>
{
    protected override string TestedUri { get; set; } = TestUris.GeeTest.V3.GeeTestV3Demo;
    protected override CaptchaType CaptchaType { get; set; } = CaptchaType.GeeTestV3Proxyless;
    

    public GeeV3ProxylessSolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        SolverArgumentsWithCaptchaType.Gt = "b6e21f90a91a3c2d4a31fe84e10d0442";
        SolverArgumentsWithCaptchaType.Challenge = "40cb68a93238c5b0f188ea88adfb07df";
    }
}