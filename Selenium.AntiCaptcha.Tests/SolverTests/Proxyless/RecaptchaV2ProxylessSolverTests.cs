using System.ComponentModel;
using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.Config;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxyless
{
    [Category(TestCategories.Proxyless)]
    public class RecaptchaV2ProxylessSolverTests : SolverTestBase<RecaptchaSolution>
    {
        protected override string TestedUri { get; set; }  = TestUris.Recaptcha.V2.NonEnterprise.AntigateDemo;
        protected override CaptchaType CaptchaType { get; set; } = CaptchaType.ReCaptchaV2Proxyless;

        public RecaptchaV2ProxylessSolverTests(WebDriverFixture fixture) : base(fixture)
        {
        }
    }
}

