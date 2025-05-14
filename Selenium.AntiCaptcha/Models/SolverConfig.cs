using AntiCaptchaApi.Net.Models;
namespace Selenium.AntiCaptcha.Models;

public class SolverConfig : ClientConfig
{
    public SolverConfig(int pageLoadTimeoutMs, int delayTimeBetweenElementValueRetrievalMs, int maxWaitForTaskResultTimeMs, int maxHttpRequestTimeMs, int solveAsyncRetries,
        int delayTimeBetweenCheckingTaskResultMs)
    {
        MaxWaitForTaskResultTimeMs = maxWaitForTaskResultTimeMs;
        MaxHttpRequestTimeMs = maxHttpRequestTimeMs;
        SolveAsyncRetries = solveAsyncRetries;
        DelayTimeBetweenCheckingTaskResultMs = delayTimeBetweenCheckingTaskResultMs;
        DelayTimeBetweenElementValueRetrievalMs = delayTimeBetweenElementValueRetrievalMs;
        PageLoadTimeoutMs = pageLoadTimeoutMs;
    }
    public int PageLoadTimeoutMs { get; set; }
    public int DelayTimeBetweenElementValueRetrievalMs { get; set; }
    public int SolvingTimeoutMs { get; set; } = 1800000;
}