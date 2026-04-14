using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.IO;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ09_MatchListStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;
        private AutomationElement _matchesGrid;
        private string _newNickname = "TestNick";

        [BeforeScenario("FZ09")]
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

        [AfterScenario("FZ09")]
        public void CleanUp()
        {
            try
            {
                _automation?.Dispose();
                if (_application != null)
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

        [Given("I am on the Match screen")]
        public void GivenIAmOnTheMatchScreen()
        {
            var matchButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnMatch"))?.AsButton()?.WaitUntilClickable();
            Assert.IsNotNull(matchButton, "Match button was not found.");
            matchButton.Invoke();
            Thread.Sleep(3000);
        }

        [When("the Match screen is opened")]
        public void WhenTheMatchScreenIsOpened()
        {
            // Match forma je otvorena u backgroundu
        }

        [Then("matched users should be displayed in the list")]
        public void ThenMatchedUsersShouldBeDisplayedInTheList()
        {
            _matchesGrid = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("dgvMatches"));
            Assert.IsNotNull(_matchesGrid, "Match list was not found.");
            var rows = _matchesGrid.FindAllChildren();
            Assert.IsTrue(rows.Length > 1, "No matched users are displayed in the list.");
        }

        [Given("at least one matched user is displayed")]
        public void GivenAtLeastOneMatchedUserIsDisplayed()
        {
            _matchesGrid = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("dgvMatches"));
            Assert.IsNotNull(_matchesGrid, "Match list was not found.");
            var rows = _matchesGrid.FindAllChildren();
            Assert.IsTrue(rows.Length > 1, "No matched users are displayed in the list.");
            rows[1].Click();
            Thread.Sleep(1000);
        }

        [When("I enter a nickname for the matched user")]
        public void WhenIEnterANicknameForTheMatchedUser()
        {
            var nickname = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName("Nickname Row 0"))?.AsTextBox()?.WaitUntilClickable();
            Assert.IsNotNull(nickname, "Nickname cell was not found");
            nickname.Click();
            Thread.Sleep(500);
            FlaUI.Core.Input.Keyboard.Type(_newNickname);
            Thread.Sleep(500);
        }

        [When("I click the save button")]
        public void WhenIClickTheSaveButton()
        {
            var saveButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnSave"))?.AsButton()?.WaitUntilClickable();
            Assert.IsNotNull(saveButton, "Save button was not found.");
            saveButton.Invoke();
            Thread.Sleep(1500);
            var succesMessage = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName("Nickname uspješno promjenjen"));
            Assert.IsNotNull(succesMessage, "Success nickname message was not found.");
            var okButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName("OK"))?.AsButton()?.WaitUntilClickable();
            Assert.IsNotNull(okButton, "OK button was not found.");
            okButton.Invoke();
            Thread.Sleep(1000);
        }

        [Then("the nickname should be saved")]
        public void ThenTheNicknameShouldBeSaved()
        {
            var nickname = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName("Nickname Row 0"))?.AsTextBox();
            Assert.IsNotNull(nickname, "Nickname cell was not found.");
            Assert.AreEqual(_newNickname, nickname.Text, "Nickname was not saved correctly.");
        }

    }
}
