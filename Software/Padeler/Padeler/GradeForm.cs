using BLL;
using Padeler.Design;
using System;
using System.Windows.Forms;

namespace Padeler
{
    public partial class GradeForm : Form // Kristian Katulić
    {
        private readonly int _commentedId;
        private readonly int _commenterId;
        private readonly CommentsService _service = new CommentsService();
        public GradeForm(int commentedId)
        {
            InitializeComponent();
            _commentedId = commentedId;
            _commenterId = AuthContext.CurrentUserId;

            btnSend.BackColor = AppColors.NavButtonBackground;
            btnSend.ForeColor = AppColors.TextOnPrimary;
            UpdateRemainingLabel();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                btnSend.Enabled = false;

                int grade = (int)nudGrade.Value;
                string comment = rtbComment.Text.Trim();

                await _service.AddRatingAsync(_commentedId, _commenterId, grade, comment);

                MessageBox.Show($"Grade saved!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                Close();
            }       
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }

        private void rtbComment_TextChanged(object sender, EventArgs e)
        {
            UpdateRemainingLabel();
        }

        private void UpdateRemainingLabel()
        {
            int remaining = _service.GetRemainingCommentCharacters(rtbComment.Text);
            if (_service.IsCommentTooLong(rtbComment.Text))
            {
                lblRemaining.Text = $"Prekoračeno za {Math.Abs(remaining)} znakova";
            }
            else
            {
                lblRemaining.Text = $"Preostalo znakova: {remaining}";
            }
        }
    }
}
