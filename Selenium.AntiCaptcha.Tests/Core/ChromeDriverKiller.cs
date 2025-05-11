using System.Management.Automation;

namespace Selenium.Anticaptcha.Tests.Core;

public static class ChromeDriverKiller
{
    private static readonly Destructor Finalise = new();
    public static bool KilledChromeDrivers { get; private set; }
    public static void KillAllPreviousChromeDriversIfTheyExist()
    {
        if (!KilledChromeDrivers)
        {
            KillAllChromeDrivers();
        }
        KilledChromeDrivers = true;
    }

    private static void KillAllChromeDrivers()
    {
        try
        {
            var ps = PowerShell.Create();
            ps.AddCommand("Stop-Process").AddParameter("Name", "chromedriver");
            ps.Invoke();

        }
        catch (Exception)
        {
            // ignore
        }
    }
    
    private sealed class Destructor
    {
        ~Destructor()
        {
            KillAllChromeDrivers();
        }
    }
}