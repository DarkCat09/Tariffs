using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Tariffs
{
    public partial class Form1 : Form
    {
        private int arrlenght = 50;
        int opersQuan = 4;

        public struct Filter
        {
            public String[] operators;

            public int min_minutes;
            public int max_minutes;

            public int min_sms;
            public int max_sms;

            public int min_inet;
            public int max_inet;

            public int min_pay;
            public int max_pay;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void оНасToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = MessageBox.Show(Properties.Resources.helpAboutUsNotReady,
                                "Информация",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
        }

        private void CloseProgram(object sender, EventArgs e)
        {
            Close();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgramHelp f2 = new ProgramHelp();
            f2.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            String remoteFile = "https://tariffslist.000webhostapp.com/list.txt";

            if (!File.Exists("list.txt"))
            {
                FileStream fstr = File.Create("list.txt");
                fstr.Close();
            }

            try
            {
                if (File.Exists("list.txt"))
                {
                    String boxres = MessageBox.Show("Обновить кэшированный файл с тарифами?",
                                                    "Вопрос",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Information).ToString();

                    if (boxres == "Yes")
                    {
                        client.DownloadFile(remoteFile, "list.txt");
                    }
                }
                else
                {
                    client.DownloadFile(remoteFile, "list.txt");
                }
            }
            catch (WebException)
            {
                String clickedButton = MessageBox.Show("Не удалось получить сведения о тарифах с сервера!\n" +
                                                       "Поиск кэшированного файла...",
                                                       "Предупреждение", MessageBoxButtons.AbortRetryIgnore,
                                                       MessageBoxIcon.Warning).ToString();

                if (clickedButton == "Abort")
                {
                    Close();
                }
                if (clickedButton == "Retry")
                {
                    try
                    {
                        client.DownloadFile(remoteFile, "list.txt");
                    }
                    catch (WebException)
                    {
                        _ = MessageBox.Show("Возможно, у Вас нет подключения к Интернету,\n" +
                                            "или адрес к веб-серверу был неверно указан разработчиком.\n" +
                                            "Приложение будет закрыто.",
                                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                    }
                }
                if (clickedButton == "Ignore")
                {
                    if (File.Exists("list.txt"))
                    {
                        StreamReader s = new StreamReader("list.txt");
                        String firstcode = s.ReadLine();
                        if (!(firstcode == "Tariffs"))
                        {
                            _ = MessageBox.Show("Неверный тип файла!\n\n" + 
                                                "Так как невозможно загрузить тарифы,\n" + 
                                                "Приложение будет закрыто.",
                                                "Ошибка",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                            s.Close();
                            Close();
                        }
                        s.Close();
                    }
                    else
                    {
                        _ = MessageBox.Show("Файл почему-то не найден!\n" + 
                                            "Приложение будет закрыто.",
                                            "Ошибка",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        Close();
                    }
                }
            }

            ReadListFile();
        }

        private void ReadListFile()
        {
            ReadListFile(new Filter());
        }

        private void ReadListFile(Filter filt_params)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = 1;
            StreamReader listfile = new StreamReader("list.txt");
            String firststr = listfile.ReadLine();
            String[,] tariffsArr = new String[arrlenght, 8];
            String line;
            int propind = 0;
            int i = 0;

            if (!(firststr == "Tariffs"))
            {
                MessageBox.Show("Неверный тип файла!\n\n" +
                                "Так как невозможно загрузить тарифы,\n" +
                                "приложение будет закрыто.",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                listfile.Close();
                Close();
            }

            line = listfile.ReadLine();

            while (line != "End")
            {
                if (!(line.StartsWith("#")) && line != "" && line != null)
                {
                    for (int ind = 1; ind < 8; ind++)
                    {
                        tariffsArr[i, ind] = line.Split(new char[] { ',' })[ind - 1];
                    }
                }

                line = listfile.ReadLine();
                i++;
            }

            for (int rowind = 0; rowind < i; rowind++)
            {
                tariffsArr[rowind, 0] = "false";
            }

            listfile.Close();

            String[] rowsArray = new String[8];

            for (int elind = 0; elind < i; elind++)
            {
                propind = 0;
                while (propind < 8)
                {
                    rowsArray[propind] = tariffsArr[elind, propind];
                    propind++;
                }

                bool needToAdd = true;
                bool[] operMatch = new bool[opersQuan];
                int x;

                //operators
                /*for (int curOperInd = 0; curOperInd < opersQuan; curOperInd++)
                {
                    if (filt_params.operators != null &&
                        rowsArray[1] != null &&
                        rowsArray[1] == filt_params.operators[curOperInd])
                    {
                        operMatch[curOperInd] = true;
                    }
                }

                for (int curOperInd = 0; curOperInd < opersQuan; curOperInd++)
                {
                    if (operMatch[curOperInd])
                    {
                        needToAdd = true;
                    }
                }*/

                //mins
                if (filt_params.min_minutes > 0 &&
                    rowsArray[3] != null &&
                    Int32.TryParse(rowsArray[3], out x) &&
                    Convert.ToInt32(rowsArray[3]) < filt_params.min_minutes)
                {
                    needToAdd = false;
                }

                if (filt_params.max_minutes > 0 &&
                    rowsArray[3] != null &&
                    Int32.TryParse(rowsArray[3], out x) &&
                    Convert.ToInt32(rowsArray[3]) > filt_params.max_minutes)
                {
                    needToAdd = false;
                }

                //sms
                if (filt_params.min_sms > 0 &&
                    rowsArray[4] != null &&
                    Int32.TryParse(rowsArray[4], out x) &&
                    Convert.ToInt32(rowsArray[4]) < filt_params.min_sms)
                {
                    needToAdd = false;
                }

                if (filt_params.max_sms > 0 &&
                    rowsArray[4] != null &&
                    Int32.TryParse(rowsArray[4], out x) &&
                    Convert.ToInt32(rowsArray[4]) > filt_params.max_sms)
                {
                    needToAdd = false;
                }

                //internet
                if (filt_params.min_inet > 0 &&
                    rowsArray[6] != null &&
                    Int32.TryParse(rowsArray[6], out x) &&
                    Convert.ToInt32(rowsArray[6]) < filt_params.min_inet)
                {
                    needToAdd = false;
                }

                if (filt_params.max_inet > 0 &&
                    rowsArray[6] != null &&
                    Int32.TryParse(rowsArray[6], out x) &&
                    Convert.ToInt32(rowsArray[6]) > filt_params.max_inet)
                {
                    needToAdd = false;
                }

                //pay
                if (filt_params.min_pay > 0 &&
                    rowsArray[7] != null &&
                    Int32.TryParse(rowsArray[7], out x) &&
                    Convert.ToInt32(rowsArray[7]) < filt_params.min_pay)
                {
                    needToAdd = false;
                }

                if (filt_params.max_pay > 0 &&
                    rowsArray[7] != null &&
                    Int32.TryParse(rowsArray[7], out x) &&
                    Convert.ToInt32(rowsArray[7]) > filt_params.max_pay)
                {
                    needToAdd = false;
                }

                //adding
                if (needToAdd)
                {
                    _ = dataGridView1.Rows.Add(rowsArray);
                }
            }
        }

        private void openBrowser(String operatorsUrl)
        {
            Browser b = new Browser(operatorsUrl);
            b.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openBrowser("https://www.megafon.ru/");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openBrowser("https://www.mts.ru/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openBrowser("https://www.beeline.ru/");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openBrowser("https://www.tele2.ru/");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openBrowser("https://www.tinkoff.ru/mobile-operator/");
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadListFile();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Filter filt = new Filter();

            //minutes
            filt.min_minutes = Convert.ToInt32(numericUpDown1.Value);
            filt.max_minutes = Convert.ToInt32(numericUpDown2.Value);

            //sms - short message service
            filt.min_sms = Convert.ToInt32(numericUpDown3.Value);
            filt.max_sms = Convert.ToInt32(numericUpDown4.Value);

            //internet
            filt.min_inet = Convert.ToInt32(numericUpDown5.Value);
            filt.max_inet = Convert.ToInt32(numericUpDown6.Value);

            //pay
            filt.min_pay = Convert.ToInt32(numericUpDown7.Value);
            filt.max_pay = Convert.ToInt32(numericUpDown8.Value);

            ReadListFile(filt);
        }
    }
}
