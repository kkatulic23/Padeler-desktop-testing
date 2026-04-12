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
using Padeler.Design;
using System.Diagnostics;
using System.IO;

namespace Padeler
{
    public partial class BaseForm : Form
    {
        private Button _activeNavButton;
        private readonly LocationService _locationService = new LocationService();
        private Form _currentChild;

        public BaseForm()
        {
            InitializeComponent();
            ApplyTemplateColors();

            this.KeyPreview = true;
            this.KeyDown += BaseForm_KeyDown;

            lblUsername.Text = BLL.AuthContext.CurrentUsername;
            SetActiveNav(NavigationPage.Home);
        }

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
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

        private void ApplyTemplateColors()
        {
            pnlTopBar.BackColor = AppColors.TopBarBackground;
            lblAppTitle.ForeColor = AppColors.TextOnPrimary;
            lblUsername.ForeColor = AppColors.TextOnPrimary;

            btnLogout.BackColor = AppColors.NavButtonBackground;
            btnLogout.ForeColor = AppColors.TextOnPrimary;

            pnlSidebar.BackColor = AppColors.SidebarBackground;

            StyleNavButtons(btnHome);
            StyleNavButtons(btnMatch);
            StyleNavButtons(btnProfile);
            StyleNavButtons(btnFilter);

            pnlContent.BackColor = AppColors.ContentBackground;
        }

        private void StyleNavButtons(Button btn)
        {
            btn.BackColor = AppColors.NavButtonBackground;
            btn.ForeColor = AppColors.TextOnPrimary;
            btn.MouseEnter += (s, e) =>
            {
                if (btn != _activeNavButton)
                {
                    btn.BackColor = AppColors.NavButtonHover;
                }
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn != _activeNavButton)
                {
                    btn.BackColor = AppColors.NavButtonBackground;
                }
            };
        }

        private async void Form1_Load(object sender, EventArgs e) // Karlo Kršak
        {
            var home = new HomeForm();
            ShowChildForm(home);

            _ = UpdateLocationAndRefreshAsync(home);
            var presenter = new WinFormsNotificationPresenter(this);
            var notificationService = new NotificationService(presenter);

            await notificationService.CheckAndShowNotificationAsync(AuthContext.CurrentUserId);
        }
           


            

        private void btnHome_Click(object sender, EventArgs e)
        {
            SetActiveNav(NavigationPage.Home);
            ShowChildForm(new HomeForm());
        }

        /// <summary>
        /// Postavlja aktivnu navigacijsku stavku sučelja ovisno o odabranoj stranici.
        /// Metoda vizualno označava aktivni gumb promjenom boje pozadine
        /// i stila fonta, dok prethodno aktivni gumb vraća u početno stanje.
        /// </summary>
        protected void SetActiveNav(NavigationPage page)
        {
            if(_activeNavButton != null)
            {
                _activeNavButton.BackColor = AppColors.NavButtonBackground;
                _activeNavButton.Font = new Font(_activeNavButton.Font, FontStyle.Regular);
            }

            switch (page)
            {
                case NavigationPage.Home:
                    _activeNavButton = btnHome;
                    break;
                case NavigationPage.Match:
                    _activeNavButton = btnMatch;
                    break;
                case NavigationPage.Profile:
                    _activeNavButton = btnProfile;
                    break;
                case NavigationPage.Filter:
                    _activeNavButton = btnFilter;
                    break;
            }
            
            _activeNavButton.BackColor= AppColors.NavButtonHover;
            _activeNavButton.Font = new Font(_activeNavButton.Font, FontStyle.Bold);
        }

        private void ShowChildForm(Form form)
        {
            if (_currentChild != null)
            {
                try { _currentChild.Close(); } catch { }
                _currentChild = null;
            }
            _currentChild = form;

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            SetActiveNav(NavigationPage.Match);
            ShowChildForm(new MatchForm());
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            SetActiveNav(NavigationPage.Profile);
            ShowChildForm(new ProfileForm());
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            SetActiveNav(NavigationPage.Filter);
            ShowChildForm(new FilterForm());
        }

        private bool _logoutRequested = false;

        /// <summary>
        /// Odjavljuje trenutno prijavljenog korisnika, briše spremljene postavke
        /// (remember user i filtere), vraća aplikaciju na LoginForm
        /// te zatvara glavnu (Base) formu.
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e) // Karlo Kršak
        {
            _logoutRequested = true;
            var auth = new BLL.AuthService();
            auth.Logout();

            Properties.Settings.Default.RememberUserId = 0;
            Properties.Settings.Default.RadiusKm = 50;
            Properties.Settings.Default.FilterGender = "";
            Properties.Settings.Default.FilterLevel = "";
            Properties.Settings.Default.FilterPosition = "";
            Properties.Settings.Default.FilterFrequency = "";
            Properties.Settings.Default.Save();

            var lf = new LoginForm();
            lf.Show();

            this.Close();
        }
        /// <summary>
        /// Obraduje zatvaranje glavne forme aplikacije.
        /// Ako korisnik zatvara aplikaciju bez eksplicitnog logouta,
        /// sprema podatke o zadnjem prijavljenom korisniku (remember user)
        /// te sigurno gasi aplikaciju.
        /// </summary>
        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e) // Karlo Kršak
        {
            if (e.CloseReason == CloseReason.UserClosing && !_logoutRequested)
            {
                if (BLL.AuthContext.IsLoggedIn)
                {
                    Properties.Settings.Default.RememberUserId = BLL.AuthContext.CurrentUserId;
                    Properties.Settings.Default.RememberUsername = BLL.AuthContext.CurrentUsername;
                }
                else
                {
                    Properties.Settings.Default.RememberUserId = 0;
                    Properties.Settings.Default.RememberUsername = "";
                }
                Properties.Settings.Default.Save();
                Application.Exit();
            }
        }

        /// <summary>
        /// Asinkrono dohvaća i sprema trenutnu lokaciju korisnika u pozadini
        /// te osvježava HomeForm ako je ona još uvijek aktivna i prikazana,
        /// bez promjene trenutne navigacije korisnika.
        /// </summary>
        private async Task UpdateLocationAndRefreshAsync(HomeForm homeAtStart) // Karlo Kršak
        {
            bool ok = false;
            try
            {
                ok = await _locationService.TryUpdateCurrentUserLocationAsync();
            }
            catch { }
            if (!ok) return;
            if (_currentChild == homeAtStart && !homeAtStart.IsDisposed && homeAtStart.IsHandleCreated)
            {
                homeAtStart.BeginInvoke(new Action(async () =>
                {
                    if (_currentChild != homeAtStart) return;
                    await homeAtStart.ReloadUsersAsync();
                }));
            }
        }
    }
}
