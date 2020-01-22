using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tariffs
{
    public partial class Browser : Form
    {
        private Uri weburi;

        public Browser(String url)
        {
            InitializeComponent();

            weburi = new Uri(url);
            webBrowser1.Url = weburi;
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            String openInBrowser = MessageBox.Show("Открыть ссылку в браузере?",
                                                   "Вопрос",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Information).ToString();

            if (openInBrowser == "Yes")
            {
                _ = Process.Start(weburi.ToString());
                Close();
            }
        }
    }
}
