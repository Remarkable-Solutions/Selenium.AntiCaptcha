﻿using AntiCaptchaApi.Net.Models;
using OpenQA.Selenium;
using Selenium.CaptchaIdentifier.Constants;
using Selenium.CaptchaIdentifier.Enums;
using Selenium.CaptchaIdentifier.Extensions;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.CaptchaIdentifier.CaptchaIdentifiers;

public class FunCaptchaIdentifier : ProxyCaptchaIdentifier
{
    public FunCaptchaIdentifier()
    {
        IdentifiableTypes.AddRange(CaptchaTypeGroups.FunCaptchaTypes);
    }
    
    public override async Task<CaptchaType?> IdentifyInCurrentFrameAsync(IWebDriver driver, IWebElement? imageElement, ProxyConfig? proxyConfig,
        CancellationToken cancellationToken)
    {
        if(IsFunCaptcha(driver))
            return await base.SpecifyCaptcha(CaptchaType.FunCaptchaProxyless, driver, imageElement, proxyConfig, cancellationToken);
        return null;
    }

    private bool IsFunCaptcha(IWebDriver driver)
        => IsThereFunCaptchaFunCaptchaScriptInAnyIFrames(driver) || IsThereAnElementWithPkey(driver);


    private bool IsThereAnElementWithPkey(IWebDriver driver)
    {
        driver.SwitchTo().DefaultContent();
        return !string.IsNullOrEmpty(driver.FindFunCaptchaSiteKey());
    }

    private static bool IsThereFunCaptchaFunCaptchaScriptInAnyIFrames(IWebDriver driver)
    {
        driver.SwitchTo().DefaultContent();
        return driver.FindByXPathAllFrames("//script[contains(@src, 'funcaptcha'") != null;
    }
}