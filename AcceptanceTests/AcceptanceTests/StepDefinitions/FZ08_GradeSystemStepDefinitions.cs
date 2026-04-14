using System;
using System.IO;
using System.Linq;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ08_GradeSystemStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;
        private AutomationElement _matchesGrid;

        [BeforeScenario("FZ08")]
        public void Setup()
        {
            _application = Application.Launch(GetAppPath());
            _application.WaitWhileMainHandleIsMissing();
            _automation = new UIA3Automation();
            Thread.Sleep(2000);
        }

        [AfterScenario("FZ08")]
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

        [Given("I am logged into the application for rating")]
        public void GivenIAmLoggedIntoTheApplicationForRating()
        {
        }

        [Given("I open the match list")]
        public void GivenIOpenTheMatchList()
        {
            var matchButton = _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId("btnMatch"))
                ?.AsButton()
                ?.WaitUntilClickable();

            Assert.IsNotNull(matchButton, "Match button was not found.");
            matchButton.Click();
            Thread.Sleep(2500);

            _matchesGrid = _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId("dgvMatches"));

            Assert.IsNotNull(_matchesGrid, "Matches grid was not found.");
        }

        [When("I open the rating form for the first available match")]
        public void WhenIOpenTheRatingFormForTheFirstAvailableMatch()
        {
            _matchesGrid = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("dgvMatches"));

            Assert.IsNotNull(_matchesGrid, "Matches grid was not found.");

            var rows = _matchesGrid.FindAllChildren();
            Assert.IsTrue(rows.Length > 1, "No match rows are displayed.");

            rows[1].Click();
            Thread.Sleep(500);

            var gradeCell = _automation.GetDesktop().FindAllDescendants(cf => cf.ByName(" Row 0"))[1];

            Assert.IsNotNull(gradeCell, "Grade cell was not found.");
            gradeCell.Click();
            Thread.Sleep(1500);
        }

        [Then("the rating form should be displayed")]
        public void ThenTheRatingFormShouldBeDisplayed()
        {
            var gradeInput = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("nudGrade"));

            var commentBox = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("rtbComment"));

            var sendButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnSend"));

            Assert.IsNotNull(gradeInput, "Grade input was not found.");
            Assert.IsNotNull(commentBox, "Comment box was not found.");
            Assert.IsNotNull(sendButton, "Send button was not found.");
        }

        [When("I set the rating to {int}")]
        public void WhenISetTheRatingTo(int rating)
        {
            var gradeInput = _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId("nudGrade"));

            Assert.IsNotNull(gradeInput, "Grade input was not found.");

            gradeInput.Click();
            Thread.Sleep(300);

            for (int i = 0; i < 5; i++)
            {
                Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.DOWN);
                Thread.Sleep(50);
            }

            for (int i = 1; i < rating; i++)
            {
                Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.UP);
                Thread.Sleep(50);
            }

            Thread.Sleep(300);
        }

        [When("I enter the comment {string}")]
        public void WhenIEnterTheComment(string comment)
        {
            var commentBox = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("rtbComment"))?.AsTextBox();

            Assert.IsNotNull(commentBox, "Comment box was not found.");

            commentBox.Focus();
            commentBox.Enter(comment);
            Thread.Sleep(300);
        }

        [When("I submit the rating")]
        public void WhenISubmitTheRating()
        {
            var sendButton = _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId("btnSend"))
                ?.AsButton()?.WaitUntilClickable();

            Assert.IsNotNull(sendButton, "Send button was not found.");
            sendButton.Invoke();
            Thread.Sleep(1500);
        }

        [Then("the rating should be saved successfully")]
        public void ThenTheRatingShouldBeSavedSuccessfully()
        {
            var successMessage = WaitForElementByName("Grade saved!", 5000);
            Assert.IsNotNull(successMessage, "Success message was not found.");

            var okButton = WaitForButtonByName("OK", 5000);
            Assert.IsNotNull(okButton, "OK button was not found.");
            okButton.Click();
        }

        [Given("I have already rated the first available match user")]
        public void GivenIHaveAlreadyRatedTheFirstAvailableMatchUser()
        {
        }

        [When("I try to rate the same user again")]
        public void WhenITryToRateTheSameUserAgain()
        {
            WhenIOpenTheRatingFormForTheFirstAvailableMatch();
            ThenTheRatingFormShouldBeDisplayed();
            WhenISetTheRatingTo(4);
            WhenIEnterTheComment("Drugi unos");
            WhenISubmitTheRating();
        }

        [Then("an error message about duplicate rating should appear")]
        public void ThenAnErrorMessageAboutDuplicateRatingShouldAppear()
        {
            var errorMessage = WaitForElementByName("You have already graded this user.", 5000);
            Assert.IsNotNull(errorMessage, "Duplicate rating error message was not found.");

            var okButton = WaitForButtonByName("OK", 5000);
            Assert.IsNotNull(okButton, "OK button was not found.");
            okButton.Click();
        }

        private AutomationElement WaitForElementByName(string name, int timeoutMs)
        {
            var endTime = DateTime.Now.AddMilliseconds(timeoutMs);

            while (DateTime.Now < endTime)
            {
                var element = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName(name));
                if (element != null)
                {
                    return element;
                }

                Thread.Sleep(200);
            }

            return null;
        }

        private Button WaitForButtonByName(string name, int timeoutMs)
        {
            var endTime = DateTime.Now.AddMilliseconds(timeoutMs);

            while (DateTime.Now < endTime)
            {
                var button = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName(name))?.AsButton();

                if (button != null)
                {
                    return button.WaitUntilClickable();
                }

                Thread.Sleep(200);
            }

            return null;
        }
    }
}