using BLL;
using Padeler.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EL;

namespace Padeler
{
    public partial class HomeForm : Form
    {
        private bool isFrontVisible = true;
        private bool isShrinking = true;
        private int cardOriginalWidth;
        private int cardOriginalX;

        private readonly UserService _userService;
        private readonly MatchService _matchService;
        private readonly ImageService _imageService;
        private readonly NotificationService _notificationService;
        private readonly BadgeService _badgeService;
        private List<UserCardDto> _cards;
        private int _currentIndex = 0;
        public HomeForm()
        {
            InitializeComponent();
            _userService = new UserService();
            _matchService = new MatchService();
            _imageService = new ImageService();
            _badgeService = new BadgeService();
            _notificationService = new NotificationService(null);
        }

        private async void HomeForm_Load(object sender, EventArgs e) // Filip Grgac
        {
            if (!AuthContext.IsLoggedIn)
            {
                MessageBox.Show("You are not logged in!");
                Close();
                return;
            }

            pnlFrontCard.Visible = true;
            pnlBackCard.Visible = false;
            isFrontVisible = true;
            
            await ReloadUsersAsync();
            await LoadBadgesAsync();
        }

        private void LoadUser() // Filip Grgac
        {
            if (!HasMoreUsers())
            {
                MessageBox.Show("Nema više korisnika");

                pbLike.Enabled = false;
                pbDisslike.Enabled = false;
                return;
            }

            var cards = _cards[_currentIndex];

            lblPlayer.Text = cards.FullName;
            lblYearsOld.Text = cards.Age + " years old";
            lblLevel.Text = cards.Level;
            lblPos.Text = cards.Position;
            lblFreq.Text = cards.FrequencyOfPlaying;
            lblDistance.Text = cards.DistanceKm.ToString("0.0") + " km";
            lblRating.Text = cards.Rating > 0 ? $"{cards.Rating:0.0} ★" : "";

            pbProfilePicture.Image = _imageService.ConvertUserImage(cards.Image, Properties.Resources.user__1_);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblPos_Click(object sender, EventArgs e)
        {

        }

        private bool HasMoreUsers()
        {
            return _cards != null && _currentIndex < _cards.Count;
        }


        private async void pictureBox1_Click(object sender, EventArgs e) // Filip Grgac
        {
            if (!HasMoreUsers()) return;
            var card = _cards[_currentIndex];

            await _matchService.DislikeAsync(AuthContext.CurrentUserId, card.UserId);

            _currentIndex++;
            LoadUser();
            OnSwiped();
        }

        private void lblDistance_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Upravlja animacijom okretanja kartice igrača.
        /// Tijekom izvođenja postupno smanjuje i ponovno povećava širinu kartice,
        /// mijenja vidljivu stranu kartice te zaustavlja timer po završetku animacije.
        /// </summary>
        private void flipTimer_Tick(object sender, EventArgs e) // Filip Grgac
        {
            if (isShrinking)
            {
                pnlPlayerCard.Left += 10;
                pnlPlayerCard.Width -= 20;

                if (pnlPlayerCard.Width <= 0)
                {
                    pnlFrontCard.Visible = !pnlFrontCard.Visible;
                    pnlBackCard.Visible = !pnlBackCard.Visible;

                    isFrontVisible = !isFrontVisible;
                    isShrinking = false;
                }
            }
            else
            {
                pnlPlayerCard.Left -= 10;
                pnlPlayerCard.Width += 20;

                if (pnlPlayerCard.Width >= cardOriginalWidth)
                {
                    pnlPlayerCard.Width = cardOriginalWidth;
                    pnlPlayerCard.Left = cardOriginalX;

                    flipTimer.Stop();
                }
            }
        }

        private void pbInfoFront_Click(object sender, EventArgs e) // Filip Grgac
        {
            cardOriginalWidth = pnlPlayerCard.Width;
            cardOriginalX = pnlPlayerCard.Left;

            isShrinking = true;
            flipTimer.Start();
        }

        private void pbInfoBack_Click(object sender, EventArgs e) // Filip Grgac
        {
            cardOriginalWidth = pnlPlayerCard.Width;
            cardOriginalX = pnlPlayerCard.Left;

            isShrinking = true;
            flipTimer.Start();
        }

        private async void pbLike_Click(object sender, EventArgs e) // Filip Grgac
        {
            if (!HasMoreUsers()) return;
            var card = _cards[_currentIndex];

            bool matched = await _matchService.LikeAsync(AuthContext.CurrentUserId, card.UserId);

            if (matched)
            {
                var result = MessageBox.Show("New Match for you!","MATCH!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (result == DialogResult.OK)
                {
                    await _notificationService
                        .MarkLatestMatchAsReadAsync(AuthContext.CurrentUserId);
                }
            }
            _currentIndex++;
            LoadUser();
            OnSwiped();
        }

        /// <summary>
        /// Obraduje swipe korisnika.
        /// Registrira swipe, provjerava je li dodijeljena nova značka
        /// i po potrebi prikazuje obavijest te osvježava prikaz znački.
        /// </summary>
        private async void OnSwiped() // Kristian Katulić
        {
            try
            {
                var (newSwipeNum, newlyAwarded) = await _badgeService.RegisterSwipeAsync(AuthContext.CurrentUserId);

                if (newlyAwarded.Count > 0)
                {
                    var names = string.Join(", ", newlyAwarded.Select(b => b.Name));
                    MessageBox.Show($"New badge unlocked: {names}", "Badge unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadBadgesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Swipe/badge error: " + ex.Message);
            }
        }

        /// <summary>
        /// Učitava i prikazuje sve značke korisnika.
        /// Značke se dohvaćaju iz servisa i prikazuju jedna ispod druge
        /// u panelu s omogućenim skrolanjem.
        /// </summary>
        private async Task LoadBadgesAsync() // Kristian Katulić
        {
            pnlBadges.Controls.Clear();
            pnlBadges.AutoScroll = true;

            var badges = (await _badgeService.GetUserBadgesAsync(AuthContext.CurrentUserId))
                .OrderBy(b => b.PointsRequired)
                .ToList();

            int padding = 10;
            int y = padding;

            int iconSize = 80;

            foreach (var b in badges)
            {
                var pb = new PictureBox
                {
                    Image = GetBadgeImage(b.BadgeId),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = iconSize,
                    Height = iconSize,
                    Left = (pnlBadges.ClientSize.Width - iconSize) / 2,
                    Top = y,
                    BorderStyle = BorderStyle.FixedSingle
                };

                pb.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                pb.BorderStyle = BorderStyle.None;

                pnlBadges.Controls.Add(pb);

                y += iconSize + padding;
            }

        }
        private Image GetBadgeImage(int badgeId) // Kristian Katulić
        {
            switch (badgeId)
            {
                case 1: return Properties.Resources.badge1;
                case 2: return Properties.Resources.badge2;
                case 3: return Properties.Resources.badge3;
                case 4: return Properties.Resources.badge4;
                case 5: return Properties.Resources.badge5;
                default: return null;
            }
        }

        /// <summary>
        /// Ponovno dohvaća i učitava listu korisnika prema trenutnim filterima
        /// i radijusu iz postavki aplikacije te osvježava prikaz kartica
        /// na HomeFormi.
        /// </summary>
        public async Task ReloadUsersAsync() // Karlo Kršak
        {
            int radiusKm = Properties.Settings.Default.RadiusKm;
            string gender = Properties.Settings.Default.FilterGender ?? "";
            string level = Properties.Settings.Default.FilterLevel ?? "";
            string position = Properties.Settings.Default.FilterPosition ?? "";
            string frequency = Properties.Settings.Default.FilterFrequency ?? "";

            _cards = await _userService.GetUsersForCardAsync(radiusKm, gender, level, position, frequency);

            if (_cards == null || _cards.Count == 0)
            {
                MessageBox.Show("There are no users nearby!");
                return;
            }
            LoadUser();
        }

    }
}
