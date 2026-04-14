using System;
using System.IO;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class FZ01_AuthenticationStepDefinitions
    {
        private Application _application;
        private UIA3Automation _automation;
        private Window _mainWindow;

        [BeforeScenario("FZ01")]
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

        [AfterScenario("FZ01")]
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

        [Given("I launch the application")]
        public void GivenILaunchTheApplication()
        {
            Assert.IsNotNull(_application, "Application was not launched.");
            Assert.IsNotNull(_mainWindow, "Main window was not found.");
        }

        [Given("I open the registration form")]
        public void GivenIOpenTheRegistrationForm()
        {
            var registerText = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("llRegister"));
            Assert.IsNotNull(registerText, "Register text was not found.");
            var rect = registerText.BoundingRectangle;
            var centerX = rect.Left + rect.Width / 2;
            var centerY = rect.Top + rect.Height / 2;
            Mouse.MoveTo(new System.Drawing.Point((int)centerX, (int)centerY));
            Thread.Sleep(200);
            Mouse.Click();
            Thread.Sleep(1000);

            _mainWindow = _application.GetMainWindow(_automation);
            Thread.Sleep(500);

            var registerButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnRegister"));
            Assert.IsNotNull(registerButton, "Registration form was not opened.");
        }

        [When("I enter valid registration data")]
        public void WhenIEnterValidRegistrationData()
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 6);

            var nameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtNameRegister"))?.AsTextBox();
            var surnameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtSurnameRegister"))?.AsTextBox();
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameRegister"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordRegister"))?.AsTextBox();
            var emailTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtEmailRegister"))?.AsTextBox();
            var phoneTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPhoneRegister"))?.AsTextBox();

            Assert.IsNotNull(nameTextBox, "Name textbox was not found.");
            Assert.IsNotNull(surnameTextBox, "Surname textbox was not found.");
            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(emailTextBox, "Email textbox was not found.");
            Assert.IsNotNull(phoneTextBox, "Phone textbox was not found.");

            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";

            nameTextBox.Enter("Test");
            surnameTextBox.Enter("User");
            usernameTextBox.Enter("test" + unique);
            passwordTextBox.Enter("Test123!");
            emailTextBox.Enter("test" + unique + "@mail.com");
            var phone = "09" + Guid.NewGuid().ToString("N").Substring(0, 8);
            phoneTextBox.Enter(phone);

            Thread.Sleep(1000);
        }

        [When("I click the register button")]
        public void WhenIClickTheRegisterButton()
        {
            var registerButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnRegister"));
            Assert.IsNotNull(registerButton, "Register button was not found.");
            registerButton.Click();
            Thread.Sleep(1000);
            _mainWindow = _application.GetMainWindow(_automation);
        }

        [Then("the registration should be successful")]
        public void ThenTheRegistrationShouldBeSuccessful()
        {
            Thread.Sleep(1000);
            var desktop = _automation.GetDesktop();
            var messageBox = desktop.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(messageBox, "Success popup was not found.");

            Thread.Sleep(500);
            var okButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
            Assert.IsNotNull(okButton, "OK button was not found on popup.");
            okButton.Invoke();
            Thread.Sleep(1000);
            _mainWindow = _application.GetMainWindow(_automation);

            var usernameLogin = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"));
            var passwordLogin = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"));
            var loginButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"));

            Assert.IsNotNull(usernameLogin, "Username login textbox was not found after registration.");
            Assert.IsNotNull(passwordLogin, "Password login textbox was not found after registration.");
            Assert.IsNotNull(loginButton, "Login button was not found after registration.");
        }
        [When("I enter an existing username")]
        public void WhenIEnterAnExistingUsername()
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 6);
            var phone = "09" + Guid.NewGuid().ToString("N").Substring(0, 8);

            var nameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtNameRegister"))?.AsTextBox();
            var surnameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtSurnameRegister"))?.AsTextBox();
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameRegister"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordRegister"))?.AsTextBox();
            var emailTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtEmailRegister"))?.AsTextBox();
            var phoneTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPhoneRegister"))?.AsTextBox();

            Assert.IsNotNull(nameTextBox, "Name textbox was not found.");
            Assert.IsNotNull(surnameTextBox, "Surname textbox was not found.");
            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(emailTextBox, "Email textbox was not found.");
            Assert.IsNotNull(phoneTextBox, "Phone textbox was not found.");

            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";

            nameTextBox.Enter("Test");
            surnameTextBox.Enter("ExistingUser");
            usernameTextBox.Enter("mmarkic");
            passwordTextBox.Enter("Test123!");
            emailTextBox.Enter("existing" + unique + "@mail.com");
            phoneTextBox.Enter(phone);

            Thread.Sleep(1000);
        }

        [Then("an error message should be displayed")]
        public void ThenAnErrorMessageShouldBeDisplayed()
        {
            Thread.Sleep(1500);

            var desktop = _automation.GetDesktop();
            var messageBox = desktop.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(messageBox, "Error popup was not found.");

            var okButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
            Assert.IsNotNull(okButton, "OK button was not found on error popup.");

            okButton.Invoke();
            Thread.Sleep(1000);
        }

        [When("I enter an existing email")]
        public void WhenIEnterAnExistingEmail()
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 6);
            var phone = "09" + Guid.NewGuid().ToString("N").Substring(0, 8);

            var nameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtNameRegister"))?.AsTextBox();
            var surnameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtSurnameRegister"))?.AsTextBox();
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameRegister"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordRegister"))?.AsTextBox();
            var emailTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtEmailRegister"))?.AsTextBox();
            var phoneTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPhoneRegister"))?.AsTextBox();

            Assert.IsNotNull(nameTextBox, "Name textbox was not found.");
            Assert.IsNotNull(surnameTextBox, "Surname textbox was not found.");
            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(emailTextBox, "Email textbox was not found.");
            Assert.IsNotNull(phoneTextBox, "Phone textbox was not found.");

            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";

            nameTextBox.Enter("Test");
            surnameTextBox.Enter("ExistingEmail");
            usernameTextBox.Enter("user" + unique);
            passwordTextBox.Enter("Test123!");
            emailTextBox.Enter("mmarkic@foi.hr");
            phoneTextBox.Enter(phone);

            Thread.Sleep(1000);
        }

        [When("I leave required fields empty")]
        public void WhenILeaveRequiredFieldsEmpty()
        {
            var nameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtNameRegister"))?.AsTextBox();
            var surnameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtSurnameRegister"))?.AsTextBox();
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameRegister"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordRegister"))?.AsTextBox();
            var emailTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtEmailRegister"))?.AsTextBox();
            var phoneTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPhoneRegister"))?.AsTextBox();

            Assert.IsNotNull(nameTextBox, "Name textbox was not found.");
            Assert.IsNotNull(surnameTextBox, "Surname textbox was not found.");
            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(emailTextBox, "Email textbox was not found.");
            Assert.IsNotNull(phoneTextBox, "Phone textbox was not found.");

            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";

            Thread.Sleep(500);
        }

        [Then("a validation error message should be displayed")]
        public void ThenAValidationErrorMessageShouldBeDisplayed()
        {
            Thread.Sleep(1500);

            var desktop = _automation.GetDesktop();
            var messageBox = desktop.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(messageBox, "Validation popup was not found.");

            var okButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
            Assert.IsNotNull(okButton, "OK button was not found on validation popup.");

            okButton.Invoke();
            Thread.Sleep(1000);
        }

        [When("I enter an invalid email format")]
        public void WhenIEnterAnInvalidEmailFormat()
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 6);
            var phone = "09" + Guid.NewGuid().ToString("N").Substring(0, 8);

            var nameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtNameRegister"))?.AsTextBox();
            var surnameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtSurnameRegister"))?.AsTextBox();
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameRegister"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordRegister"))?.AsTextBox();
            var emailTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtEmailRegister"))?.AsTextBox();
            var phoneTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPhoneRegister"))?.AsTextBox();

            Assert.IsNotNull(nameTextBox, "Name textbox was not found.");
            Assert.IsNotNull(surnameTextBox, "Surname textbox was not found.");
            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(emailTextBox, "Email textbox was not found.");
            Assert.IsNotNull(phoneTextBox, "Phone textbox was not found.");

            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";

            nameTextBox.Enter("Test");
            surnameTextBox.Enter("InvalidEmail");
            usernameTextBox.Enter("user" + unique);
            passwordTextBox.Enter("Test123!");
            emailTextBox.Enter("invalid-email-format");
            phoneTextBox.Enter(phone);

            Thread.Sleep(1000);
        }

        [When("I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"))?.AsTextBox();

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");

            usernameTextBox.Focus();
            Thread.Sleep(200);
            usernameTextBox.Text = "mmarkic";

            passwordTextBox.Focus();
            Thread.Sleep(200);
            passwordTextBox.Text = "Marko123";

            Thread.Sleep(500);
        }

        [When("I enter a valid username and wrong password")]
        public void WhenIEnterAValidUsernameAndWrongPassword()
        {
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"))?.AsTextBox();

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");

            usernameTextBox.Focus();
            Thread.Sleep(200);
            usernameTextBox.Text = "mmarkic";

            passwordTextBox.Focus();
            Thread.Sleep(200);
            passwordTextBox.Text = "KrivaLozinka123";

            Thread.Sleep(500);
        }

        [When("I enter a wrong username and valid password")]
        public void WhenIEnterAWrongUsernameAndValidPassword()
        {
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"))?.AsTextBox();

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");

            usernameTextBox.Focus();
            Thread.Sleep(200);
            usernameTextBox.Text = "randomUser123";

            passwordTextBox.Focus();
            Thread.Sleep(200);
            passwordTextBox.Text = "Marko123";

            Thread.Sleep(500);
        }

        [When("I leave username and password empty")]
        public void WhenILeaveUsernameAndPasswordEmpty()
        {
            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"))?.AsTextBox();

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");

            usernameTextBox.Focus();
            Thread.Sleep(200);
            usernameTextBox.Text = "";

            passwordTextBox.Focus();
            Thread.Sleep(200);
            passwordTextBox.Text = "";

            Thread.Sleep(300);
        }

        [When("I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            var loginButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"));
            Assert.IsNotNull(loginButton, "Login button was not found.");

            loginButton.Focus();
            Thread.Sleep(200);
            loginButton.Click();

            Thread.Sleep(1000);
        }

        [Given("I am on the login form")]
        public void GivenIAmOnTheLoginForm()
        {
            _mainWindow = _application.GetMainWindow(_automation);

            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"));
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"));
            var loginButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"));

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(loginButton, "Login button was not found.");
        }

        [Then("I should see the login form")]
        public void ThenIShouldSeeTheLoginForm()
        {
            _mainWindow = _application.GetMainWindow(_automation);

            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"));
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"));
            var loginButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"));

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(loginButton, "Login button was not found.");
        }

        [Then("I should be logged into the application")]
        public void ThenIShouldBeLoggedIntoTheApplication()
        {
            Thread.Sleep(1000);

            var logoutButton = _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId("btnLogout"));

            Assert.IsNotNull(logoutButton, "Logout button was not found after successful login.");
        }

        [Given("I am logged in")]
        public void GivenIAmLoggedIntoTheApplication()
        {
            PerformValidLogin();
        }

        [When("I close the application")]
        public void WhenICloseTheApplication()
        {
            _mainWindow = _application.GetMainWindow(_automation);
            Assert.IsNotNull(_mainWindow, "Main window not found.");

            _mainWindow.Close();            
            Thread.Sleep(1000);             
            if (!_application.HasExited)
            {
                Assert.Fail("Application did not close normally (X), cannot test session persistence.");
            }
            _automation?.Dispose();
            _application = Application.Launch(GetAppPath());
            _application.WaitWhileMainHandleIsMissing();
            _automation = new UIA3Automation();
            _mainWindow = _application.GetMainWindow(_automation);
            Thread.Sleep(1500);
        }

        [Then("I should still be logged in")]
        public void ThenIShouldStillBeLoggedIn()
        {
            var logoutButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnLogout"));
            Assert.IsNotNull(logoutButton, "User is not logged in after restarting the application.");
        }

        [When("I click the logout button")]
        public void WhenIClickTheLogoutButton()
        {
            var logoutButton = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId("btnLogout"));

            Assert.IsNotNull(logoutButton, "Logout button was not found.");

            logoutButton.Click();
            Thread.Sleep(1000);

            _mainWindow = _application.GetMainWindow(_automation);
        }

        [Then("I should be redirected to the login form")]
        public void ThenIShouldBeRedirectedToTheLoginForm()
        {
            _mainWindow = _application.GetMainWindow(_automation);

            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"));
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"));
            var loginButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"));

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found after logout.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found after logout.");
            Assert.IsNotNull(loginButton, "Login button was not found after logout.");
        }

        [Given("I log out from the application")]
        public void GivenILogOutFromTheApplication()
        {
            PerformValidLogin();
            WhenIClickTheLogoutButton();
        }

        private void PerformValidLogin()
        {
            _mainWindow = _application.GetMainWindow(_automation);

            var usernameTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtUsernameLogin"))?.AsTextBox();
            var passwordTextBox = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtPasswordLogin"))?.AsTextBox();
            var loginButton = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"));

            Assert.IsNotNull(usernameTextBox, "Username textbox was not found.");
            Assert.IsNotNull(passwordTextBox, "Password textbox was not found.");
            Assert.IsNotNull(loginButton, "Login button was not found.");

            usernameTextBox.Focus();
            Thread.Sleep(200);
            usernameTextBox.Text = "mmarkic";

            passwordTextBox.Focus();
            Thread.Sleep(200);
            passwordTextBox.Text = "Marko123";

            Thread.Sleep(300);

            loginButton.Focus();
            Thread.Sleep(200);
            loginButton.Click();

            Thread.Sleep(1000);

            var logoutButton = _automation.GetDesktop()
                .FindFirstDescendant(cf => cf.ByAutomationId("btnLogout"));

            Assert.IsNotNull(logoutButton, "Logout button was not found after login.");
        }
    }
}