using System.ComponentModel;
using AntiCaptchaApi.Net.Models.Solutions;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.Anticaptcha.Tests.Core.SolverTestBases;
using Selenium.FramesSearcher.Extensions;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.SolverTests.Proxyless;

[Category(TestCategories.Proxyless)]
public class ImageToTextSolverTests : SolverTestBase<ImageToTextSolution>
{
    protected override string TestedUri { get; set; } = TestUris.ImageToText.Wikipedia;
    protected override CaptchaType CaptchaType { get; set; } = CaptchaType.ImageToText;

    public ImageToTextSolverTests(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        
    }

    protected override async Task BeforeTestAction()
    {
        var imageElement = Driver.FindByXPathInCurrentFrame("//img[contains(@class, 'captcha')]");
        SolverArgumentsWithCaptchaType.ImageElement = imageElement;
        SolverArgumentsWithoutCaptchaType.ImageElement = imageElement;
    }
}