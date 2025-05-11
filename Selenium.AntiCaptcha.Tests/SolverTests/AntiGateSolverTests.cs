using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests
{
    public class AntiGateSolverTests : SolverTestBase  <AntiGateSolution>
    {
        protected override string TestedUri { get; set; } = TestUris.AntiGate.AntiCaptchaTuttorialAntiGate;
        protected override CaptchaType CaptchaType { get; set; }  = CaptchaType.AntiGate;

        public AntiGateSolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            SolverArgumentsWithoutCaptchaType.TemplateName = "CloudFlare cookies for a proxy";
            SolverArgumentsWithCaptchaType.TemplateName = "CloudFlare cookies for a proxy";
        }
    }
}

