using System;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;
using System.IO;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ04_InteractionStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;
        private AutomationElement _frontCard;
        private string _previousPlayerName;

        [BeforeScenario("FZ04")]
        public void Setup()
        {
            _application = Application.Launch(GetAppPath());
            _application.WaitWhileMainHandleIsMissing();
            _automation = new UIA3Automation();
            Thread.Sleep(2000);
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

        [AfterScenario("FZ04")]
        public void CleanUp()
        {
            try
            {
                _automation?.Dispose();
                if(_application != null)
                {
                    _application?.Close();
                    Thread.Sleep(1000);

                    if (!_application.HasExited)
                    {
                        _application.Kill();
                    }
                }
            }
            catch { }
        }

        [Given("I am logged into the application")]
        public void GivenIAmLoggedIntoTheApplication()
        {
            //Podrazumijeva se da je korisnik prijavljen.
        }

        [Given("I am on the interaction screen")]
        public void GivenIAmOnTheInteractionScreen()
        {
            var homeButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnHome"))?.AsButton()?.WaitUntilClickable();
            Assert.IsNotNull(homeButton, "Home button was not found!");
            homeButton.Invoke();
            Thread.Sleep(1500);
        }

        [When("the interaction screen is opened")]
        public void WhenTheInteractionScreenIsOpened()
        {
            //Otvaranje forme za interakciju je pokriveno u backgorund djelu.
        }

        [Then("player cards should be displayed")]
        public void ThenPlayerCardsShouldBeDisplayed()
        {
            _frontCard = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("pnlFrontCard"))?.WaitUntilClickable();
            Assert.IsNotNull(_frontCard, "Player card was not found!");
            var playerName = _frontCard.FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"))?.AsLabel()?.Text;
            Assert.IsFalse(string.IsNullOrWhiteSpace(playerName), "Player name is not displayed!");
        }

        [Given("at least two cards are available")]
        public void GivenAtLeastTwoCardsAreAvailable()
        {
            _frontCard = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("pnlFrontCard"))?.WaitUntilClickable();
            Assert.IsNotNull(_frontCard, "Player card was not found!");
            Thread.Sleep(1000);
            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"))?.AsLabel();
            Assert.IsNotNull(playerLabel, "Player label was not found!");
            _previousPlayerName = playerLabel.Text;
            Assert.IsFalse(string.IsNullOrWhiteSpace(_previousPlayerName), "Current player name is empty");
        }

        [When("I click the skip button")]
        public void WhenIClickTheSkipButton()
        {
            var skipButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("pbDisslike"))?.WaitUntilClickable();
            Assert.IsNotNull(skipButton, "Skip button was not found!");
            skipButton.Click();
            Thread.Sleep(1500);
        }

        [When("I click the like button")]
        public void WhenIClickTheLikeButton()
        {
            var likeButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("pbLike"))?.WaitUntilClickable();
            Assert.IsNotNull(likeButton, "Like button was not found!");
            likeButton.Click();
            Thread.Sleep(1500);
        }

        [Then("the next player card should be displayed")]
        public void ThenTheNextPlayerCardShouldBeDisplayed()
        {
            var currentCard = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("pnlFrontCard"))?.WaitUntilClickable();
            Assert.IsNotNull(currentCard, "No player card is displayed after action");
            Thread.Sleep(1000);
            var currentPlayerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"))?.AsLabel();
            Assert.IsNotNull(currentPlayerLabel, "Player label was not found on the new card.");
            var currentPlayerName = currentPlayerLabel.Text;
            Assert.IsFalse(string.IsNullOrWhiteSpace(currentPlayerName), "New player name is empty!");
            Assert.AreNotEqual(_previousPlayerName, currentPlayerName, "The next player card was not displayed because the same player is still shown.");
        }

        [When("I click the info button")]
        public void WhenIClickTheInfoButton()
        {
            var infoButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("pbInfoFront"))?.AsButton()?.WaitUntilClickable();
            Assert.IsNotNull(infoButton, "Info button was not found!");
            infoButton.Click();
            Thread.Sleep(1000);
        }

        [Then("I should see more informations about player")]
        public void ThenIShouldSeeMoreInformationsAboutPlayer()
        {
            var backCard = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("pnlBackCard"))?.WaitUntilClickable();
            Assert.IsNotNull(backCard, "No player card info is displayed after action");
            Thread.Sleep(1000);
            var aboutLabel = backCard.FindFirstDescendant(cf => cf.ByAutomationId("lblAbout"))?.AsLabel();
            Assert.IsNotNull(aboutLabel, "About player section was not displayed.");
            var levelLabel = backCard.FindFirstDescendant(cf => cf.ByAutomationId("lblLevel"))?.AsLabel();
            Assert.IsNotNull(levelLabel, "Player level information was not displayed.");
            var frequencyLabel = backCard.FindFirstDescendant(cf => cf.ByAutomationId("lblFrequency"))?.AsLabel();
            Assert.IsNotNull(frequencyLabel, "Player frequency information was not displayed.");
            var positionLabel = backCard.FindFirstDescendant(cf => cf.ByAutomationId("lblPosition"))?.AsLabel();
            Assert.IsNotNull(positionLabel, "Player position information was not displayed.");
        }

    }
}
