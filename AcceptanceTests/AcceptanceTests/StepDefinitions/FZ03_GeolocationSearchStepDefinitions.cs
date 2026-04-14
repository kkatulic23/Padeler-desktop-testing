using System;
using System.Globalization;
using System.IO;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ03_GeolocationSearchStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;
        private Window _mainWindow;

        private int _selectedRadiusKm;

        [BeforeScenario("FZ03")]
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

        [AfterScenario("FZ03")]
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

        [Given("I launch the application for geolocation")]
        public void GivenILaunchTheApplication()
        {
            Assert.IsNotNull(_application, "Application was not launched.");
            Assert.IsNotNull(_mainWindow, "Main window was not found.");
        }

        [Given("I am on the home form")]
        public void GivenIAmOnTheHomeForm()
        {
            _mainWindow = _application.GetMainWindow(_automation);

            var homeButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnHome"));
            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            var distanceLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblDistance"));

            Assert.IsNotNull(homeButton, "Home button was not found.");
            Assert.IsNotNull(playerLabel, "Player label was not found.");
            Assert.IsNotNull(distanceLabel, "Distance label was not found.");
        }

        [When("I open the filter form")]
        public void WhenIOpenTheFilterForm()
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

        [When(@"I apply geolocation filters with radius (.*) km")]
        public void WhenIApplyGeolocationFiltersWithRadiusKm(int radiusKm)
        {
            _selectedRadiusKm = radiusKm;

            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not found.");
            var radiusSlider = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("trkRadius"))?.AsSlider();
            Assert.IsNotNull(radiusSlider, "Radius slider was not found.");
            radiusSlider.Focus();
            Thread.Sleep(300);

            Keyboard.Press(VirtualKeyShort.HOME);
            Thread.Sleep(300);

            for (int i = 1; i < radiusKm; i++)
            {
                Keyboard.Press(VirtualKeyShort.RIGHT);
                Thread.Sleep(30);
            }

            Thread.Sleep(500);

            var applyButton = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("btnApply"));
            Assert.IsNotNull(applyButton, "Apply button was not found.");
            applyButton.Click();
            Thread.Sleep(1000);

            var desktop = _automation.GetDesktop();
            var messageBox = desktop.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(messageBox, "Confirmation popup was not found after applying filters.");

            var okButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
            Assert.IsNotNull(okButton, "OK button was not found on confirmation popup.");
            okButton.Invoke();
            Thread.Sleep(1000);

            _mainWindow = _application.GetMainWindow(_automation);

            var homeButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnHome"));
            Assert.IsNotNull(homeButton, "Home button was not found after closing popup.");
            homeButton.Focus();
            Thread.Sleep(200);
            homeButton.Click();
            Thread.Sleep(1500);

            _mainWindow = _application.GetMainWindow(_automation);

            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            var distanceLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblDistance"));

            Assert.IsNotNull(playerLabel, "Player label was not found after applying filters.");
            Assert.IsNotNull(distanceLabel, "Distance label was not found after applying filters.");
        }

        [When("I apply geolocation filters with a random radius")]
        public void WhenIApplyGeolocationFiltersWithARandomRadius()
        {
            var random = new Random();
            _selectedRadiusKm = random.Next(2, 51);

            var filterForm = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("FilterForm"));
            Assert.IsNotNull(filterForm, "Filter form was not found.");
            var radiusSlider = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("trkRadius"))?.AsSlider();
            Assert.IsNotNull(radiusSlider, "Radius slider was not found.");
            radiusSlider.Focus();
            Thread.Sleep(300);

            Keyboard.Press(VirtualKeyShort.HOME);
            Thread.Sleep(300);

            for (int i = 1; i < _selectedRadiusKm; i++)
            {
                Keyboard.Press(VirtualKeyShort.RIGHT);
                Thread.Sleep(30);
            }

            Thread.Sleep(500);

            var applyButton = filterForm.FindFirstDescendant(cf => cf.ByAutomationId("btnApply"));
            Assert.IsNotNull(applyButton, "Apply button was not found.");

            applyButton.Click();
            Thread.Sleep(1000);

            var desktop = _automation.GetDesktop();
            var messageBox = desktop.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(messageBox, "Confirmation popup was not found after applying filters.");
            var okButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
            Assert.IsNotNull(okButton, "OK button was not found on confirmation popup.");
            okButton.Invoke();
            Thread.Sleep(1000);

            _mainWindow = _application.GetMainWindow(_automation);

            var homeButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnHome"));
            Assert.IsNotNull(homeButton, "Home button was not found after closing popup.");
            homeButton.Focus();
            Thread.Sleep(200);
            homeButton.Click();
            Thread.Sleep(1500);

            _mainWindow = _application.GetMainWindow(_automation);

            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            var distanceLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblDistance"));

            Assert.IsNotNull(playerLabel, "Player label was not found after applying filters.");
            Assert.IsNotNull(distanceLabel, "Distance label was not found after applying filters.");
        }

        [Then("nearby players should be displayed on the home screen")]
        public void ThenNearbyPlayersShouldBeDisplayedOnTheHomeScreen()
        {
            var playerLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblPlayer"));
            var distanceLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblDistance"));

            Assert.IsNotNull(playerLabel, "Player label was not found.");
            Assert.IsNotNull(distanceLabel, "Distance label was not found.");

            var playerText = playerLabel.Name?.Trim();
            var distanceText = distanceLabel.Name?.Trim();

            Assert.IsFalse(string.IsNullOrWhiteSpace(playerText), "Player name is empty.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(distanceText), "Distance is empty.");
            Assert.IsTrue(distanceText.Contains("km"), "Distance is not shown in km.");
        }

        [Then(@"displayed players should be within (.*) km")]
        public void ThenDisplayedPlayersShouldBeWithinKm(int expectedRadiusKm)
        {
            var distanceLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblDistance"));
            Assert.IsNotNull(distanceLabel, "Distance label was not found.");

            var distanceText = distanceLabel.Name?.Trim();
            Assert.IsFalse(string.IsNullOrWhiteSpace(distanceText), "Distance text is empty.");

            string normalized = distanceText.Replace("km", "").Trim().Replace(",", ".");
            bool parsed = double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out double distance);

            Assert.IsTrue(parsed, $"Distance value '{distanceText}' could not be parsed.");
            Assert.IsTrue(distance <= expectedRadiusKm, $"Displayed player is outside expected radius. Distance: {distance} km, expected: <= {expectedRadiusKm} km.");
        }

        [Then("displayed players should be within the selected random radius")]
        public void ThenDisplayedPlayersShouldBeWithinTheSelectedRandomRadius()
        {
            var distanceLabel = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("lblDistance"));
            Assert.IsNotNull(distanceLabel, "Distance label was not found.");
            var distanceText = distanceLabel.Name?.Trim();
            Assert.IsFalse(string.IsNullOrWhiteSpace(distanceText), "Distance text is empty.");
            string normalized = distanceText.Replace("km", "").Trim().Replace(",", ".");
            bool parsed = double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out double distance);

            Assert.IsTrue(parsed, $"Distance value '{distanceText}' could not be parsed.");
            Assert.IsTrue(distance <= _selectedRadiusKm, $"Displayed player is outside expected radius. Distance: {distance} km, expected: <= {_selectedRadiusKm} km.");
        }
    }
}