using System;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ04_InteractionStepDefinitions
    {
        private const string appPath = @"C:\Users\Fiæo\source\repos\tikpp2526-fgrgac-kkatulic-kkrsak\Software\Padeler\Padeler\bin\Debug\Padeler.exe";

        private Application _application;
        private UIA3Automation _automation;
        private AutomationElement _frontCard;
        private string _previousPlayerName;

        [BeforeScenario]
        public void Setup()
        {
            _application = Application.Launch(appPath);
            _application.WaitWhileMainHandleIsMissing();
            _automation = new UIA3Automation();
        }

        [AfterScenario]
        public void CleanUp()
        {
            _automation?.Dispose();
            _application?.Close();
        }

        [Given("I am logged into the application")]
        public void GivenIAmLoggedIntoTheApplication()
        {
        }

        [Given("I am on the interaction screen")]
        public void GivenIAmOnTheInteractionScreen()
        {
            var homeButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnHome"))?.AsButton()?.WaitUntilClickable();

            Assert.IsNotNull(homeButton, "Home button was not found!");

            homeButton.Invoke();
        }

        [When("the interaction screen is opened")]
        public void WhenTheInteractionScreenIsOpened()
        {
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
            throw new PendingStepException();
        }

        [When("I click the skip button")]
        public void WhenIClickTheSkipButton()
        {
            throw new PendingStepException();
        }

        [Then("the nex player card should be displayed")]
        public void ThenTheNexPlayerCardShouldBeDisplayed()
        {
            throw new PendingStepException();
        }

        [When("I click the like button")]
        public void WhenIClickTheLikeButton()
        {
            throw new PendingStepException();
        }

        [Then("the next player card should be displayed")]
        public void ThenTheNextPlayerCardShouldBeDisplayed()
        {
            throw new PendingStepException();
        }

        [Given("another player has already clicked the like button")]
        public void GivenAnotherPlayerHasAlreadyClickedTheLikeButton()
        {
            throw new PendingStepException();
        }

        [Then("a match notificaiton should be displayed")]
        public void ThenAMatchNotificaitonShouldBeDisplayed()
        {
            throw new PendingStepException();
        }

    }
}
