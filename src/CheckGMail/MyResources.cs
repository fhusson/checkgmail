using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace CheckGMail
{
    public class MyResources
    {
        private Assembly _assembly = null;
        private Icon _notifInit = null;
        private Icon _notifMessages = null;
        private Icon _notifError = null;

        private MyResources()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _notifInit = new Icon(_assembly.GetManifestResourceStream("CheckGMail.Resources.NotifInit.ico"));
            _notifMessages = new Icon(_assembly.GetManifestResourceStream("CheckGMail.Resources.NotifMessages.ico"));
            _notifError = new Icon(_assembly.GetManifestResourceStream("CheckGMail.Resources.NotifError.ico"));
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

        public Icon ApplicationIcon {
            get
            {
                return _notifInit;
            }
        }

        public Icon NotificationInit
        {
            get
            {
                return _notifInit;
            }
        }

        public Icon NotificationMessages
        {
            get
            {
                return _notifMessages;
            }
        }

        public Icon NotificationNoMessage
        {
            get
            {
                return _notifInit;
            }
        }

        public Icon NotificationError
        {
            get
            {
                return _notifError;
            }
        }

        public Stream GetClientSecretsStream()
        {
            return _assembly.GetManifestResourceStream("CheckGMail.Resources.client_secrets.json");
        }
    }
}
