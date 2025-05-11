using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxy
{
    public class RecaptchaV2SolverTests : SolverTestBase<RecaptchaSolution>
    {
        protected override string TestedUri { get; set; }  = TestUris.Recaptcha.V2.NonEnterprise.AntigateDemo;
        protected override CaptchaType CaptchaType { get; set; }  = CaptchaType.ReCaptchaV2;
        
        public RecaptchaV2SolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
    }
}

