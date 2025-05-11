using System.ComponentModel;
using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxyless
{
    [Category(TestCategories.Proxyless)]
    public class RecaptchaV2EnterpriseProxylessSolverTests : SolverTestBase<RecaptchaSolution>
    {
        protected override string TestedUri { get; set; }  = TestUris.Recaptcha.V2.Enterprise.Steam;
        protected override CaptchaType CaptchaType { get; set; } = CaptchaType.ReCaptchaV2EnterpriseProxyless;
        
        public RecaptchaV2EnterpriseProxylessSolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
    }
}

