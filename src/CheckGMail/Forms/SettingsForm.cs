using Baleinoid.Windows.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckGMail
{
    public partial class SettingsForm : Form
    {
        private Configuration config = new Configuration();

        public SettingsForm()
        {
            config.Load();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(config.Language);

            InitializeComponent();

            SetControlsValue();

            this.Icon = MyResources.Instance.ApplicationIcon;
            notifyIcon.Icon = MyResources.Instance.NotificationInit;

            UpdateRequestQ();
            UpdateInterval();

            CheckMail();
            timerCheck.Start();
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            GetControlsValue();
            config.Save();

            UpdateRequestQ();
            UpdateInterval();
            UpdateOnStart();

            this.Hide();

            CheckMail();
        }

        private void UpdateRequestQ()
        {
            GoogleBusiness.Instance.RequestQ = config.RequestQ;
        }

        private void UpdateInterval()
        {
            this.timerCheck.Interval = config.IntervalInMillisecond;
        }

        private void UpdateOnStart()
        {
            if (this.cbOnStart.Checked)
                StartApplicationOnWindowsStartup.Enable();
            else
                StartApplicationOnWindowsStartup.Disable();
        }

        private void GetControlsValue()
        {
            GetLanguage();
            GetFilter();
            GetInterval();
        }


        private void SetControlsValue()
        {
            SetLinkLabel();
            SetLanguage();
            SetFilter();
            SetInterval();
            SetOnStart();
        }

        private void SetLinkLabel()
        {
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = config.UrlMoreInformation;
            this.linkLabel1.Links.Clear();
            this.linkLabel1.Links.Add(link);
        }

        private void GetLanguage()
        {
            config.Language = "en";
            if (this.rbFrench.Checked)
                config.Language = "fr";
        }

        private void SetLanguage()
        {
            if (config.Language == "fr")
                this.rbFrench.Checked = true;
            this.rbEnglish.Checked = !this.rbFrench.Checked;
        }

        private void SetOnStart()
        {
            cbOnStart.Checked = StartApplicationOnWindowsStartup.IsEnabled();
        }

        private void GetFilter()
        {
            config.FilterType = FilterEnum.InboxOnly;
            if (rbCustomQ.Checked)
                config.FilterType = FilterEnum.CustomQ;
            else if (rbAll.Checked)
                config.FilterType = FilterEnum.AllMessageExceptSpamAndTrash;

            config.CustomQ = this.tbCustomQ.Text;
        }

        private void SetFilter()
        {
            this.tbCustomQ.Text = config.CustomQ;
            switch (config.FilterType)
            {
                case FilterEnum.AllMessageExceptSpamAndTrash:
                    this.rbAll.Checked = true;
                    break;
                case FilterEnum.CustomQ:
                    this.rbCustomQ.Checked = true;
                    break;
                default: // FilterEnum.InboxOnly
                    this.rbInbox.Checked = true;
                    break;
            }
        }

        private void GetInterval()
        {
            this.config.IntervalInMinutes = (int) this.numIntervalMinutes.Value;
        }

        private void SetInterval()
        {
            this.numIntervalMinutes.Value = this.config.IntervalInMinutes;
            this.timerCheck.Interval = this.config.IntervalInMillisecond;
        }

        private void viewOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetControlsValue();
            this.Show();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ViewInbox();
        }

        private void timerCheck_Tick(object sender, EventArgs e)
        {
            CheckMail();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }


        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CleanExit();
        }

        private void CleanExit()
        {
            this.timerCheck.Stop();
            this.notifyIcon.Visible = false;
            Application.Exit();
        }

        private void checkMailNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckMail();
        }

        private void inboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewInbox();
        }


        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm a = new AboutForm();
            a.ShowDialog();
        }

        private void revokeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoogleBusiness.Instance.Revoke();
            CleanExit();
        }

        private void ViewInbox()
        {
            Process.Start(config.InboxUrl);
        }

        private void CheckMail()
        {
            try
            {
                long? messagesCount = GoogleBusiness.Instance.GMailCheckMessages();
                this.notifyIcon.Icon = (messagesCount.GetValueOrDefault() > 0) ? MyResources.Instance.NotificationMessages : MyResources.Instance.NotificationNoMessage;
            }
            catch(Exception)
            {
                this.notifyIcon.Icon = MyResources.Instance.NotificationError;
            }
        }
   }
}
