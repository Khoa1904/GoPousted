namespace ReceiptPrinterTestApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            ClosePortButton = new Button();
            OpenPortButton = new Button();
            BaudrateComboBox = new ComboBox();
            label3 = new Label();
            SerialPortComboBox = new ComboBox();
            label2 = new Label();
            comboBox1 = new ComboBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            label6 = new Label();
            button3 = new Button();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            groupBox3 = new GroupBox();
            button4 = new Button();
            checkBox1 = new CheckBox();
            textBox4 = new TextBox();
            label5 = new Label();
            textBox3 = new TextBox();
            label4 = new Label();
            groupBox4 = new GroupBox();
            button6 = new Button();
            button5 = new Button();
            pictureBox1 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            groupBox5 = new GroupBox();
            button1 = new Button();
            textBox5 = new TextBox();
            label7 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ClosePortButton);
            groupBox1.Controls.Add(OpenPortButton);
            groupBox1.Controls.Add(BaudrateComboBox);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(SerialPortComboBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(808, 120);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "프린터 접속";
            // 
            // ClosePortButton
            // 
            ClosePortButton.Enabled = false;
            ClosePortButton.Location = new Point(636, 77);
            ClosePortButton.Name = "ClosePortButton";
            ClosePortButton.Size = new Size(102, 23);
            ClosePortButton.TabIndex = 7;
            ClosePortButton.Text = "CLOSE";
            ClosePortButton.UseVisualStyleBackColor = true;
            ClosePortButton.Click += ClosePortButton_Click;
            // 
            // OpenPortButton
            // 
            OpenPortButton.Location = new Point(529, 77);
            OpenPortButton.Name = "OpenPortButton";
            OpenPortButton.Size = new Size(101, 23);
            OpenPortButton.TabIndex = 6;
            OpenPortButton.Text = "OPEN";
            OpenPortButton.UseVisualStyleBackColor = true;
            OpenPortButton.Click += OpenPortButton_Click;
            // 
            // BaudrateComboBox
            // 
            BaudrateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            BaudrateComboBox.FormattingEnabled = true;
            BaudrateComboBox.Items.AddRange(new object[] { "9600", "14400", "19600", "38400", "57600", "115200" });
            BaudrateComboBox.Location = new Point(495, 27);
            BaudrateComboBox.Name = "BaudrateComboBox";
            BaudrateComboBox.Size = new Size(121, 23);
            BaudrateComboBox.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(430, 30);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 4;
            label3.Text = "통신 속도";
            // 
            // SerialPortComboBox
            // 
            SerialPortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SerialPortComboBox.FormattingEnabled = true;
            SerialPortComboBox.Location = new Point(265, 27);
            SerialPortComboBox.Name = "SerialPortComboBox";
            SerialPortComboBox.Size = new Size(121, 23);
            SerialPortComboBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(228, 30);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 2;
            label2.Text = "포트";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Enabled = false;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "일반" });
            comboBox1.Location = new Point(73, 27);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 30);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 0;
            label1.Text = "프린터";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Controls.Add(radioButton4);
            groupBox2.Controls.Add(radioButton3);
            groupBox2.Controls.Add(radioButton2);
            groupBox2.Controls.Add(radioButton1);
            groupBox2.Location = new Point(12, 138);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(808, 206);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "텍스트";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 33);
            label6.Name = "label6";
            label6.Size = new Size(70, 15);
            label6.TabIndex = 9;
            label6.Text = "텍스트 크기";
            // 
            // button3
            // 
            button3.Location = new Point(659, 166);
            button3.Name = "button3";
            button3.Size = new Size(101, 23);
            button3.TabIndex = 8;
            button3.Text = "텍스트 인쇄";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.Location = new Point(458, 30);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(327, 130);
            textBox2.TabIndex = 5;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(125, 30);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(327, 130);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(18, 130);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(93, 19);
            radioButton4.TabIndex = 3;
            radioButton4.Text = "가로/세로 x2";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(18, 105);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(64, 19);
            radioButton3.TabIndex = 2;
            radioButton3.Text = "세로 x2";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(18, 80);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(64, 19);
            radioButton2.TabIndex = 1;
            radioButton2.Text = "가로 x2";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(18, 55);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(49, 19);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "기본";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button4);
            groupBox3.Controls.Add(checkBox1);
            groupBox3.Controls.Add(textBox4);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(textBox3);
            groupBox3.Controls.Add(label4);
            groupBox3.Location = new Point(12, 350);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(808, 135);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "바코드";
            // 
            // button4
            // 
            button4.Location = new Point(659, 102);
            button4.Name = "button4";
            button4.Size = new Size(101, 23);
            button4.TabIndex = 9;
            button4.Text = "바코드 인쇄";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(228, 73);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(72, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "HRI 인쇄";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(136, 71);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(77, 23);
            textBox4.TabIndex = 3;
            textBox4.Text = "81";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 75);
            label5.Name = "label5";
            label5.Size = new Size(70, 15);
            label5.TabIndex = 2;
            label5.Text = "바코드 높이";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(136, 34);
            textBox3.MaxLength = 64;
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "인쇄할 바코드 데이터";
            textBox3.Size = new Size(567, 23);
            textBox3.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 37);
            label4.Name = "label4";
            label4.Size = new Size(82, 15);
            label4.TabIndex = 0;
            label4.Text = "바코드 데이터";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button6);
            groupBox4.Controls.Add(button5);
            groupBox4.Controls.Add(pictureBox1);
            groupBox4.Location = new Point(12, 491);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(808, 154);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "이미지";
            // 
            // button6
            // 
            button6.Location = new Point(552, 115);
            button6.Name = "button6";
            button6.Size = new Size(101, 23);
            button6.TabIndex = 11;
            button6.Text = "이미지 선택";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Location = new Point(659, 115);
            button5.Name = "button5";
            button5.Size = new Size(101, 23);
            button5.TabIndex = 10;
            button5.Text = "이미지 인쇄";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(6, 22);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(344, 126);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(button1);
            groupBox5.Controls.Add(textBox5);
            groupBox5.Controls.Add(label7);
            groupBox5.Location = new Point(12, 651);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(808, 110);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "기타";
            // 
            // button1
            // 
            button1.Location = new Point(246, 26);
            button1.Name = "button1";
            button1.Size = new Size(104, 24);
            button1.TabIndex = 5;
            button1.Text = "feed && cut";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(163, 27);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(77, 23);
            textBox5.TabIndex = 4;
            textBox5.Text = "5";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(18, 30);
            label7.Name = "label7";
            label7.Size = new Size(115, 15);
            label7.TabIndex = 0;
            label7.Text = "용지 절삭 전 행 이동";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(832, 773);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "프린터 프로토콜 테스트";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button ClosePortButton;
        private Button OpenPortButton;
        private ComboBox BaudrateComboBox;
        private Label label3;
        private ComboBox SerialPortComboBox;
        private Label label2;
        private ComboBox comboBox1;
        private Label label1;
        private GroupBox groupBox2;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Button button3;
        private TextBox textBox2;
        private TextBox textBox1;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private GroupBox groupBox3;
        private Button button4;
        private CheckBox checkBox1;
        private TextBox textBox4;
        private Label label5;
        private TextBox textBox3;
        private Label label4;
        private GroupBox groupBox4;
        private Button button5;
        private PictureBox pictureBox1;
        private Button button6;
        private Label label6;
        private OpenFileDialog openFileDialog1;
        private GroupBox groupBox5;
        private Button button1;
        private TextBox textBox5;
        private Label label7;
    }
}