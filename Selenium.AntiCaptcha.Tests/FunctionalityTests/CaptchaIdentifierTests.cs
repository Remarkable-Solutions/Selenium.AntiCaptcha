using AntiCaptchaApi.Net.Models;
using AntiCaptchaApi.Net.Models.Solutions;
using OpenQA.Selenium;
using Selenium.AntiCaptcha;
using Selenium.AntiCaptcha.Enums;
using Selenium.AntiCaptcha.Extensions;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.Models;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.FunctionalityTests;


public class CaptchaIdentifierTests : WebDriverBasedTestBase
{
    private async Task TestProxyCaptchaIdentification(CaptchaUri captchaUri)
    {                
        var proxyType = captchaUri.ExpectedType.ToProxyType();
        if (proxyType.IsProxyType())
        {
            await TestNonGenericIdentifier(captchaUri with { ExpectedType = proxyType }, TestEnvironment.GetCurrentTestProxyConfig());
        }
    }

    private async Task TestProxylessCaptchaIdentification(CaptchaUri captchaUri)
    {
        if (captchaUri.ExpectedType.IsProxylessType())
        {
            await TestNonGenericIdentifier(captchaUri);
        }
    }
    
    public class IdentificationWithoutSolutionTypeSpecified : CaptchaIdentifierTests
    {
        public IdentificationWithoutSolutionTypeSpecified(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            
        }

