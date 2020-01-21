using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Tariffs
{
    public partial class Form1 : Form
    {
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
            int arrlenght = 50;
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
                String clickedButton = MessageBox.Show("Не удалось получить сведения о тарифах с сервера.\n" +
                                                       "Поиск кэшированного файла...",
                                                       "Ошибка", MessageBoxButtons.AbortRetryIgnore,
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

            StreamReader listfile = new StreamReader("list.txt");
            String firststr = listfile.ReadLine();
            String[,] tariffsArr = new String[arrlenght, 8];
            String line;
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
                    for (int ind = 1; ind < 7; ind++)
                    {
                        tariffsArr[i, ind] = line.Split(new char[] { ',' })[ind];
                    }
                }

                line = listfile.ReadLine();
                i++;
            }

            for (int rowind = 0; rowind < 50; rowind++)
            {
                tariffsArr[rowind, 0] = "false";
            }

            listfile.Close();

            for (int j = 0; j < arrlenght; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    _ = dataGridView1.Rows.Add(tariffsArr[j, k]);
                }
            }
        }
    }
}
