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

namespace Padeler
{
    public partial class FilterForm : Form
    {
        public FilterForm() // Karlo Kršak
        {
            InitializeComponent();
            trkRadius.Value = Properties.Settings.Default.RadiusKm;
            lblRadius.Text = $"{trkRadius.Value} km";
            trkRadius.ValueChanged += (s, e) => lblRadius.Text = $"{trkRadius.Value} km";

            var combo = new ComboFiller();

            FillComboWithAny(cboGender, combo.GetGenders());
            FillComboWithAny(cboLevel, combo.GetLevelsOfPlay());
            FillComboWithAny(cboPosition, combo.GetPositions());
            FillComboWithAny(cboFrequency, combo.GetFrequenciesOfPlay());

            RestoreSelection(cboGender, Properties.Settings.Default.FilterGender);
            RestoreSelection(cboLevel, Properties.Settings.Default.FilterLevel);
            RestoreSelection(cboPosition, Properties.Settings.Default.FilterPosition);
            RestoreSelection(cboFrequency, Properties.Settings.Default.FilterFrequency);
        }

        private void btnApply_Click(object sender, EventArgs e) // Karlo Kršak
        {
            Properties.Settings.Default.RadiusKm = trkRadius.Value;
            Properties.Settings.Default.FilterGender = SelectedOrEmpty(cboGender);
            Properties.Settings.Default.FilterLevel = SelectedOrEmpty(cboLevel);
            Properties.Settings.Default.FilterPosition = SelectedOrEmpty(cboPosition);
            Properties.Settings.Default.FilterFrequency = SelectedOrEmpty(cboFrequency);
            Properties.Settings.Default.Save();
            MessageBox.Show(
                "Filters have been successfully applied.",
                "Filters applied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        private void FIlterForm_Load(object sender, EventArgs e)
        {
            
        }

        private void trkRadius_ValueChanged(object sender, EventArgs e) // Karlo Kršak
        {
            lblRadius.Text = $"{trkRadius.Value} km";
        }

        private void FilterForm_Load_1(object sender, EventArgs e)
        {

        }
        private void FillComboWithAny(ComboBox cbo, IEnumerable<string> values) // Karlo Kršak
        {
            cbo.Items.Clear();
            cbo.Items.Add("Any");               
            foreach (var v in values) cbo.Items.Add(v);
            cbo.SelectedIndex = 0;              
        }

        /// <summary>
        /// Vraća odabranu vrijednost iz ComboBoxa ili prazan string
        /// ako je odabrana opcija "Any" ili nema odabira.
        /// </summary>
        private static string SelectedOrEmpty(ComboBox cbo) // Karlo Kršak
        {
            var s = cbo.SelectedItem != null ? cbo.SelectedItem.ToString() : "";
            return s == "Any" ? "" : s;         
        }

        /// <summary>
        /// Vraća spremljeni odabir u ComboBox ako postoji,
        /// u suprotnom postavlja zadanu opciju "Any".
        /// </summary>
        private void RestoreSelection(ComboBox cbo, string saved) // Karlo Kršak
        {
            if (saved == null) saved = "";
            if (saved != "" && cbo.Items.Contains(saved)) 
                cbo.SelectedItem = saved;
            else
                cbo.SelectedIndex = 0; 
        }

    }
}
