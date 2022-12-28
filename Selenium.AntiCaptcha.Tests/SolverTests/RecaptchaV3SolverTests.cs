using System.ComponentModel;
using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.Config;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;

namespace Selenium.Anticaptcha.Tests.SolverTests
{
    [Category(TestCategories.Proxyless)]
    public class RecaptchaV3SolverTests : SolverTestBase<RecaptchaSolution>
    {
        protected override string TestedUri { get; set; }  = TestUris.Recaptcha.V3.NonEnterprise.RecaptchaV3Demo;
        protected override CaptchaType CaptchaType { get; set; } = CaptchaType.ReCaptchaV3;
        
        public RecaptchaV3SolverTests(WebDriverFixture fixture) : base(fixture)
        {
        }
    }
}

