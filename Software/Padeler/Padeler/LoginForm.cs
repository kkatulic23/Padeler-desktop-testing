using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using System.Diagnostics;
using System.IO;
using System.Text.Json;


namespace Padeler
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;

            this.KeyPreview = true;
            this.KeyDown += Login_KeyDown;
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                OpenUserManual();
                e.Handled = true;
            }
        }

        private void OpenUserManual()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PADELER_UserManual.pdf");

            if (!File.Exists(path))
            {
                MessageBox.Show(
                    "User manual not found.",
                    "Help",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }


        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
        private void llRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) // Karlo Kršak
        {
            using (var rf = new RegistrationForm())
            {
                this.Hide();
                rf.ShowDialog();   
                this.Show();
                this.Activate();
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e) // Karlo Kršak
        {
            try
            {
                var authService = new AuthService();
                int userId = await authService.LoginAsync(
                    txtUsernameLogin.Text,
                    txtPasswordLogin.Text
                );
                var baseForm = new BaseForm();
                baseForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                string msg = ExtractErrorMessage(ex.Message);

                if (msg.IndexOf("Invalid credentials", StringComparison.OrdinalIgnoreCase) >= 0)
                    msg = "Invalid username or password.";

                if (msg.Equals("Email not verified", StringComparison.OrdinalIgnoreCase))
                    msg = "Account not activated. Chechk your email (spam folder) and activate your account.";

                MessageBox.Show(msg, "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e) // Karlo Kršak
        {
            if (e.CloseReason == CloseReason.UserClosing) Application.Exit();
        }

        private void LoginForm_Shown(object sender, EventArgs e) // Karlo Kršak
        {
            int rememberedId = Properties.Settings.Default.RememberUserId;
            string rememberedUsername = Properties.Settings.Default.RememberUsername;
            if (rememberedId > 0)
            {
                BLL.AuthContext.SetUser(rememberedId, rememberedUsername);
                var baseForm = new BaseForm();
                baseForm.Show();
                this.Hide();
            }
        }

        private static string ExtractErrorMessage(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "There was an error.";
            try
            {
                var doc = JsonDocument.Parse(raw);
                if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                    doc.RootElement.TryGetProperty("error", out var err))
                {
                    var msg = err.GetString();
                    if (!string.IsNullOrWhiteSpace(msg))
                        return msg;
                }
            }
            catch
            {
            }
            return raw;
        }

    }
}
