using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tariffs
{
    public partial class ProgramHelp : Form
    {
        public ProgramHelp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Visible)
            {
                textBox1.Visible = false;
                button2.Text = "Показать описание";
            }
            else
            {
                textBox1.Visible = true;
                button2.Text = "Скрыть описание";
            }
        }
    }
}
