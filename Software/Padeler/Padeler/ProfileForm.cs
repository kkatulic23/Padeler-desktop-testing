using BLL;
using EL;
using Padeler.Design;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Padeler
{
    public partial class ProfileForm : Form // Kristian Katulić
    {
        private readonly ComboFiller _comboFiller = new ComboFiller();
        private readonly BLL.ImageConverter _imageConverter = new BLL.ImageConverter();
        private readonly EditProfile _logic = new EditProfile();
        private readonly Validator _validator = new Validator();
        public ProfileForm() // Karlo Kršak
        {
            InitializeComponent();
            dtpDate.MaxDate = DateTime.Now;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd.MMM.yyyy.";
            cboGender.DataSource = _comboFiller.GetGenders();
            cboFrequency.DataSource = _comboFiller.GetFrequenciesOfPlay();
            cboLevel.DataSource = _comboFiller.GetLevelsOfPlay();
            cboPosition.DataSource = _comboFiller.GetPositions();
            this.BackColor = AppColors.ContentBackground;
            pbImage.BorderStyle = BorderStyle.FixedSingle;
        }

        private async void ProfileForm_Load(object sender, EventArgs e) // Karlo Kršak
        {
            UserDto user = await _logic.GetUserDataAsync(AuthContext.CurrentUserId);
            txtName.Text = user.name;
            txtSurname.Text = user.surname;
            txtUsername.Text = user.username;
            txtEmail.Text = user.email;
            txtPhone.Text = user.phone;
            cboGender.SelectedItem = user.gender;
            dtpDate.Value = user.dateOfBirth;
            cboFrequency.SelectedItem = user.frequencyOfPlay;
            cboLevel.SelectedItem = user.levelOfPlay;
            cboPosition.SelectedItem = user.position;
            if (user.image != null && user.image.Length > 0)
                pbImage.Image = _imageConverter.BytesToImage(user.image);
        }

        private async void btnSave_Click(object sender, EventArgs e) // Karlo Kršak
        {
            UpdateUserRequest user = new UpdateUserRequest
            {
                userId = AuthContext.CurrentUserId,
                name = txtName.Text,
                surname = txtSurname.Text,
                username = txtUsername.Text,
                email = txtEmail.Text,
                phone = txtPhone.Text,
                gender = cboGender.SelectedItem.ToString(),
                dateOfBirth = dtpDate.Value,
                frequency = cboFrequency.SelectedItem.ToString(),
                level = cboLevel.SelectedItem.ToString(),
                position = cboPosition.SelectedItem.ToString()
            };
            var errors = _validator.ValidateUser(user);
            var bytes = _imageConverter.ImageToBytes(pbImage.Image);
            if(bytes.Length > 600000)
            {
                errors.Add("The selected image is too large. Please choose an image smaller than 600KB.");
            }
            if (pbImage.Image != null) {
                user.imageBase64 = bytes;
                user.mimeType = "image/jpeg";
            }
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                await _logic.UpdateUserDataAsync(user);
                MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnChangeImage_Click(object sender, EventArgs e) // Karlo Kršak
        {
            var ofd = new OpenFileDialog
            {
                Title = "Choose image",
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _imageConverter.LoadImage(ofd.FileName, pbImage);
            }
        }
    }
}
