using EL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Padeler
{
    public partial class PushNotificationForm : Form // Filip Grgac
    {
        private readonly Notification _notification;

        public event EventHandler NotificationRead;
        public PushNotificationForm(Notification notification)
        {
            InitializeComponent();
            _notification = notification;

            lblMessage.Text = _notification.Content;
        }

        public void PositionOwner()
        {
            if (this.Owner == null) return;

            this.Location = new Point(Owner.Right - this.Width - 20, Owner.Bottom - this.Height - 40);
        }

        private void PushNotificationForm_Load(object sender, EventArgs e)
        {

        }

        private void lnlblOk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NotificationRead?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
