using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckGMail
{
    class Configuration
    {
        public string Language { get; set; }

        public int IntervalInMinutes { get; set; }

        public string CustomQ { get; set; }

        public FilterEnum FilterType { get; set; }

        private const int MINUTE_IN_MILLISECOND = 60000;
        public int IntervalInMillisecond
        {
            get
            {
                if (this.IntervalInMinutes > 0)
                    return this.IntervalInMinutes * MINUTE_IN_MILLISECOND;
                else
                    return MINUTE_IN_MILLISECOND;
            }
        }

        public string RequestQ
        {
            get
            {
                string q = "in:inbox is:unread";
                if (FilterType == FilterEnum.AllMessageExceptSpamAndTrash)
                    q = "is:unread -in:trash -in:spam";
                else if (FilterType == FilterEnum.CustomQ)
                    q = CustomQ;
                return q;
            }
        }

        public void Load()
        {
            this.Language = (string)Properties.Settings.Default["Language"];
            this.CustomQ = (string)Properties.Settings.Default["CustomQ"];
            this.FilterType = (FilterEnum)Properties.Settings.Default["FilterType"];
            this.IntervalInMinutes = (int)Properties.Settings.Default["IntervalInMinutes"];
        }

        public void Save()
        {
            Properties.Settings.Default["Language"] = this.Language;
            Properties.Settings.Default["CustomQ"] = this.CustomQ;
            Properties.Settings.Default["FilterType"] = (int)this.FilterType;
            Properties.Settings.Default["IntervalInMinutes"] = this.IntervalInMinutes;

            Properties.Settings.Default.Save();
        }

        private const string URL_MORE_INFORMATION = @"https://support.google.com/mail/answer/7190?hl={0}";
        public object UrlMoreInformation
        {
            get
            {
                return string.Format(URL_MORE_INFORMATION, this.Language);
            }
        }

        private const string URL_INBOX = @"https://mail.google.com";
        public string InboxUrl
        {
            get
            {
                return URL_INBOX;
            }
        }
    }
}
