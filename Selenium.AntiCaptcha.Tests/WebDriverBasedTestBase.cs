using System.Diagnostics;
using OpenQA.Selenium;
using Selenium.Anticaptcha.Tests.Config;
using Selenium.Anticaptcha.Tests.Core;
using Selenium.FramesSearcher.Extensions;
using Xunit.Abstractions;

namespace Selenium.Anticaptcha.Tests;
public abstract class WebDriverBasedTestBase : IClassFixture<WebDriverFixture>, IDisposable
{
    private readonly WebDriverFixture _fixture;
    protected readonly ITestOutputHelper _output;
    protected IWebDriver Driver => _fixture.Driver;
    private const int MaxWaitingTimeInMilliseconds = 5000;
    private const int StepDelayTimeInMilliseconds = 500;
    private const string ResetWebsiteUri = "https://www.google.com/";
    
    protected WebDriverBasedTestBase(WebDriverFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }
    
    protected readonly string ClientKey = TestEnvironment.ClientKey;
    
    public virtual void Dispose()
    {
        //_fixture?.Dispose();
    }
    
    private async Task WaitForLoad()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        while (stopWatch.ElapsedMilliseconds <= MaxWaitingTimeInMilliseconds)
        {
            if(IsLoaded())
                break;
            
            await Task.Delay(StepDelayTimeInMilliseconds);
        }
        
    }

    private bool IsLoaded()
    {
        var isResetPage = Driver.Url == ResetWebsiteUri;
        return isResetPage ?
                   Driver.FindByXPathAllFrames("//img[contains(@alt, 'oogle')]") != null : //TODO: better xpaths.
                   Driver.FindByXPathAllFrames("//*[contains(@class, 'captcha')]") != null;
    }

    protected async Task ResetDriverUri()
    {
        await SetDriverUrl(ResetWebsiteUri);
    }

    protected async Task SetDriverUrl(string url)
    {
        if (url == Driver.Url)
        {
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Navigate().Refresh();
        }
        else
        {
            Driver.Navigate().GoToUrl(url);   
        }
        await WaitForLoad();
    }
}