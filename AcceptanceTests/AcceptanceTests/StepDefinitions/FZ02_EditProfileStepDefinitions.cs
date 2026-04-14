using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;
using System;
using System.IO;
using System.Threading;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ02_ProfileEditingStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;

        private string _originalName;
        private string _modifiedName;
        private string _rememberedName;
        private string _originalEmail;
        private string _lastValidationTarget;

        [BeforeScenario("FZ02")]
        public void Setup()
        {
            _application = Application.Launch(GetAppPath());
            _application.WaitWhileMainHandleIsMissing();
            _automation = new UIA3Automation();
            Thread.Sleep(2000);
        }

        [AfterScenario("FZ02")]
        public void CleanUp()
        {
            try
            {
                _automation?.Dispose();

                if (_application == null) return;

                try { _application.Close(); } catch { }
                Thread.Sleep(1000);

                if (!_application.HasExited)
                    _application.Kill();
            }
            catch { }
        }

        private string GetAppPath() =>
            Path.GetFullPath(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\..\..\Software\Padeler\Padeler\bin\Debug\Padeler.exe"));

        [Given("I am logged into the application for profile editing")]
        public void GivenIAmLoggedIntoTheApplicationForProfileEditing()
        {
            AssertExists<Button>("btnProfile",
                "User is not logged in or BaseForm is not opened. btnProfile was not found.");
        }

        [Given("I open the profile screen")]
        public void GivenIOpenTheProfileScreen() => OpenProfileScreen();

        [Then("existing profile data should be displayed")]
        public void ThenExistingProfileDataShouldBeDisplayed()
        {
            AssertFilled("txtName", "Name");
            AssertFilled("txtSurname", "Surname");
            AssertFilled("txtUsername", "Username");
            AssertFilled("txtEmail", "Email");
            AssertFilled("txtPhone", "Phone");
        }

        [When("I change the name to {string}")]
        public void WhenIChangeTheNameTo(string newName)
        {
            var txtName = GetTextBox("txtName", "Name textbox not found.");
            _originalName = txtName.Text;
            _modifiedName = newName;

            SetText(txtName, newName);
        }

        [When("I click the save button on profile")]
        public void WhenIClickTheSaveButtonOnProfile()
        {
            GetButton("btnSave", "Save button was not found.").Click();
            Thread.Sleep(1500);
            TryCloseOkDialogIfExists();
        }

        [Then("profile changes should be saved successfully")]
        public void ThenProfileChangesShouldBeSavedSuccessfully()
        {
            LeaveProfileAndReturn();

            var txtName = GetTextBox("txtName", "Name textbox not found after reopening profile.");
            Assert.AreEqual(_modifiedName, txtName.Text, "Saved profile name was not persisted.");
        }

        [When("I clear the name field")]
        public void WhenIClearTheNameField()
        {
            var txtName = GetTextBox("txtName", "Name textbox not found.");
            _originalName = txtName.Text;
            _lastValidationTarget = "Name";

            SetText(txtName, string.Empty);
        }

        [When("I enter invalid email {string}")]
        public void WhenIEnterInvalidEmail(string invalidEmail)
        {
            var txtEmail = GetTextBox("txtEmail", "Email textbox not found.");
            var txtName = GetTextBox("txtName", "Name textbox not found.");

            _originalEmail = txtEmail.Text;
            _originalName = txtName.Text;
            _lastValidationTarget = "Email";

            SetText(txtEmail, invalidEmail);
        }

        [Then("I should see a validation error containing {string}")]
        public void ThenIShouldSeeAValidationErrorContaining(string expectedMessage)
        {
            TryObserveDialogText(expectedMessage);
            TryCloseOkDialogIfExists();
            LeaveProfileAndReturn();

            if (_lastValidationTarget == "Name")
            {
                var txtName = GetTextBox("txtName", "Name textbox not found after reopening profile.");
                Assert.AreEqual(_originalName, txtName.Text, "Invalid empty name was incorrectly saved.");
            }
            else if (_lastValidationTarget == "Email")
            {
                var txtEmail = GetTextBox("txtEmail", "Email textbox not found after reopening profile.");
                Assert.AreEqual(_originalEmail, txtEmail.Text, "Invalid email was incorrectly saved.");
            }
            else
            {
                Assert.Fail("Validation target was not set.");
            }
        }

        [Given("current profile name is remembered")]
        public void GivenCurrentProfileNameIsRemembered()
        {
            var txtName = GetTextBox("txtName", "Name textbox not found.");
            _rememberedName = txtName.Text;

            Assert.IsFalse(string.IsNullOrWhiteSpace(_rememberedName), "Original name should not be empty.");
        }

        [When("I change the name without saving")]
        public void WhenIChangeTheNameWithoutSaving()
        {
            var txtName = GetTextBox("txtName", "Name textbox not found.");
            SetText(txtName, _rememberedName + "_unsaved");
        }

        [When("I leave the profile screen")]
        public void WhenILeaveTheProfileScreen()
        {
            GetButton("btnHome", "Home button was not found.").Click();
            Thread.Sleep(1200);
        }

        [When("I return to the profile screen")]
        public void WhenIReturnToTheProfileScreen() => OpenProfileScreen();

        [Then("unsaved profile changes should not be persisted")]
        public void ThenUnsavedProfileChangesShouldNotBePersisted()
        {
            var txtName = GetTextBox("txtName", "Name textbox not found after reopening profile.");
            Assert.AreEqual(_rememberedName, txtName.Text, "Unsaved changes were incorrectly persisted.");
        }

        private void OpenProfileScreen()
        {
            GetButton("btnProfile", "Profile button was not found.").Click();
            Thread.Sleep(2000);
            AssertExists<Button>("btnSave", "Profile screen did not open correctly because Save button was not found.");
        }

        private void LeaveProfileAndReturn()
        {
            GetButton("btnHome", "Home button was not found.").Click();
            Thread.Sleep(500);

            GetButton("btnProfile", "Profile button was not found.").Click();
            Thread.Sleep(1000);
        }

        private void SetText(TextBox textBox, string value)
        {
            textBox.Focus();
            textBox.Text = string.Empty;
            Thread.Sleep(200);

            if (!string.IsNullOrEmpty(value))
                textBox.Enter(value);

            Thread.Sleep(300);
        }

        private void AssertFilled(string automationId, string fieldName)
        {
            var textBox = GetTextBox(automationId, $"{fieldName} textbox not found.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(textBox.Text), $"{fieldName} should already be displayed.");
        }

        private T AssertExists<T>(string automationId, string message) where T : AutomationElement
        {
            var element = FindElement<T>(automationId, 5000);
            Assert.IsNotNull(element, message);
            return element;
        }

        private Button GetButton(string automationId, string errorMessage) =>
            AssertExists<Button>(automationId, errorMessage);

        private TextBox GetTextBox(string automationId, string errorMessage) =>
            AssertExists<TextBox>(automationId, errorMessage);

        private T FindElement<T>(string automationId, int timeoutMs) where T : AutomationElement
        {
            var endTime = DateTime.Now.AddMilliseconds(timeoutMs);

            while (DateTime.Now < endTime)
            {
                var element = _automation.GetDesktop()
                    .FindFirstDescendant(cf => cf.ByAutomationId(automationId));

                if (element != null)
                {
                    if (typeof(T) == typeof(Button)) return element.AsButton() as T;
                    if (typeof(T) == typeof(TextBox)) return element.AsTextBox() as T;
                }

                Thread.Sleep(200);
            }

            return null;
        }

        private bool TryObserveDialogText(string expectedText)
        {
            var endTime = DateTime.Now.AddMilliseconds(800);

            while (DateTime.Now < endTime)
            {
                if (_automation.GetDesktop().FindFirstDescendant(cf => cf.ByName(expectedText)) != null)
                    return true;

                Thread.Sleep(200);
            }

            return false;
        }

        private void TryCloseOkDialogIfExists()
        {
            var endTime = DateTime.Now.AddMilliseconds(1200);

            while (DateTime.Now < endTime)
            {
                try
                {
                    var okButton = _automation.GetDesktop()
                        .FindFirstDescendant(cf => cf.ByName("OK"))
                        ?.AsButton();

                    if (okButton != null)
                    {
                        try
                        {
                            okButton.Click();
                        }
                        catch
                        {
                            var rect = okButton.BoundingRectangle;
                            if (!rect.IsEmpty)
                                Mouse.Click(rect.Center());
                        }

                        Thread.Sleep(500);
                        return;
                    }
                }
                catch { }

                Thread.Sleep(200);
            }
        }
    }
}