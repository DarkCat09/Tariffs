using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tariffs
{
    public partial class Browser : Form
    {

        public Browser(String url)
        {
            InitializeComponent();

            Uri weburi = new Uri(url);
            String openInBrowser = MessageBox.Show("Открыть ссылку в браузере?",
                                                   "Вопрос",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Information).ToString();

            if (openInBrowser == "Yes")
            {
                _ = Process.Start(weburi.ToString());
            }
            else
            {
                webBrowser1.Url = weburi;
                webBrowser1.ScriptErrorsSuppressed = true;
            }
        }
    }
}
