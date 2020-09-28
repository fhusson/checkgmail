using System;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace CheckGMail
{
    public class MyResources: IDisposable
    {
        private readonly Assembly _assembly;

        private MyResources()
        {
            _assembly = Assembly.GetExecutingAssembly();
            ApplicationIcon = new Icon(_assembly.GetManifestResourceStream("CheckGMail.Resources.NotifInit.ico"));
            NotificationMessages = new Icon(_assembly.GetManifestResourceStream("CheckGMail.Resources.NotifMessages.ico"));
            NotificationError = new Icon(_assembly.GetManifestResourceStream("CheckGMail.Resources.NotifError.ico"));
        }

        private static MyResources instance;
        public static MyResources Instance
        {
            get
            {
                if (instance == null)
                    instance = new MyResources();

                return instance;
            }
        }

        public Icon ApplicationIcon { get; private set; }

        public Icon NotificationInit
        {
            get
            {
                return ApplicationIcon;
            }
        }

        public Icon NotificationMessages { get; private set; }

        public Icon NotificationNoMessage
        {
            get
            {
                return ApplicationIcon;
            }
        }

        public Icon NotificationError { get; private set; }

        public Stream GetClientSecretsStream()
        {
            return _assembly.GetManifestResourceStream("CheckGMail.Resources.client_secrets.json");
        }

        #region IDisposable Support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (ApplicationIcon != null)
                    {
                        ApplicationIcon.Dispose();
                    }

                    if (NotificationMessages != null)
                    {
                        NotificationMessages.Dispose();
                    }

                    if (NotificationError != null)
                    {
                        NotificationError.Dispose();
                    }
                }

                ApplicationIcon = null;
                NotificationMessages = null;
                NotificationError = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
