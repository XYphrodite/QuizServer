using System;
using System.Windows.Forms;
using SimpleTCP;

namespace Labs_OS6r2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpServer server;

        int number = 0;
        string question;
        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;
            server.StringEncoder = System.Text.Encoding.UTF8;
            StopButton.Enabled = false;
            dataGridView1.Rows.Add("1", "Сколько бит в килобайте?", "4096", "128", "8", "8196", "d");
            dataGridView1.Rows.Add("2", "Является ли файл каталогом?", "да", "нет", "—", "—", "b");
            dataGridView1.Rows.Add("3", "Максимальный объём оперативной памяти в 32-ух битных системах?", 
                "2гб", "4гб", "128мб", "64кб", "b");
            richTextBox1.ReadOnly = true;
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(textBox1.Text);
            server.Start(ip, int.Parse(textBox2.Text));
            StopButton.Enabled = true;
            StartButton.Enabled = false;
            richTextBox1.Text += "Server is running...\n";
            server.DataReceived += Server_DataReceived;
        }
        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            richTextBox1.Invoke((MethodInvoker)delegate ()
            {
                if (e.MessageString == "get")
                {
                    e.Reply((dataGridView1.RowCount-1).ToString());
                    richTextBox1.Text += "someone connected\n";
                }
                else if (Char.IsLetter(e.MessageString[0]))
                {
                    richTextBox1.Text += "answer was got\n";
                    if (dataGridView1[6, number].Value.ToString() == e.MessageString)
                    {
                        e.Reply("Ответ верный");
                    }
                    else
                    {
                        e.Reply("Ответ неверный");
                    }
                }
                else
                {
                    richTextBox1.Text += "number was got\n";
                    number = int.Parse(e.MessageString);
                    question = dataGridView1[1, number].Value.ToString() + "\n" +
                    "a)" + dataGridView1[2, number].Value.ToString() + "\n" +
                    "b)" + dataGridView1[3, number].Value.ToString() + "\n" +
                    "c)" + dataGridView1[4, number].Value.ToString() + "\n" +
                    "d)" + dataGridView1[5, number].Value.ToString();
                    e.Reply(question);
                }
            });
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            server.Stop();
            StopButton.Enabled = false;
            StartButton.Enabled = true;
            richTextBox1.Text += "Server was stopped\n";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}