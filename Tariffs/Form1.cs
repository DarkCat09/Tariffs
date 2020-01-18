﻿using System;
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
            WebClient client = new WebClient();
            String remoteFile = "https://tariffslist.000webhostapp.com/list.txt";

            if (!File.Exists("list.txt"))
            {
                _ = File.Create("list.txt");
            }

            try
            {
                client.DownloadFile(remoteFile, "list.txt");
            }
            catch (WebException)
            {
                String clickedButton = MessageBox.Show("Не удалось получить сведения о тарифах с сервера.\n" +
                                                       "Поиск кэшированного файла...",
                                                       "Ошибка", MessageBoxButtons.AbortRetryIgnore,
                                                       MessageBoxIcon.Error).ToString();

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
            listfile.Close();
        }
    }
}