        public class Recaptcha : IdentificationWithoutSolutionTypeSpecified
        {
            public Recaptcha(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
            {
            }
        
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V3.Enterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V3.Enterprise))]
            public async Task TestRecaptchaV3EnterpriseUrisProxyless(CaptchaUri captchaUri)
            {
                await TestProxylessCaptchaIdentification(captchaUri);
            }
         
         
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V3.NonEnterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V3.NonEnterprise))]
            public async Task TestRecaptchaV3NonEnterpriseUrisProxyless(CaptchaUri captchaUri)
            {
                await TestProxylessCaptchaIdentification(captchaUri);
            }

         
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V2.NonEnterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V2.NonEnterprise))]
            public async Task TestRecaptchaV2NonEnterpriseUrisProxyless(CaptchaUri captchaUri)
            {
                await TestProxylessCaptchaIdentification(captchaUri);
            }
         
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V2.Enterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V2.Enterprise))]
            public async Task TestRecaptchaV2EnterpriseUrisProxyless(CaptchaUri captchaUri)
            {
                await TestProxylessCaptchaIdentification(captchaUri);
            }
        
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V3.Enterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V3.Enterprise))]
            public async Task TestRecaptchaV3EnterpriseUrisProxy(CaptchaUri captchaUri)
            {
                await TestProxyCaptchaIdentification(captchaUri);
            }
         
         
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V3.NonEnterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V3.NonEnterprise))]
            public async Task TestRecaptchaV3NonEnterpriseUrisProxy(CaptchaUri captchaUri)
            {
                await TestProxyCaptchaIdentification(captchaUri);
            }

         
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V2.NonEnterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V2.NonEnterprise))]
            public async Task TestRecaptchaV2NonEnterpriseUrisProxy(CaptchaUri captchaUri)
            {
                await TestProxyCaptchaIdentification(captchaUri);
            }
         
            [Theory]
            [MemberData(nameof(TestUris.Recaptcha.V2.Enterprise.Uris), MemberType = typeof(TestUris.Recaptcha.V2.Enterprise))]
            public async Task TestRecaptchaV2EnterpriseUrisProxy(CaptchaUri captchaUri)
            {
                await TestProxyCaptchaIdentification(captchaUri);
            }
        }

        
        public class OtherCaptcha : IdentificationWithoutSolutionTypeSpecified
        {
            public OtherCaptcha(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
            {
            }
            [Theory]
            [MemberData(nameof(TestUris.ImageToText.Uris), MemberType = typeof(TestUris.ImageToText))]
            public async Task TestProxylessImageToTextCaptchaUris(CaptchaUri captchaUri)
            {
                if (captchaUri.ExpectedType.IsProxylessType())
                {
                    await TestNonGenericIdentifier(captchaUri);
                }
            }
            
            [Theory]
            [MemberData(nameof(TestUris.TestableUrisWithoutRecaptcha), MemberType = typeof(TestUris))]
            public async Task TestProxylessCaptchaUris(CaptchaUri captchaUri)
            {
                if (captchaUri.ExpectedType.IsProxylessType())
                {
                    await TestNonGenericIdentifier(captchaUri);
                }
            }
         
            [Fact]
            public async Task TestSingleCaptchaUri()
            {
                var captchaUri = new CaptchaUri(TestUris.GeeTest.V3.Zhiyou, CaptchaType.GeeTestV3Proxyless);
                await TestNonGenericIdentifier(captchaUri);
            }
            
            [Theory]
            [MemberData(nameof(TestUris.TestableUrisWithoutRecaptcha), MemberType = typeof(TestUris))]
            public async Task TestProxyCaptchaUris(CaptchaUri captchaUri)
            {   
                var proxyType = captchaUri.ExpectedType.ToProxyType();
                if (!proxyType.IsProxylessType())
                {
                    await TestNonGenericIdentifier(captchaUri with { ExpectedType = proxyType }, TestEnvironment.GetCurrentTestProxyConfig());
                }
            }
        }
    }
    
    
    public class IdentificationWithSolutionTypeSpecified : CaptchaIdentifierTests
    {
        [Theory]
        [InlineData(TestUris.GeeTest.V3.GeeTestV3Demo, CaptchaType.GeeTestV3Proxyless)]
        public async Task ShouldReturnProperGeeV3TestType_WithoutProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<GeeTestV3Solution>(websiteUrl, expectedType, null);
        }
        
        
        [Theory]
        [InlineData(TestUris.GeeTest.V3.GeeTestV3Demo, CaptchaType.GeeTestV3)]
        public async Task ShouldReturnProperGeeV3TestType_WithProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<GeeTestV3Solution>(websiteUrl, expectedType, TestEnvironment.GetCurrentTestProxyConfig());
        }
        
        
        [Theory]
        [InlineData(TestUris.GeeTest.V4.GeeTestV4Demo, CaptchaType.GeeTestV4Proxyless)]
        public async Task ShouldReturnProperGeeV4TestType_WithoutProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<GeeTestV4Solution>(websiteUrl, expectedType, null);
        }
        
        
        [Theory]
        [InlineData(TestUris.GeeTest.V4.GeeTestV4Demo, CaptchaType.GeeTestV4)]
        public async Task ShouldReturnProperGeeV4TestType_WithProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<GeeTestV4Solution>(websiteUrl, expectedType, TestEnvironment.GetCurrentTestProxyConfig());
        }
        
        [Theory]
        [InlineData(TestUris.Recaptcha.V3.Enterprise.Netflix, CaptchaType.ReCaptchaV3Enterprise)]
        [InlineData(TestUris.Recaptcha.V3.NonEnterprise.RecaptchaV3Demo, CaptchaType.ReCaptchaV3)]
        [InlineData(TestUris.Recaptcha.V2.NonEnterprise.RecaptchaV2DemoCheckbox, CaptchaType.ReCaptchaV2Proxyless)]
        public async Task ShouldReturnProperRecaptchaType_WithoutProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<RecaptchaSolution>(websiteUrl, expectedType, null);
        }
        
        [Theory]
        [InlineData(TestUris.Recaptcha.V3.Enterprise.Netflix, CaptchaType.ReCaptchaV3Enterprise)]
        [InlineData(TestUris.Recaptcha.V2.NonEnterprise.RecaptchaV2DemoCheckbox, CaptchaType.ReCaptchaV2)]
        public async Task ShouldReturnProperRecaptchaType_WithProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<RecaptchaSolution>(websiteUrl, expectedType, TestEnvironment.GetCurrentTestProxyConfig());
        }

        [Theory]
        [InlineData(TestUris.FunCaptcha.FunCaptchaDemo, CaptchaType.FunCaptchaProxyless)]
        public async Task ShouldReturnProperFunCaptchaType_WithoutProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<FunCaptchaSolution>(websiteUrl, expectedType, null);
        }
        
        [Theory]
        [InlineData(TestUris.FunCaptcha.FunCaptchaDemo, CaptchaType.FunCaptcha)]
        public async Task ShouldReturnProperFunCaptchaType_WithProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<FunCaptchaSolution>(websiteUrl, expectedType, TestEnvironment.GetCurrentTestProxyConfig());
        }
        
        
        [Theory]
        [InlineData(TestUris.Turnstile.TurnStileDemo, CaptchaType.TurnstileProxyless)]
        public async Task ShouldReturnProperTurnstileType_WithoutProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<TurnstileSolution>(websiteUrl, expectedType, null);
        }

        [Theory]
        [InlineData(TestUris.Turnstile.TurnStileDemo, CaptchaType.Turnstile)]
        public async Task ShouldReturnProperTurnstileType_WithProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<TurnstileSolution>(websiteUrl, expectedType, TestEnvironment.GetCurrentTestProxyConfig());
        }

        [Theory]
        [InlineData(TestUris.ImageToText.Wikipedia, CaptchaType.ImageToText)]
        public async Task ShouldReturnProperImageToTextType_WithoutProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<ImageToTextSolution>(websiteUrl, expectedType, null);
        }
        
        [Theory]
        [InlineData(TestUris.AntiGate.AntiCaptchaTuttorialAntiGate, CaptchaType.AntiGate)]
        public async Task ShouldReturnProperAntiGateType_WithoutProxy(string websiteUrl, CaptchaType expectedType)
        {
            await TestIdentifier<AntiGateSolution>(websiteUrl, expectedType, null);
        }

        public IdentificationWithSolutionTypeSpecified(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            
        }
    }
    
    [Fact]
    public void ShouldBeAbleToIdentifyAllCaptchaTypes()
    {
        foreach (var enumValue in Enum.GetValues(typeof(CaptchaType)))
        {
            var canIdentify = CaptchaIdentifier.CanIdentifyCaptcha((CaptchaType)enumValue);
            if (canIdentify)
                continue;
            Assert.Null(enumValue);
            Assert.True(canIdentify);
        }
    }
    
    protected async Task TestNonGenericIdentifier(CaptchaUri captchaUri, ProxyConfig? proxyConfig = null, IWebElement? imageElement = null)
    {
        await SetDriverUrl(captchaUri.Uri);
        var identifiedTypes = await Driver.IdentifyCaptchaAsync(imageElement, proxyConfig, CancellationToken.None);
        var testFailed = identifiedTypes.Count != 1 || identifiedTypes[0] != captchaUri.ExpectedType;

        if (testFailed)
        {
            Assert.Fail(GetTestFailReasonText(captchaUri, proxyConfig, string.Join(", ", identifiedTypes.Select(x => x.ToString()))));   
        }
    }

    private static string GetTestFailReasonText(CaptchaUri captchaUri, ProxyConfig? proxyConfig, string foundTypesNames)
    {
        return $"Test non generic identifier failed for url: {captchaUri.Uri}. \n" +
               $"Expected type {captchaUri.ExpectedType}, but " + (string.IsNullOrEmpty(foundTypesNames) ? "found no matching types" : $"found {foundTypesNames}") + "\n" +
               $"ProxyConfig was {proxyConfig}";
    }
    
    protected async Task TestIdentifier<TSolution>(string websiteUri, CaptchaType expectedType, ProxyConfig? proxyConfig = null, IWebElement? imageElement = null)
        where TSolution : BaseSolution, new()
    {
        var captchaUri = new CaptchaUri(websiteUri, expectedType);
        await TestIdentifier<TSolution>(captchaUri, proxyConfig, imageElement);
    }

    protected async Task TestIdentifier<TSolution>(CaptchaUri captchaUri, ProxyConfig? proxyConfig = null, IWebElement? imageElement = null)
        where TSolution : BaseSolution, new()
    {
        await SetDriverUrl(captchaUri.Uri);
        var type = await Driver.IdentifyCaptchaAsync<TSolution>(imageElement, proxyConfig, CancellationToken.None); 
        var testFailed = type == null || type != captchaUri.ExpectedType;
        if (testFailed)
        {
            Assert.Fail(GetTestFailReasonText(captchaUri, proxyConfig, type.ToString() ?? "null"));   
        }
    }
    public CaptchaIdentifierTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output) {}
}