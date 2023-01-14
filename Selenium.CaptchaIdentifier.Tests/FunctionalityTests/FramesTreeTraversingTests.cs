﻿using OpenQA.Selenium;
using Selenium.FramesSearcher.Extensions;
using Tests.Common;
using Tests.Common.Config;
using Tests.Common.Core;

namespace Selenium.CaptchaIdentifier.Tests.FunctionalityTests;

    public class FramesTreeTraversingTests : WebDriverBasedTestBase
    {
        private const string TestUri = TestUris.Recaptcha.V2.NonEnterprise.AntigateDemo; 
        // TestUris.Recaptcha.V2.NonEnterprise.W2: Has structure
        //   1
        // 1 1 1
        // 1
        
        public FramesTreeTraversingTests(WebDriverFixture fixture) : base(fixture)
        {
            SetDriverUrl(TestUri).Wait();
        }

        public override void Dispose()
        {
            base.Dispose();
            ResetDriverUri().Wait();
        }

        
        //TODO. Find site with more iframes.
        [Fact] 
        public async Task ShouldBuildFramesTree_AndReturnToOriginalFrame_FromBottom()
        {
            var rootFrame = Driver.GetCurrentFrame();
            var bottomFrame = GetDeepestFrame(null, null, 0);
            var rootElement = Driver.GetCurrentRootWebElement();
            Driver.SwitchTo().Frame(Driver.FindIFramesInFrame(rootFrame).First().WebElement);
            var currentFrame = Driver.GetCurrentFrame();
            Driver.GetFullFramesTree();
            var afterOperationFrame = Driver.GetCurrentFrame();
            
            
            Assert.True(rootFrame.IsRoot);
            Assert.False(rootFrame.IsFrame);
            Assert.NotNull(rootElement);
            Assert.NotNull(currentFrame);
            Assert.NotNull(afterOperationFrame);
            Assert.Equal(currentFrame.WebElement, afterOperationFrame.WebElement);
        }

        private (IWebElement? frame, int level) GetDeepestFrame(ExtendedWebElement? rootFrame, IWebElement? frame, int level)
        {
            if (frame != null)
            {
                if (!Driver.TryToSwitchToFrame(new ExtendedWebElement(frame, rootFrame, Driver.GetAllElementAttributes(frame))))
                {
                    return (null, 0);
                }
            }
            rootFrame = Driver.GetCurrentFrame();
            var children = Driver.FindIFramesInFrame(rootFrame);

            if (children.Any())
            {
                var childrenFrames = children.Select(x => GetDeepestFrame(rootFrame, x, level + 1)).ToList();
                var maxLevel = childrenFrames.Max(x => x.level);
                return childrenFrames.First(x => x.level == maxLevel);
            }

            return (frame, level);
        }


        [Fact]
        public async Task ShouldBuildFramesTree_AndReturnToOriginalFrame()
        {
            await SetDriverUrl(TestUri);
            var rootFrame = Driver.GetCurrentFrame();
            var rootElement = Driver.GetCurrentRootWebElement();
            Driver.SwitchTo().Frame(Driver.FindIFramesInFrame(rootFrame).First().WebElement);
            var currentFrame = Driver.GetCurrentFrame();
            Driver.GetFullFramesTree();
            var afterOperationFrame = Driver.GetCurrentFrame();
            
            
            Assert.True(rootFrame.IsRoot);
            Assert.False(rootFrame.IsFrame);
            Assert.NotNull(rootElement);
            Assert.NotNull(currentFrame);
            Assert.NotNull(afterOperationFrame);
            Assert.True(currentFrame.Equals(afterOperationFrame));
            // Assert.Equal(currentFrame.WebElement, afterOperationFrame.WebElement);
        }

        [Fact]
        public async Task ShouldBuildProperFramesTree_AndReturnToOriginalFrame()
        {
            await SetDriverUrl(TestUri);
            var frameBeforeTraversingThrough = Driver.GetCurrentFrame();
            Driver.GetFullFramesTree();
            Assert.Equal(frameBeforeTraversingThrough, Driver.GetCurrentFrame());
        }
    }