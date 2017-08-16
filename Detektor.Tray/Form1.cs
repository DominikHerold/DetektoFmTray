using System;
using System.Configuration;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Detektor.Core;
using Detektor.Tray.Properties;

namespace Detektor.Tray
{
    public partial class Form1 : Form
    {
        private readonly Core.Detektor _detektor;

        private readonly Regex _trimmer;

        public Form1()
        {
            _trimmer = new Regex(" +", RegexOptions.Compiled);
            var host = ConfigurationManager.AppSettings["host"];
            InitializeComponent();
            var client = new WebClientWrapper();
            _detektor = new Core.Detektor(client, host);
            notifyIcon1.Icon = new Icon("warning.ico");
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            lock (this)
            {
                try
                {
                    var content = _detektor.GetTitle();
                    content = _trimmer.Replace(content, " ");
                    if (content.Length >= 64)
                        content = content.Substring(0, 63);

                    notifyIcon1.Icon = new Icon("warning.ico");
                    notifyIcon1.Text = content;
                    notifyIcon1.ShowBalloonTip(10000, string.Empty, content, ToolTipIcon.Info);
                }
                catch (Exception)
                {
                    notifyIcon1.Text = Resources.Form1_notifyIcon1_MouseClick_ERROR;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
