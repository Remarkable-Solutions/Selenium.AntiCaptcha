using Selenium.AntiCaptcha.Enums;

namespace Selenium.Anticaptcha.Tests.Core.Models;

public record CaptchaUri(string Uri, CaptchaType ExpectedType);