using log4net.Repository.Hierarchy;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ReceiptPrinterLib.Models;
using ReceiptPrinterLib.PrinterDevice;

using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

namespace ReceiptPrinterTestApp
{
    public partial class Form1 : Form
    {
        readonly ILogger<Form1> log;
        readonly IServiceProvider serviceProvider;

        GeneralPrinter? generalPrinter = null;

        public Form1(ILogger<Form1> logger, IServiceProvider serviceProvider)
        {
            this.log = logger;
            this.serviceProvider = serviceProvider;

            InitializeComponent();
        }

        /// <summary>
        /// �ø��� ��Ʈ ���
        /// </summary>
        void FillSerialPortList()
        {
            SerialPortComboBox.Items.Clear();
            SerialPortComboBox.Items.AddRange(SerialPort.GetPortNames());
            if (SerialPortComboBox.Items.Count > 0)
                SerialPortComboBox.SelectedIndex = 0;
        }

        void UpdateConnectStatus(bool connected)
        {
            OpenPortButton.Enabled = !connected;
            ClosePortButton.Enabled = connected;

            groupBox2.Enabled = connected;
            groupBox3.Enabled = connected;
            groupBox4.Enabled = connected;
            groupBox5.Enabled = connected;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillSerialPortList();
            BaudrateComboBox.SelectedIndex = 0;  // 9600 bps

            UpdateConnectStatus(false);
        }

        readonly Regex number = new Regex(@"\d+");
        private void OpenPortButton_Click(object sender, EventArgs e)
        {
            int baudRate = 0;
            int.TryParse(BaudrateComboBox.Text, out baudRate);
            Debug.Assert(baudRate > 0);

            int portNo = 0;
            if (!int.TryParse(number.Match(SerialPortComboBox.Text).Value.ToString(), out portNo))
                return;

            log.LogInformation("����� �ø��� ��Ʈ = " + portNo + ", baud-rate = " + baudRate);
            var instance = serviceProvider.GetRequiredService<GeneralPrinter>();
            bool ok = instance.TryOpen(portNo, baudRate);

            UpdateConnectStatus(ok);
            if (!ok)
            {
                MessageBox.Show("�ø��� ��Ʈ[" + portNo + "�� ����� �� �����ϴ�");
                return;
            }

            generalPrinter = instance;
        }

        private void ClosePortButton_Click(object sender, EventArgs e)
        {
            generalPrinter?.Dispose();

            UpdateConnectStatus(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SizeEnlargeEnum textSize = radioButton4.Checked ? SizeEnlargeEnum.BothDouble :
                                        radioButton3.Checked ? SizeEnlargeEnum.DoubleHeight :
                                        radioButton2.Checked ? SizeEnlargeEnum.DoubleWidth : SizeEnlargeEnum.Normal;

            var text2 = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(text2))
                return;

            // �ؽ�Ʈ ũ�� ����
            generalPrinter?.SetCharacterSize(textSize);
            // �ؽ�Ʈ �μ�
            generalPrinter?.PrintText(text2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string data = textBox3.Text.Trim();
            if (string.IsNullOrEmpty(data))
                return;

            // HRI �μ� ���� ����
            generalPrinter?.SetPrintHRI(checkBox1.Checked ? ReceiptPrinterLib.PrinterDevice.protocol.BasicProtocol.HRIPrintPositionEnum.BelowTheBarcode
                                                            : ReceiptPrinterLib.PrinterDevice.protocol.BasicProtocol.HRIPrintPositionEnum.DoNotPrint);
            // �μ�
            int size = 0;
            int.TryParse(textBox4.Text.Trim(), out size);
            if (size < 1 || size > 255)
            {
                MessageBox.Show("���ڵ� ���̸� 1~255 �� ������ �ּ���");
                return;
            }
            generalPrinter?.PrintBarcodeWithHeight(data, size);
        }

        void UpdateTextBox()
        {
            bool isDoubleWidth = radioButton2.Checked || radioButton4.Checked;
            int MaxCharAtLine = 42 / (isDoubleWidth ? 2 : 1);

            string src = textBox1.Text.TrimEnd();
            StringBuilder line = new StringBuilder();
            var lines = new List<string>();

            foreach (char ch in src)
            {
                int charSize = char.IsAscii(ch) ? 1 : 2;

                if (ch == '\n' || line.Length + charSize > MaxCharAtLine)
                {
                    lines.Add(line.ToString());
                    line.Clear();

                    if (ch == '\n')
                        continue;
                }

                line.Append(ch);
            }
            if (line.Length > 0)
                lines.Add(line.ToString());

            textBox2.Text = string.Concat(lines.Select(a => a + "\r\n"));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateTextBox();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTextBox();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (loaded == null)
                return;

            generalPrinter?.PrintRasterImage(loaded, ReceiptPrinterLib.PrinterDevice.protocol.BasicProtocol.ImagePrintMode.Normal);
        }

        Bitmap PrepareBlackWhiteImage(Image a)
        {
            Bitmap src = new Bitmap(a);
            Bitmap blackWhite = new Bitmap(src.Width, src.Height);
            var colors = new HashSet<Color>();

            for (int i = 0; i < src.Width; i++)
            {
                for (int j = 0; j < src.Height; j++)
                {
                    var pixel = src.GetPixel(i, j);

                    blackWhite.SetPixel(i, j, pixel.R < 32 && pixel.G < 32 && pixel.B < 32 ? Color.Black : Color.White);
                }
            }

            return blackWhite;
        }

        Bitmap? loaded = null;
        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                var img = Image.FromFile(openFileDialog1.FileName);
                loaded = PrepareBlackWhiteImage(img);
                pictureBox1.Image = loaded;
            }
            catch (Exception xe)
            {
                MessageBox.Show("�̹����� �ҷ� �� �� �����ϴ�. " + xe.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int feedLines = 0;
            if (!int.TryParse(textBox5.Text, out feedLines))
            {
                MessageBox.Show("�� ���� ���ڷ� �Է��� �ּ���");
                return;
            }

            if (feedLines > 0)
                generalPrinter?.FeedLines(feedLines);

            generalPrinter?.CutPaper();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}