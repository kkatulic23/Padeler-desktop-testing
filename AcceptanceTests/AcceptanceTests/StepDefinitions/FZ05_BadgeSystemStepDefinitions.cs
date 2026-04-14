using System;
using System.IO;
using System.Linq;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ05_BadgesStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;

        private int _initialBadgeCount;

        [BeforeScenario("FZ05")]
        public void Setup()
        {
            _application = Application.Launch(GetAppPath());
            _application.WaitWhileMainHandleIsMissing();
            _automation = new UIA3Automation();
            Thread.Sleep(2000);
        }

        [AfterScenario("FZ05")]
        public void CleanUp()
        {
            try
            {
                _automation?.Dispose();

                if (_application != null)
                {
                    _application.Close();
                    Thread.Sleep(1000);

                    if (!_application.HasExited)
                    {
                        _application.Kill();
                    }
                }
            }
            catch
            {
            }
        }

        private string GetAppPath()
        {
            return Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\..\..\Software\Padeler\Padeler\bin\Debug\Padeler.exe"
                )
            );
        }

        [Given("I am logged into the application for badges")]
        public void GivenIAmLoggedIntoTheApplicationForBadges()
        {
        }

        [Given("I am on the Home screen")]
        public void GivenIAmOnTheHomeScreen()
        {
            var homeButton = FindButton("btnHome");
            Assert.IsNotNull(homeButton, "Home button was not found.");

            homeButton.Invoke();
            Thread.Sleep(2000);
        }

        [When("the Home screen is opened")]
        public void WhenTheHomeScreenIsOpened()
        {
        }

        [Then("earned badges should be displayed in the badges panel")]
        public void ThenEarnedBadgesShouldBeDisplayedInTheBadgesPanel()
        {
            var badgesPanel = FindElement("pnlBadges");
            Assert.IsNotNull(badgesPanel, "Badges panel was not found.");

            Thread.Sleep(1500);

            var badgeImages = badgesPanel.FindAllChildren();
            Assert.IsTrue(badgeImages.Length > 0, "No badges are displayed in the badges panel.");
        }

        [Given("I remember the current number of displayed badges")]
        public void GivenIRememberTheCurrentNumberOfDisplayedBadges()
        {
            var badgesPanel = FindElement("pnlBadges");
            Assert.IsNotNull(badgesPanel, "Badges panel was not found.");

            Thread.Sleep(1500);
            _initialBadgeCount = badgesPanel.FindAllChildren().Length;
        }

        [When("I perform one swipe action")]
        public void WhenIPerformOneSwipeAction()
        {
            var dislikeButton = FindElement("pbDisslike");
            Assert.IsNotNull(dislikeButton, "Dislike button was not found.");

            dislikeButton.Click();
            Thread.Sleep(2500);

            TryCloseBadgeDialogIfExists();
            Thread.Sleep(1500);
        }

        [Then("a badge unlock message may appear")]
        public void ThenABadgeUnlockMessageMayAppear()
        {
            TryCloseBadgeDialogIfExists();
        }

        [Then("the number of displayed badges should stay the same or increase")]
        public void ThenTheNumberOfDisplayedBadgesShouldStayTheSameOrIncrease()
        {
            var badgesPanel = FindElement("pnlBadges");
            Assert.IsNotNull(badgesPanel, "Badges panel was not found.");

            Thread.Sleep(1500);
            var currentBadgeCount = badgesPanel.FindAllChildren().Length;

            Assert.IsTrue(
                currentBadgeCount >= _initialBadgeCount,
                $"Badge count decreased. Initial: {_initialBadgeCount}, Current: {currentBadgeCount}"
            );
        }

        [Given("badges are already displayed")]
        public void GivenBadgesAreAlreadyDisplayed()
        {
            var badgesPanel = FindElement("pnlBadges");
            Assert.IsNotNull(badgesPanel, "Badges panel was not found.");

            Thread.Sleep(1500);
            var badgeImages = badgesPanel.FindAllChildren();
            Assert.IsTrue(badgeImages.Length > 0, "No badges are displayed.");
        }

        [When("I reopen the Home screen")]
        public void WhenIReopenTheHomeScreen()
        {
            var profileButton = FindButton("btnProfile");
            Assert.IsNotNull(profileButton, "Profile button was not found.");
            profileButton.Invoke();
            Thread.Sleep(1500);

            var homeButton = FindButton("btnHome");
            Assert.IsNotNull(homeButton, "Home button was not found.");
            homeButton.Invoke();
            Thread.Sleep(2000);
        }
        [Then("earned badges should still be displayed in the badges panel")]
        public void ThenEarnedBadgesShouldStillBeDisplayedInTheBadgesPanel()
        {
            var badgesPanel = FindElement("pnlBadges");
            Assert.IsNotNull(badgesPanel, "Badges panel was not found.");

            Thread.Sleep(1500);

            var badgeImages = badgesPanel.FindAllChildren();
            Assert.IsTrue(badgeImages.Length > 0, "No badges are displayed in the badges panel.");
        }
        private Button FindButton(string automationId)
        {
            return _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId(automationId))
                ?.AsButton()
                ?.WaitUntilClickable();
        }

        private AutomationElement FindElement(string automationId)
        {
            return _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId(automationId))
                ?.WaitUntilClickable();
        }

        private void TryCloseBadgeDialogIfExists()
        {
            var endTime = DateTime.Now.AddSeconds(3);

            while (DateTime.Now < endTime)
            {
                try
                {
                    var badgeDialog = _automation.GetDesktop()
                        .FindFirstDescendant(cf => cf.ByName("Badge unlocked"));

                    if (badgeDialog != null)
                    {
                        var okButton = _automation.GetDesktop()
                            .FindFirstDescendant(cf => cf.ByName("OK"))
                            ?.AsButton()
                            ?.WaitUntilClickable();

                        if (okButton != null)
                        {
                            okButton.Invoke();
                            Thread.Sleep(500);
                        }

                        return;
                    }
                }
                catch
                {
                }

                Thread.Sleep(200);
            }
        }
    }
}