using System;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace CheckGMail
{
    public class MyResources : IDisposable
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

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_notifInit != null)
                    {
                        _notifInit.Dispose();
                    }

                    if (_notifMessages != null)
                    {
                        _notifMessages.Dispose();
                    }

                    if (_notifError != null)
                    {
                        _notifError.Dispose();
                    }
                }

                _notifInit = null;
                _notifMessages = null;
                _notifError = null;

                disposedValue = true;
            }
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
