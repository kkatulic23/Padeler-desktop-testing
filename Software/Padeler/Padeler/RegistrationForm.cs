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
        public RegistrationForm()
        {
            InitializeComponent();
            FillCombos();
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            pbPictureRegister.BorderStyle = BorderStyle.FixedSingle;

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
                var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                var tmp = Image.FromStream(fs);
                pbPictureRegister.Image?.Dispose();
                pbPictureRegister.Image = new Bitmap(tmp);
            }
            catch (Exception ex)
            {
                _profileImagePngBytes = null;
                MessageBox.Show(ex.Message, "Image error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
