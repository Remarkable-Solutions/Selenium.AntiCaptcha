using AntiCaptchaApi.Net.Models.Solutions;
using OpenQA.Selenium;
using Selenium.Anticaptcha.Tests.Config;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests.Core.SolverTestBases;

public abstract class GeeV4SolverTestBase : SolverTestBase<GeeTestV4Solution>
{
    protected override string TestedUri { get; set; } = TestUris.GeeTest.V4.GeeTestV4Demo;
    
    protected GeeV4SolverTestBase(WebDriverFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        
    }
    
    protected override async Task BeforeTestAction()
    {
        var allButtonParents = Driver.FindElements(By.XPath("//button/parent::*"));
        var slideButton = FindSlideButton(allButtonParents);
        
        Assert.NotNull(slideButton);
        slideButton.Click();
        await Task.Delay(1000);
    }
    
    
    private static IWebElement FindSlideButton(IEnumerable<IWebElement> buttons)
    {
        try
        {
            foreach (var button in buttons)
            {
                var buttonText = button.Text;

                if (buttonText.Contains("Slide"))
                {
                    return button;
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return null;
    }


}