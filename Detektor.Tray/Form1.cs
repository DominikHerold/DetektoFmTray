using System;
using System.Configuration;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;

using Detektor.Core;

using Timer = System.Timers.Timer;

namespace Detektor.Tray
{
    public partial class Form1 : Form
    {
        private readonly Timer _timer;

        private readonly Core.Detektor _detektor;

        private readonly Regex _trimmer;

        public Form1()
        {
            _trimmer = new Regex(" +", RegexOptions.Compiled);
            var host = ConfigurationManager.AppSettings["host"];
            InitializeComponent();
            var client = new WebClientWrapper();
            _detektor = new Core.Detektor(client, host);

            _timer = new Timer(5000) { AutoReset = false };
            _timer.Elapsed += OnTimeEvent;
            OnTimeEvent(null, null);
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
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
                }
                finally
                {
                    _timer.Enabled = true;
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnTimeEvent(null, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
