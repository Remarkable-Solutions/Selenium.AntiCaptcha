using AntiCaptchaApi.Net.Enums;
using AntiCaptchaApi.Net.Models;

namespace Selenium.Anticaptcha.Tests.Config;

public static class TestEnvironment
{
    public static string ClientKey = Environment.GetEnvironmentVariable("ClientKey");
    public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
    public static string ProxyAddress => Environment.GetEnvironmentVariable("ProxyAddress");
    public static string ProxyPort => Environment.GetEnvironmentVariable("ProxyPort");
    public static string ProxyLogin => Environment.GetEnvironmentVariable("ProxyLogin");
    public static string ProxyPassword => Environment.GetEnvironmentVariable("ProxyPassword");

    public static bool IsProxyDefined => 
        !string.IsNullOrEmpty(ProxyAddress) &&
        !string.IsNullOrEmpty(ProxyPort) &&
        !string.IsNullOrEmpty(ProxyLogin) &&
        !string.IsNullOrEmpty(ProxyPassword);
    
    public const string DriverBasedTestCollection = "Driver collection";
        
    public static ProxyConfig GetCurrentTestProxyConfig()
    {
        var proxyPortDefined = int.TryParse(ProxyPort, out var proxyPort2);
        return new ProxyConfig()
        {
            ProxyType = ProxyTypeOption.Http,
            ProxyAddress = ProxyAddress,
            ProxyPort = proxyPortDefined ? proxyPort2 : null,
            ProxyLogin = ProxyLogin,
            ProxyPassword = ProxyPassword
        };
    }
}