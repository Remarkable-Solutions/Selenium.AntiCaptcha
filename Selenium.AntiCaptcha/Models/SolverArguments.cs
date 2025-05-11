using AntiCaptchaApi.Net.Enums;
using AntiCaptchaApi.Net.Models;
using AntiCaptchaApi.Net.Requests.Abstractions.Interfaces;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Selenium.AntiCaptcha.Enums;

namespace Selenium.AntiCaptcha.Models;

public record SolverArguments(
    CaptchaType? CaptchaType = null,
    string? WebsiteUrl = null,
    string? WebsiteKey = null,
    string? WebsitePublicKey = null,
    string? BodyBase64 = null,
    string? FilePath = null,
    string? Cookies = null,
    string? RecaptchaDataSValue = null,
    IWebElement? ImageElement = null,
    string? UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36",
    ProxyConfig? ProxyConfig = null,
    string? FunCaptchaApiJsSubdomain = null,
    string? Data = null,
    JObject? Variables = null,
    string? TemplateName = null,
    List<string>? DomainsOfInterest = null,
    string? GeetestApiServerSubdomain = null,
    string? GeetestGetLib = null,
    string? Challenge = null,
    string? Gt = null,
    Dictionary<string, string>? InitParameters = null,
    decimal MinScore = 0.3m,
    string? PageAction = null,
    string? ApiDomain = null,
    bool? IsEnterprise = null,
    bool? Phrase = null,
    bool? Case = null,
    NumericOption? Numeric = null,
    bool? Math = null,
    int? MinLength = null,
    int? MaxLength = null,
    string? Comment = null,
    string? ImageFilePath = null,
    bool? IsInvisible = null,
    string? LanguagePool = null,
    string? CallbackUrl = null,
    Dictionary<string, string>? EnterprisePayload = null) 
      :
          IAntiGateRequest,
          IFunCaptchaRequest,
          IGeeTestV3Request,
          IGeeTestV4Request,
          IImageToTextRequest,
          IRecaptchaV2EnterpriseRequest,
          IRecaptchaV2Request,
          IRecaptchaV3EnterpriseRequest,
          ITurnstileCaptchaRequest
{
    public CaptchaType? CaptchaType { get; set; } = CaptchaType;
    public string? WebsiteUrl { get; set; } = WebsiteUrl;
    public string? WebsiteKey { get; set; } = WebsiteKey;
    public IWebElement? ImageElement { get; set; } = ImageElement;
    public string? UserAgent { get; set; } = UserAgent;
    public ProxyConfig? ProxyConfig { get; set; } = ProxyConfig;
    public string? WebsitePublicKey { get; set; } = WebsitePublicKey;
    public string? FunCaptchaApiJsSubdomain { get; set; } = FunCaptchaApiJsSubdomain;
    public string? Data { get; set; } = Data;
    public JObject? Variables { get; set; } = Variables;
    public string? TemplateName { get; set; } = TemplateName;
    public List<string>? DomainsOfInterest { get; set; } = DomainsOfInterest;
    public string? GeetestApiServerSubdomain { get; set; } = GeetestApiServerSubdomain;
    public string? GeetestGetLib { get; set; } = GeetestGetLib;
    public string? Challenge { get; set; } = Challenge;
    public string? Gt { get; set; } = Gt;
    public Dictionary<string, string>? InitParameters { get; set; } = InitParameters;
    public decimal MinScore { get; set; } = MinScore;
    public string? PageAction { get; set; } = PageAction;
    public string? ApiDomain { get; set; } = ApiDomain;
    public bool? IsEnterprise { get; set; } = IsEnterprise;
    public string? BodyBase64 { get; set; } = BodyBase64;
    public bool? Phrase { get; set; } = Phrase;
    public bool? Case { get; set; } = Case;
    public NumericOption? Numeric { get; set; } = Numeric;
    public bool? Math { get; set; } = Math;
    public int? MinLength { get; set; } = MinLength;
    public int? MaxLength { get; set; } = MaxLength;
    public string? Comment { get; set; } = Comment;
    public string? FilePath { get; set; } = FilePath;
    public string? ImageFilePath { get; set; } = ImageFilePath;
    public bool? IsInvisible { get; set; } = IsInvisible;
    public Dictionary<string, string>? EnterprisePayload { get; set; } = EnterprisePayload;
    public string? Cookies { get; set; } = Cookies;
    public string? RecaptchaDataSValue { get; set; } = RecaptchaDataSValue;
    public string? LanguagePool { get; set; } = LanguagePool;
    public string? CallbackUrl { get; set; } = CallbackUrl;
}