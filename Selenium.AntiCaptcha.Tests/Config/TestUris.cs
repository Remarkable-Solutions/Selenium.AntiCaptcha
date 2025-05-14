using System.Diagnostics.CodeAnalysis;
using Selenium.AntiCaptcha.Enums;
using Selenium.Anticaptcha.Tests.Core.Models;

namespace Selenium.Anticaptcha.Tests.Config;


[SuppressMessage("ReSharper", "IdentifierTypo")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
public static class TestUris
{
    public static IEnumerable<object[]> Uris() => 
        Recaptcha.Uris()
            .Concat(FunCaptcha.Uris())
            .Concat(ImageToText.Uris())
            .Concat(GeeTest.Uris())
            .Concat(Turnstile.Uris());

    public static IEnumerable<object[]> TestableUris() =>        
        Recaptcha.Uris()
            .Concat(FunCaptcha.Uris())
            .Concat(GeeTest.Uris())
            .Concat(ImageToText.Uris())
            .Concat(Turnstile.Uris());

    public static IEnumerable<object[]> TestableUrisWithoutRecaptcha() =>        
        FunCaptcha.Uris()
            .Concat(GeeTest.Uris())
            .Concat(ImageToText.Uris())
            .Concat(Turnstile.Uris());

    public class Recaptcha
    {
        public static IEnumerable<object[]> Uris() => V3.Uris().Concat(V2.Uris());
        public static class V3
        {
            public static IEnumerable<object[]> Uris() => Enterprise.Uris().Concat(NonEnterprise.Uris());

            public static class Enterprise
            {
                public const CaptchaType Type = CaptchaType.ReCaptchaV3Enterprise;
                public static IEnumerable<object[]> Uris()
                {
                    yield return new object[] { new CaptchaUri(Netflix, Type) };
                    yield return new object[] { new CaptchaUri(SquareUp, Type) };
                }
                public const string Netflix = "https://www.netflix.com/pl-en/login";
                public const string SquareUp = "https://squareup.com/signup";
            }

            public static class NonEnterprise
            {
                public const CaptchaType Type = CaptchaType.ReCaptchaV3;
                public static IEnumerable<object[]> Uris()
                {
                    yield return new object[] { new CaptchaUri(RecaptchaV3Demo, Type) };
                    yield return new object[] { new CaptchaUri(Twillo, Type) };
                }
                public const string RecaptchaV3Demo = "https://recaptcha-demo.appspot.com/recaptcha-v3-request-scores.php";
                public const string Twillo = "https://www.twilio.com/go/twilio-brand-sales-1";
            }
        }

        public static class V2
        {
            public static IEnumerable<object[]> Uris() => Enterprise.Uris().Concat(NonEnterprise.Uris());

            public static class Enterprise
            {
                public const CaptchaType Type = CaptchaType.ReCaptchaV2EnterpriseProxyless;
                public static IEnumerable<object[]> Uris()
                {
                    yield return new object[] { new CaptchaUri(Steam, Type) };
                }
                public const string Steam = "https://store.steampowered.com/join";
            }

            public static class NonEnterprise
            {
                public const CaptchaType Type = CaptchaType.ReCaptchaV2Proxyless;

                public static IEnumerable<object[]> Uris()
                {
                    yield return new object[] { new CaptchaUri(RecaptchaV2DemoCheckbox, Type) };
                    yield return new object[] { new CaptchaUri(AntigateDemo, Type) };
                    yield return new object[] { new CaptchaUri(Rescan, Type) };
                    yield return new object[] { new CaptchaUri(RecaptchaV2DemoInvisible, Type) };
                }
                public const string RecaptchaV2DemoCheckbox = "https://recaptcha-demo.appspot.com/recaptcha-v2-checkbox.php";
                public const string AntigateDemo = "http://antigate.com/logintest.php";
                public const string Rescan = "https://rescan.io/join/";
                public const string RecaptchaV2DemoInvisible = "https://recaptcha-demo.appspot.com/recaptcha-v2-invisible.php";
            }
        }
    }
    public static class FunCaptcha
    {
        public const CaptchaType Type = CaptchaType.FunCaptchaProxyless;
        public static IEnumerable<object[]> Uris()
        {
            yield return new object[] { new CaptchaUri(FunCaptchaDemo, Type) };
            yield return new object[] { new CaptchaUri(Roblox, Type) };
            yield return new object[] { new CaptchaUri(Github, Type) };
        }
        public const string FunCaptchaDemo = "https://api.funcaptcha.com/fc/api/nojs/?pkey=69A21A01-CC7B-B9C6-0F9A-E7FA06677FFC";
        public const string Roblox = "https://www.roblox.com/";
        public const string Github = "https://github.com/signup?ref_cta=Sign+up&ref_loc=header+logged+out&ref_page=%2F&source=header-home";
    }

    public static class Turnstile
    {
        public const CaptchaType Type = CaptchaType.TurnstileProxyless;
        public static IEnumerable<object[]> Uris()
        {
            yield return new object[] { new CaptchaUri(TurnStileDemo, Type) };
        }
        public const string TurnStileDemo = "https://react-turnstile.vercel.app/";
    }
    public static class GeeTest
    {
        public static IEnumerable<object[]> Uris() => V4.Uris().Concat(V3.Uris());
        public static class V4
        {
            public const CaptchaType Type = CaptchaType.GeeTestV4Proxyless;
            public static IEnumerable<object[]> Uris()
            {
                yield return new object[] { new CaptchaUri(GeeTestV4Demo, Type) };
            }
            public const string GeeTestV4Demo = "https://www.geetest.com/en/adaptive-captcha-demo";
        }

        public static class V3
        {
            public const CaptchaType Type = CaptchaType.GeeTestV3Proxyless;
            public static IEnumerable<object[]> Uris()
            {
                yield return new object[] { new CaptchaUri(GeeTestV3Demo, Type) };
                //yield return new object[] { new CaptchaUri(Seloger, Type) };
                yield return new object[] { new CaptchaUri(Zhiyou, Type) };
            }
            public const string GeeTestV3Demo = "https://www.geetest.com/en/demo";
            //public const string Seloger = "https://www.seloger.com/";
            public const string Zhiyou = "https://zhiyou.smzdm.com/user/login?redirect_to=http%3A%2F%2Fzhiyou.smzdm.com%2Fuser";
        }
    }

    public static class ImageToText
    {
        public const CaptchaType Type = CaptchaType.ImageToText;
        public static IEnumerable<object[]> Uris()
        {
            yield return new object[] { new CaptchaUri(Wikipedia, Type) };
            yield return new object[] { new CaptchaUri(DemoCaptcha, Type) };
        }
        public const string Wikipedia = "https://en.wikipedia.org/w/index.php?title=Special:CreateAccount&returnto=Main+Page";
        public const string DemoCaptcha = "https://democaptcha.com/demo-form-eng/image.html";
    }
}