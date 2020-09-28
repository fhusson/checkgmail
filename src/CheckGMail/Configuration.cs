using System.Globalization;
using System.Reflection;

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
            this.Language = (string)Properties.Settings.Default[nameof(Language)];
            this.CustomQ = (string)Properties.Settings.Default[nameof(CustomQ)];
            this.FilterType = (FilterEnum)Properties.Settings.Default[nameof(FilterType)];
            this.IntervalInMinutes = (int)Properties.Settings.Default[nameof(IntervalInMinutes)];
        }

        public void Save()
        {
            Properties.Settings.Default[nameof(Language)] = this.Language;
            Properties.Settings.Default[nameof(CustomQ)] = this.CustomQ;
            Properties.Settings.Default[nameof(FilterType)] = (int)this.FilterType;
            Properties.Settings.Default[nameof(IntervalInMinutes)] = this.IntervalInMinutes;

            Properties.Settings.Default.Save();
        }

        private const string URL_MORE_INFORMATION = @"https://support.google.com/mail/answer/7190?hl={0}";
        public object UrlMoreInformation
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, URL_MORE_INFORMATION, this.Language);
            }
        }

        private const string URL_INBOX = @"https://mail.google.com";
        public static string InboxUrl => URL_INBOX;

        public static string ProductName
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string ProductVersion
        {
            get
            {
                var v = Assembly.GetExecutingAssembly().GetName().Version;
                return string.Format(CultureInfo.CurrentCulture, "Version {0}.{1}.{2}", v.Major, v.Minor, v.Revision);
            }
        }
    }
}
