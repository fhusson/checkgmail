using System;
using System.Windows.Forms;

namespace CheckGMail
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.labelProduct.Text = Configuration.ProductName;
            this.labelVersion.Text = Configuration.ProductVersion;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
