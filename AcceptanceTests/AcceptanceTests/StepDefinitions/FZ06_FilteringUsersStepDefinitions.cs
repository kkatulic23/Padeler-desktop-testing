using System;
using System.IO;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ06_FilteringUsersStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;
        private Window _mainWindow;

        private string _rememberedPlayer;

        [BeforeScenario("FZ06")]
        public void Setup()
        {
            _application = Application.Launch(GetAppPath());
            _application.WaitWhileMainHandleIsMissing();
            _automation = new UIA3Automation();
            _mainWindow = _application.GetMainWindow(_automation);
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

        [AfterScenario("FZ06")]
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

        [Given("I launch the application for filtering users")]
        public void GivenILaunchTheApplicationForFilteringUsers()
        {
            Assert.IsNotNull(_application, "Application was not launched.");
            Assert.IsNotNull(_mainWindow, "Main window was not found.");
        }

        [Given("I am on the home form for filtering users")]
        public void GivenIAmOnTheHomeFormForFilteringUsers()
        {
            _mainWindow = _application.GetMainWindow(_automation);

            var homeButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnHome"));
            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));

            Assert.IsNotNull(homeButton, "Home button was not found.");
            Assert.IsNotNull(playerLabel, "Player label was not found.");
        }

        [When("I open the filter form for user filtering")]
        public void WhenIOpenTheFilterFormForUserFiltering()
        {
            _mainWindow = _application.GetMainWindow(_automation);

            var filterButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnFilter"));
            Assert.IsNotNull(filterButton, "Filter button was not found.");

            filterButton.Focus();
            Thread.Sleep(200);
            filterButton.Click();
            Thread.Sleep(1500);

            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not opened.");
        }

        [When("I apply filters with gender Male")]
        public void WhenIApplyFiltersWithGenderMale()
        {
            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not found.");

            var genderCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboGender"))?.AsComboBox();
            var levelCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboLevel"))?.AsComboBox();
            var positionCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboPosition"))?.AsComboBox();
            var frequencyCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboFrequency"))?.AsComboBox();

            Assert.IsNotNull(genderCombo, "Gender combo box was not found.");
            Assert.IsNotNull(levelCombo, "Level combo box was not found.");
            Assert.IsNotNull(positionCombo, "Position combo box was not found.");
            Assert.IsNotNull(frequencyCombo, "Frequency combo box was not found.");

            genderCombo.Focus();
            Thread.Sleep(200);
            genderCombo.Select("Male");
            Thread.Sleep(300);

            levelCombo.Focus();
            Thread.Sleep(200);
            levelCombo.Select("Any");
            Thread.Sleep(300);

            positionCombo.Focus();
            Thread.Sleep(200);
            positionCombo.Select("Any");
            Thread.Sleep(300);

            frequencyCombo.Focus();
            Thread.Sleep(200);
            frequencyCombo.Select("Any");
            Thread.Sleep(500);

            ApplyFiltersAndReturnHome(filterForm);
        }

        [When("I apply filters with gender Female")]
        public void WhenIApplyFiltersWithGenderFemale()
        {
            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not found.");

            var genderCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboGender"))?.AsComboBox();
            var levelCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboLevel"))?.AsComboBox();
            var positionCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboPosition"))?.AsComboBox();
            var frequencyCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboFrequency"))?.AsComboBox();

            Assert.IsNotNull(genderCombo, "Gender combo box was not found.");
            Assert.IsNotNull(levelCombo, "Level combo box was not found.");
            Assert.IsNotNull(positionCombo, "Position combo box was not found.");
            Assert.IsNotNull(frequencyCombo, "Frequency combo box was not found.");

            genderCombo.Focus();
            Thread.Sleep(200);
            genderCombo.Select("Female");
            Thread.Sleep(300);

            levelCombo.Focus();
            Thread.Sleep(200);
            levelCombo.Select("Any");
            Thread.Sleep(300);

            positionCombo.Focus();
            Thread.Sleep(200);
            positionCombo.Select("Any");
            Thread.Sleep(300);

            frequencyCombo.Focus();
            Thread.Sleep(200);
            frequencyCombo.Select("Any");
            Thread.Sleep(500);

            ApplyFiltersAndReturnHome(filterForm);
        }

        [When("I apply filters with gender Male and level Intermediate")]
        public void WhenIApplyFiltersWithGenderMaleAndLevelIntermediate()
        {
            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not found.");

            var genderCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboGender"))?.AsComboBox();
            var levelCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboLevel"))?.AsComboBox();
            var positionCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboPosition"))?.AsComboBox();
            var frequencyCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboFrequency"))?.AsComboBox();

            Assert.IsNotNull(genderCombo, "Gender combo box was not found.");
            Assert.IsNotNull(levelCombo, "Level combo box was not found.");
            Assert.IsNotNull(positionCombo, "Position combo box was not found.");
            Assert.IsNotNull(frequencyCombo, "Frequency combo box was not found.");

            genderCombo.Focus();
            Thread.Sleep(200);
            genderCombo.Select("Male");
            Thread.Sleep(300);

            levelCombo.Focus();
            Thread.Sleep(200);
            levelCombo.Select("Intermediate");
            Thread.Sleep(300);

            positionCombo.Focus();
            Thread.Sleep(200);
            positionCombo.Select("Any");
            Thread.Sleep(300);

            frequencyCombo.Focus();
            Thread.Sleep(200);
            frequencyCombo.Select("Any");
            Thread.Sleep(500);

            ApplyFiltersAndReturnHome(filterForm);
        }

        [When("I apply filters with gender Male, level Intermediate, position Right and frequency Weekly")]
        public void WhenIApplyFiltersWithAllSelectedCriteria()
        {
            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not found.");

            var genderCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboGender"))?.AsComboBox();
            var levelCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboLevel"))?.AsComboBox();
            var positionCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboPosition"))?.AsComboBox();
            var frequencyCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboFrequency"))?.AsComboBox();

            Assert.IsNotNull(genderCombo, "Gender combo box was not found.");
            Assert.IsNotNull(levelCombo, "Level combo box was not found.");
            Assert.IsNotNull(positionCombo, "Position combo box was not found.");
            Assert.IsNotNull(frequencyCombo, "Frequency combo box was not found.");

            genderCombo.Focus();
            Thread.Sleep(200);
            genderCombo.Select("Male");
            Thread.Sleep(300);

            levelCombo.Focus();
            Thread.Sleep(200);
            levelCombo.Select("Intermediate");
            Thread.Sleep(300);

            positionCombo.Focus();
            Thread.Sleep(200);
            positionCombo.Select("Right");
            Thread.Sleep(300);

            frequencyCombo.Focus();
            Thread.Sleep(200);
            frequencyCombo.Select("Weekly");
            Thread.Sleep(500);

            ApplyFiltersAndReturnHome(filterForm);
        }

        [When("I apply filters with all values set to Any")]
        public void WhenIApplyFiltersWithAllValuesSetToAny()
        {
            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not found.");

            var genderCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboGender"))?.AsComboBox();
            var levelCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboLevel"))?.AsComboBox();
            var positionCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboPosition"))?.AsComboBox();
            var frequencyCombo = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("cboFrequency"))?.AsComboBox();

            Assert.IsNotNull(genderCombo, "Gender combo box was not found.");
            Assert.IsNotNull(levelCombo, "Level combo box was not found.");
            Assert.IsNotNull(positionCombo, "Position combo box was not found.");
            Assert.IsNotNull(frequencyCombo, "Frequency combo box was not found.");

            genderCombo.Focus();
            Thread.Sleep(200);
            genderCombo.Select("Any");
            Thread.Sleep(300);

            levelCombo.Focus();
            Thread.Sleep(200);
            levelCombo.Select("Any");
            Thread.Sleep(300);

            positionCombo.Focus();
            Thread.Sleep(200);
            positionCombo.Select("Any");
            Thread.Sleep(300);

            frequencyCombo.Focus();
            Thread.Sleep(200);
            frequencyCombo.Select("Any");
            Thread.Sleep(500);

            ApplyFiltersAndReturnHome(filterForm);
        }

        [When("I remember the first displayed player")]
        public void WhenIRememberTheFirstDisplayedPlayer()
        {
            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            Assert.IsNotNull(playerLabel, "Player label was not found.");

            _rememberedPlayer = playerLabel.Name?.Trim();
            Assert.IsFalse(string.IsNullOrWhiteSpace(_rememberedPlayer), "Displayed player name is empty.");
        }

        [Then("filtered users should be displayed")]
        public void ThenFilteredUsersShouldBeDisplayed()
        {
            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            var distanceLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblDistance"));

            Assert.IsNotNull(playerLabel, "Player label was not found after filtering.");
            Assert.IsNotNull(distanceLabel, "Distance label was not found after filtering.");

            var playerText = playerLabel.Name?.Trim();
            var distanceText = distanceLabel.Name?.Trim();

            Assert.IsFalse(string.IsNullOrWhiteSpace(playerText), "Player name is empty after filtering.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(distanceText), "Distance is empty after filtering.");
        }

        [Then("the displayed player should be different")]
        public void ThenTheDisplayedPlayerShouldBeDifferent()
        {
            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            Assert.IsNotNull(playerLabel, "Player label was not found after changing filters.");

            var currentPlayer = playerLabel.Name?.Trim();

            Assert.IsFalse(string.IsNullOrWhiteSpace(_rememberedPlayer), "Remembered player was not stored.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(currentPlayer), "Current player is empty.");
            Assert.AreNotEqual(_rememberedPlayer, currentPlayer, "Displayed player did not change after changing filters.");
        }

        private void ApplyFiltersAndReturnHome(AutomationElement filterForm)
        {
            var applyButton = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("btnApply"));
            Assert.IsNotNull(applyButton, "Apply button was not found.");

            applyButton.Focus();
            Thread.Sleep(200);
            applyButton.Click();
            Thread.Sleep(1200);

            var desktop = _automation.GetDesktop();

            var okButton = desktop.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
            Assert.IsNotNull(okButton, "OK button was not found after applying filters.");

            okButton.Focus();
            Thread.Sleep(200);
            okButton.Invoke();
            Thread.Sleep(1200);

            _mainWindow = _application.GetMainWindow(_automation);

            var homeButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnHome"));
            Assert.IsNotNull(homeButton, "Home button was not found after closing popup.");

            homeButton.Focus();
            Thread.Sleep(200);
            homeButton.Click();
            Thread.Sleep(1500);

            _mainWindow = _application.GetMainWindow(_automation);

            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            Assert.IsNotNull(playerLabel, "No users were displayed after applying filters and returning to Home.");
        }
    }
}