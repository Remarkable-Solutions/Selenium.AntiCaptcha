using AntiCaptchaApi.Net;
using AntiCaptchaApi.Net.Models.Solutions;
using AntiCaptchaApi.Net.Requests;
using Newtonsoft.Json;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Models;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxy
{
    public class FunCaptchaSolverTests : SolverTestBase <FunCaptchaSolution>
    {
        protected override string TestedUri { get; set; } = TestUris.FunCaptcha.FunCaptchaDemo;
        protected override CaptchaType CaptchaType { get; set; }  = CaptchaType.FunCaptcha;

        public FunCaptchaSolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
        
        
        
        [Fact]
        public virtual async Task Solve_Roblox()
        {
            var request = new FunCaptchaRequest
            {
                WebsiteUrl = "https://www.roblox.com",
                WebsitePublicKey = "A2A14B1D-1AF3-C791-9BBC-EE33CC7A0A6F",
                FunCaptchaApiJsSubdomain = "roblox-api.arkoselabs.com",
                Data = JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    { "blob", "F0b2cuullUe0U8ID.oN6ksrLpRZeEYYYmnL6r4Yuwhd0KNduKLyL4t3zfK8HeY95iV78WG%2FqN0Oac6ZLyvfQFntuot9tut8Q7phoYNj6PzJOUIaw%2FQEBEq2bIlDRinpFhcX4VCnZMwB6lBo0V%2BZECFKnvZD448bLiGKKvTntU7c0ikEjZ5yN691659Vva2j%2FMxh%2B7a234KYb6KP7m0jPK5Zkj3HUYzjLZT6HmzYQs452YAaA4f9L71HR06zxfFxzdEdJHyjhDINxl8rn5%2FiyI7wJIG7i2eRhW7rUZfW7nvreha1h7bqplSsG8W%2FKsXhobZRX0y6Yj5gsZErqwRfXPHLKmUxPUYeHTrypU%2BjdFd32fBqhvgdA9S93Mh0FOdN9M%2BtxZI90bgyl5bBLdvWYG2jzqgNUBgCQzGnTnyFdgemS%2BcyAoKz3ZfHKNHnmlrBldilUC8PgPTX7KowyvTnQ2Sr39yNW%2BPwvuNKJUbNp2%2FOd6LV72J2vOpJGap1s%3D" },
                }),
                UserAgent = TestEnvironment.UserAgent,
                ProxyConfig = TestEnvironment.GetCurrentTestProxyConfig()
            };

            //
            // var antiCaptchaClient = new AnticaptchaClient(ClientKey, new DefaultSolverConfig(maxHttpRequestTimeMs: 300));

            var result = await AnticaptchaClient.SolveCaptchaAsync<FunCaptchaSolution>(request);
            AssertSolveCaptchaResult(result, CaptchaType.FunCaptcha);
        }

    }
}

