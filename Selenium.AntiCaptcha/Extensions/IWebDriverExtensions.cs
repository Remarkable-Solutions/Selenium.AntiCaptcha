using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Selenium.FramesSearcher.Extensions;

namespace Selenium.AntiCaptcha.Extensions;

public static class IWebDriverExtensions
{
    private const string FunCaptchaRegexSiteKeyPattern = @"(?:funcaptcha|arkoselabs){1}.{0,200}(\w{8}-\w{4}-\w{4}-\w{4}-\w{12})";
    public static string FindFunCaptchaSiteKey(this IWebDriver driver)
    {       
        try
        {
            return driver.FindElement(By.Id("funcaptcha")).GetAttribute("data-pkey");
        }
        catch (Exception)
        {
            // ignore
        }

        var pageSource = driver.GetAllPageSource();
        var match = pageSource.GetFirstRegexThatFits(true, FunCaptchaRegexSiteKeyPattern);

        if (match == null || match.Groups.Count == 0)
            return string.Empty;

        return match.Groups[1].Value;
    }

    public static string FindSingleImageSourceForImageToText(this IWebDriver driver)
    {
        var pageSource = driver.GetAllPageSource();

        var doesContainIFrames = pageSource.DoesContainRegex(@"<\s*iframe.*<\/iframe>");

        if (doesContainIFrames)
            return string.Empty;
        
        
        var imageSources = pageSource.GetFirstRegexThatFits(RegexOptions.Multiline | RegexOptions.IgnoreCase, "^.*?<\\s*?img.*src\\s*=\\s*\"(.+?)\".*?$");

        if (!imageSources.Any(x => x.Success))
        {
            return string.Empty;
        }


        var possibleCaptchaImageSources = new List<string>();
            
        foreach (Match match in imageSources)
        {
            if(match.Groups.Count != 2 || !match.Groups[1].Success) 
                continue;
            var sourceValue = Regex.Unescape((System.Net.WebUtility.HtmlDecode(match.Groups[1].Value)));
            
            var idMatch = sourceValue.GetFirstRegexThatFits(true, @".*?=(\d{1,20})\D*?");

            if (sourceValue.ToLower().Contains("captcha") && idMatch is not null && idMatch.Success)
            {
                possibleCaptchaImageSources.Add(sourceValue);
            }

        }
        
        return possibleCaptchaImageSources.Count == 1 ? possibleCaptchaImageSources[0] : string.Empty;
    }
}