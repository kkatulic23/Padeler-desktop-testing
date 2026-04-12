using BLL;
using EL;
using Padeler.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Padeler
{
    public partial class MatchForm : Form
    {
        private readonly MatchService _matchService;

        private BindingList<MatchRow> _rows = new BindingList<MatchRow>();

        private const string ColFullName = "colFullName";
        private const string ColNickname = "colNickname";
        private const string ColPhone = "colPhone";
        private const string ColWhatsapp = "colWhatsapp";
        private const string ColGrade = "colGrade";
        public MatchForm()
        {
            InitializeComponent();
            _matchService = new MatchService();

            dgvMatches.AutoGenerateColumns = false;
            dgvMatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMatches.MultiSelect = false;
            dgvMatches.AllowUserToAddRows = false;

            btnDelete.BackColor = AppColors.TopBarBackground;
            btnDelete.ForeColor = AppColors.TextOnPrimary;

            btnSave.BackColor = AppColors.NavButtonBackground;
            btnSave.ForeColor = AppColors.TextOnPrimary;
        }

        private void dgvMatches_CellContentClick(object sender, DataGridViewCellEventArgs e) // Filip Grgac
        {
            if (e.RowIndex < 0) return;

            if (dgvMatches.Columns[e.ColumnIndex].Name == ColWhatsapp)
            {
                var row = dgvMatches.Rows[e.RowIndex].DataBoundItem as MatchRow;
                if (row == null) return;

                OpenWhatsApp(row.Phone);
            }

            if (dgvMatches.Columns[e.ColumnIndex].Name == ColGrade)
            {
                var row = dgvMatches.Rows[e.RowIndex].DataBoundItem as MatchRow;
                if (row == null) return;

                new GradeForm(row.OtherUserId).Show();
            }
        }

        private void OpenWhatsApp(string phone) // Filip Grgac
        {
            if (String.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Korisnik nema broj telefona.");
                return;
            }

            var digits = Regex.Replace(phone, "[^0-9]", "");

            if(digits.Length < 9)
            {
                MessageBox.Show("Neispravan broj telefona.");
                return;
            }

            var url = "https://wa.me/" + digits;

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nemogu otvoriti WhatsApp: " +  ex.Message);
            }
        }

        private async void MatchForm_Load(object sender, EventArgs e) // Filip Grgac
        {
            if (!AuthContext.IsLoggedIn)
            {
                MessageBox.Show("Niste prijavljeni");
                Close();
                return;
            }

            BuildDGV();

            await ReloadAsync();
        }

        private async Task ReloadAsync() // Filip Grgac
        {
            try
            {
                var list = await _matchService.GetMatchedEntries(AuthContext.CurrentUserId);

                _rows = new BindingList<MatchRow>(list);
                dgvMatches.DataSource = _rows;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Greška kod dohvaćanja Matcheva: " + ex.Message);
            }
        }

        private void BuildDGV() // Filip Grgac
        {
            dgvMatches.Columns.Clear();

            var fullNamceCol = new DataGridViewTextBoxColumn
            {
                Name = ColFullName,
                HeaderText = "Player",
                DataPropertyName = nameof(MatchRow.FullName),
                ReadOnly = true
            };

            var nickCol = new DataGridViewTextBoxColumn
            {
                Name = ColNickname,
                HeaderText = "Nickname",
                DataPropertyName = nameof(MatchRow.Nickname),
                ReadOnly = false
            };

            var phoneCol = new DataGridViewTextBoxColumn
            {
                Name = ColPhone,
                HeaderText = "Phone",
                DataPropertyName = nameof(MatchRow.Phone),
                ReadOnly = true
            };

            var waCol = new DataGridViewImageColumn
            {
                Name = ColWhatsapp,
                HeaderText = "",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 15
            };

            var grCol = new DataGridViewImageColumn // Kristian Katulić
            {
                Name = ColGrade,
                HeaderText = "",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 15
            };

            grCol.Image = Properties.Resources.star;
            waCol.Image = Properties.Resources.logo;

            dgvMatches.Columns.Add(fullNamceCol);
            dgvMatches.Columns.Add(nickCol);
            dgvMatches.Columns.Add(phoneCol);
            dgvMatches.Columns.Add(waCol);
            dgvMatches.Columns.Add(grCol);
        }

        private async void btnSave_Click(object sender, EventArgs e) // Filip Grgac
        {
            var row = SelectedRow();
            if (row == null)
            {
                MessageBox.Show("Odaberi red u tablici.");
                return;
            }

            try
            {
                var entry = new MatchEntryDto
                {
                    CurrentUserId = AuthContext.CurrentUserId,
                    MatchedUserId = row.OtherUserId,
                    CustomNickname = String.IsNullOrWhiteSpace(row.Nickname) ? null : row.Nickname.Trim(),
                    IsHidden = false
                };
                await _matchService.UpdateEntry(entry);
                MessageBox.Show("Nickname uspješno promjenjen");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Greška kod spremanja: " + ex.Message);
            }
        }

        private MatchRow SelectedRow() // Filip Grgac
        {
            if(dgvMatches.CurrentRow == null)
            {
                return null;
            }

            return dgvMatches.CurrentRow.DataBoundItem as MatchRow;
        }

        private async void btnDelete_Click(object sender, EventArgs e) // Filip Grgac
        {
            var row = SelectedRow();
            if (row == null)
            {
                MessageBox.Show("Odaberi red u tablici.");
                return;
            }

            var confirm = MessageBox.Show($"Želite li maknuti korisnika '{row.FullName}' iz liste?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.No) return;

            try
            {
                var entry = new MatchEntryDto
                {
                    CurrentUserId = AuthContext.CurrentUserId,
                    MatchedUserId = row.OtherUserId
                };
                await _matchService.DeleteEntry(entry);

                _rows.Remove(row);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Greška prilikom brisanja: " + ex.Message);
            }
        }
    }
}
