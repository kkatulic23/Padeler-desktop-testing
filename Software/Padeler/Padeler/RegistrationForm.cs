using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using System.Diagnostics;

namespace Padeler
{
    public partial class RegistrationForm : Form
    {
        private byte[] _profileImagePngBytes = null;
        private readonly PasswordStrengthService _passwordStrengthService = new PasswordStrengthService();
        private ProgressBar _passwordStrengthProgressBar;
        private Label _passwordStrengthLabel;
        public RegistrationForm()
        {
            InitializeComponent();
            FillCombos();
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            pbPictureRegister.BorderStyle = BorderStyle.FixedSingle;

            InitializePasswordStrengthControls();
            UpdatePasswordStrength();
            txtPasswordRegister.TextChanged += txtPasswordRegister_TextChanged;

            this.KeyPreview = true;
            this.KeyDown += Register_KeyDown;
        }

        private void Register_KeyDown(object sender, KeyEventArgs e)
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



        private void FillCombos() // Karlo Kršak
        {
            var filler = new ComboFiller();
            cboFrequencyRegister.DataSource = filler.GetFrequenciesOfPlay();
            cboLevelRegister.DataSource = filler.GetLevelsOfPlay();
            cboPositionRegister.DataSource = filler.GetPositions();
            cboGenderRegister.DataSource = filler.GetGenders();
        }

        private async void btnRegister_Click(object sender, EventArgs e) // Karlo Kršak
        {
            try
            {
                btnRegister.Enabled = false;
                var authService = new AuthService();
                int userId = await authService.RegisterAsync(
                    txtUsernameRegister.Text,
                    txtEmailRegister.Text,
                    txtPasswordRegister.Text,
                    txtPhoneRegister.Text,
                    txtNameRegister.Text,
                    txtSurnameRegister.Text,
                    cboGenderRegister.SelectedItem?.ToString(),
                    dtpDobRegister.Value.Date,
                    cboFrequencyRegister.SelectedItem?.ToString(),
                    cboLevelRegister.SelectedItem?.ToString(),
                    cboPositionRegister.SelectedItem?.ToString(),
                    _profileImagePngBytes
                );
                MessageBox.Show($"Registration successful!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Registration error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnRegister.Enabled = true;
            }
        }

        /// <summary>
        /// Omogućuje korisniku odabir slike profila s diska, učitava ju,
        /// konvertira u PNG format za pohranu te prikazuje pregled slike
        /// u sučelju aplikacije.
        /// </summary>
        private void btnLoadImage_Click(object sender, EventArgs e) // Karlo Kršak
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select profile image",
                Filter = "Images|*.jpg;*.jpeg;*.png;*.webp;*.bmp",
                Multiselect = false
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var authService = new AuthService();
                _profileImagePngBytes = authService.LoadProfileImageAsPngBytes(ofd.FileName);

                using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                using (var tmp = Image.FromStream(fs))
                {
                    pbPictureRegister.Image?.Dispose();
                    pbPictureRegister.Image = new Bitmap(tmp);
                }
            }
            catch (Exception ex)
            {
                _profileImagePngBytes = null;
                MessageBox.Show(ex.Message, "Image error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializePasswordStrengthControls()
        {
            _passwordStrengthProgressBar = new ProgressBar
            {
                Minimum = 0,
                Maximum = 5,
                Value = 0,
                Location = new Point(668, 160),
                Size = new Size(90, 20)
            };

            _passwordStrengthLabel = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(668, 183),
                Text = "Very weak"
            };

            Controls.Add(_passwordStrengthProgressBar);
            Controls.Add(_passwordStrengthLabel);
        }

        private void txtPasswordRegister_TextChanged(object sender, EventArgs e)
        {
            UpdatePasswordStrength();
        }

        private void UpdatePasswordStrength()
        {
            int score = _passwordStrengthService.CalculateScore(txtPasswordRegister.Text);
            string label = _passwordStrengthService.GetStrengthLabel(txtPasswordRegister.Text);

            _passwordStrengthProgressBar.Value = score;
            _passwordStrengthLabel.Text = label;
        }
    }
}
