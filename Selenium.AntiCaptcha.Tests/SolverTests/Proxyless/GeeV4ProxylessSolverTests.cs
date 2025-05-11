using System.ComponentModel;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxyless;


[Category(TestCategories.Proxyless)]
public class GeeV4ProxylessSolverTests : GeeV4SolverTestBase
{
    protected override CaptchaType CaptchaType { get; set; } = CaptchaType.GeeTestV4Proxyless;

    public GeeV4ProxylessSolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }
}