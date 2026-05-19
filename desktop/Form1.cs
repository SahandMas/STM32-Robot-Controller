using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace MP_lab_HW3
{
    public class Form1 : Form
    {
        private ComboBox cboPort;
        private Button btnOpen;
        private Button btnClose;
        private Button btnSend;
        private Button btnReceive;
        private TextBox txtMessage;
        private TextBox txtReceive;
        private Label lblPort;
        private Label lblMessage;
        private Label lblReceive;
        private System.ComponentModel.IContainer components;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label1;
        private Label label2;
        private TextBox motor1_textbox;
        private TextBox motor2_textbox;
        private Label label3;
        private TextBox analog_textbox;

        private Label pwmLabel;
        private TextBox pwmFreqTextBox;

        private int motor_direction; // 0 = left, 1 = right
        private SerialPort serialPort1;

        private readonly StringBuilder rxBuffer = new StringBuilder();

        // [LAB8 ADD] UI elements for Lab8 tick display + logging
        private Label tickLabel;
        private TextBox tickTextBox;
        private CheckBox tickLogCheckBox;
        private Button tickChooseFileBtn;

        // [LAB8 ADD] logging fields
        private readonly object tickFileLock = new object();
        private string tickLogPath;

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnReceive = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblReceive = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.motor1_textbox = new System.Windows.Forms.TextBox();
            this.motor2_textbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.analog_textbox = new System.Windows.Forms.TextBox();
            this.pwmLabel = new System.Windows.Forms.Label();
            this.pwmFreqTextBox = new System.Windows.Forms.TextBox();

            // [LAB8 ADD] create Lab8 controls
            this.tickLabel = new System.Windows.Forms.Label();
            this.tickTextBox = new System.Windows.Forms.TextBox();
            this.tickLogCheckBox = new System.Windows.Forms.CheckBox();
            this.tickChooseFileBtn = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // cboPort
            this.cboPort.Location = new System.Drawing.Point(80, 15);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(120, 21);
            this.cboPort.TabIndex = 1;
            this.cboPort.DropDownStyle = ComboBoxStyle.DropDownList;

            // btnOpen
            this.btnOpen.Location = new System.Drawing.Point(220, 14);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(80, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(310, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // btnSend
            this.btnSend.Location = new System.Drawing.Point(401, 221);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(129, 40);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // btnReceive
            this.btnReceive.Location = new System.Drawing.Point(401, 437);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(129, 36);
            this.btnReceive.TabIndex = 9;
            this.btnReceive.Text = "Receive";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);

            // txtMessage
            this.txtMessage.Location = new System.Drawing.Point(80, 55);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(450, 160);
            this.txtMessage.TabIndex = 5;

            // txtReceive
            this.txtReceive.Location = new System.Drawing.Point(80, 279);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceive.Size = new System.Drawing.Size(450, 140);
            this.txtReceive.TabIndex = 8;

            // lblPort
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(20, 20);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 13);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Port:";

            // lblMessage
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(20, 60);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(35, 13);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "Send:";

            // lblReceive
            this.lblReceive.AutoSize = true;
            this.lblReceive.Location = new System.Drawing.Point(12, 279);
            this.lblReceive.Name = "lblReceive";
            this.lblReceive.Size = new System.Drawing.Size(50, 13);
            this.lblReceive.TabIndex = 7;
            this.lblReceive.Text = "Receive:";

            // radioButton1 (Left)
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(819, 58);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(43, 17);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Left";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.change_dirction_left);

            // radioButton2 (Right)
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(819, 94);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(50, 17);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Right";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.change_dirction_right);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(536, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Motor1_Speed";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(536, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Motor2_Speed";

            // motor1_textbox
            this.motor1_textbox.Location = new System.Drawing.Point(648, 56);
            this.motor1_textbox.Name = "motor1_textbox";
            this.motor1_textbox.Size = new System.Drawing.Size(164, 20);
            this.motor1_textbox.TabIndex = 14;

            // motor2_textbox
            this.motor2_textbox.Location = new System.Drawing.Point(645, 92);
            this.motor2_textbox.Name = "motor2_textbox";
            this.motor2_textbox.Size = new System.Drawing.Size(167, 20);
            this.motor2_textbox.TabIndex = 15;

            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(536, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Analog voltage";

            // analog_textbox
            this.analog_textbox.Location = new System.Drawing.Point(648, 276);
            this.analog_textbox.Name = "analog_textbox";
            this.analog_textbox.ReadOnly = true;
            this.analog_textbox.Size = new System.Drawing.Size(164, 20);
            this.analog_textbox.TabIndex = 17;

            // pwmLabel
            this.pwmLabel.AutoSize = true;
            this.pwmLabel.Location = new System.Drawing.Point(536, 342);
            this.pwmLabel.Name = "pwmLabel";
            this.pwmLabel.Size = new System.Drawing.Size(92, 13);
            this.pwmLabel.TabIndex = 18;
            this.pwmLabel.Text = "PWM Frequency :";

            // pwmFreqTextBox
            this.pwmFreqTextBox.Location = new System.Drawing.Point(648, 339);
            this.pwmFreqTextBox.Name = "pwmFreqTextBox";
            this.pwmFreqTextBox.ReadOnly = true;
            this.pwmFreqTextBox.Size = new System.Drawing.Size(164, 20);
            this.pwmFreqTextBox.TabIndex = 19;

            // [LAB8 ADD] tickLabel
            this.tickLabel.AutoSize = true;
            this.tickLabel.Location = new System.Drawing.Point(536, 380);
            this.tickLabel.Name = "tickLabel";
            this.tickLabel.Size = new System.Drawing.Size(83, 13);
            this.tickLabel.TabIndex = 20;
            this.tickLabel.Text = "HAL Tick (ms):";

            // [LAB8 ADD] tickTextBox
            this.tickTextBox.Location = new System.Drawing.Point(648, 377);
            this.tickTextBox.Name = "tickTextBox";
            this.tickTextBox.ReadOnly = true;
            this.tickTextBox.Size = new System.Drawing.Size(164, 20);
            this.tickTextBox.TabIndex = 21;

            // [LAB8 ADD] tickLogCheckBox
            this.tickLogCheckBox.AutoSize = true;
            this.tickLogCheckBox.Location = new System.Drawing.Point(539, 410);
            this.tickLogCheckBox.Name = "tickLogCheckBox";
            this.tickLogCheckBox.Size = new System.Drawing.Size(122, 17);
            this.tickLogCheckBox.TabIndex = 22;
            this.tickLogCheckBox.Text = "Save ticks to file";
            this.tickLogCheckBox.UseVisualStyleBackColor = true;
            this.tickLogCheckBox.Checked = true;

            // [LAB8 ADD] tickChooseFileBtn
            this.tickChooseFileBtn.Location = new System.Drawing.Point(668, 406);
            this.tickChooseFileBtn.Name = "tickChooseFileBtn";
            this.tickChooseFileBtn.Size = new System.Drawing.Size(144, 23);
            this.tickChooseFileBtn.TabIndex = 23;
            this.tickChooseFileBtn.Text = "Choose tick file...";
            this.tickChooseFileBtn.UseVisualStyleBackColor = true;
            this.tickChooseFileBtn.Click += new System.EventHandler(this.tickChooseFileBtn_Click);

            // Form1
            this.ClientSize = new System.Drawing.Size(876, 485);
            this.Controls.Add(this.tickChooseFileBtn);
            this.Controls.Add(this.tickLogCheckBox);
            this.Controls.Add(this.tickTextBox);
            this.Controls.Add(this.tickLabel);

            this.Controls.Add(this.pwmFreqTextBox);
            this.Controls.Add(this.pwmLabel);
            this.Controls.Add(this.analog_textbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.motor2_textbox);
            this.Controls.Add(this.motor1_textbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.cboPort);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblReceive);
            this.Controls.Add(this.txtReceive);
            this.Controls.Add(this.btnReceive);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Port Demo";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void change_dirction_right(object sender, EventArgs e)
        {
            if (radioButton2.Checked) motor_direction = 1;
        }

        private void change_dirction_left(object sender, EventArgs e)
        {
            if (radioButton1.Checked) motor_direction = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                Array.Sort(ports, StringComparer.OrdinalIgnoreCase);

                cboPort.Items.Clear();
                cboPort.Items.AddRange(ports);

                if (cboPort.Items.Count > 0)
                    cboPort.SelectedIndex = 0;

                btnClose.Enabled = false;

                radioButton2.Checked = true;
                motor_direction = 1;

                // [LAB8 ADD] default log file path (next to exe)
                tickLogPath = Path.Combine(Application.StartupPath, "ticks.txt");

                AppendReceive("Ready.");
                AppendReceive("Tick file: " + tickLogPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading ports: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboPort.SelectedItem == null)
                {
                    MessageBox.Show("Please select a COM port.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (serialPort1.IsOpen)
                {
                    MessageBox.Show("Port is already open.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                serialPort1.PortName = cboPort.Text;
                serialPort1.BaudRate = 115200;
                serialPort1.Parity = Parity.None;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;

                serialPort1.NewLine = "\n";
                serialPort1.ReadTimeout = 500;
                serialPort1.WriteTimeout = 500;

                serialPort1.DtrEnable = true;
                serialPort1.RtsEnable = true;

                serialPort1.DataReceived -= SerialPort1_DataReceived;
                serialPort1.DataReceived += SerialPort1_DataReceived;

                serialPort1.Open();

                btnOpen.Enabled = false;
                btnClose.Enabled = true;

                AppendReceive("Opened: " + serialPort1.PortName);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied. The port may be open in another program.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("I/O error opening port: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();

                btnOpen.Enabled = true;
                btnClose.Enabled = false;

                AppendReceive("Closed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error closing port: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    MessageBox.Show("Port is not open.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txtMessage.Text))
                {
                    string[] lines = txtMessage.Text.Replace("\r", "").Split('\n');
                    foreach (var line in lines)
                    {
                        string cmd = (line ?? "").Trim();
                        if (cmd.Length == 0) continue;
                        SendCommand(cmd);
                    }
                }

                if (!string.IsNullOrWhiteSpace(motor1_textbox.Text))
                {
                    if (int.TryParse(motor1_textbox.Text.Trim(), out int duty))
                    {
                        if (duty < 0 || duty > 100)
                        {
                            MessageBox.Show("Motor1_Speed must be 0..100 (PWM duty percent).",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        SendCommand($"PWM {duty}");
                    }
                    else
                    {
                        MessageBox.Show("Invalid Motor1_Speed number.",
                            "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                SendCommand($"OUT {motor_direction}");

                AppendReceive("Sent: PWM (if provided) + OUT (direction).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    MessageBox.Show("Port is not open.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SendCommand("ADC?");
                SendCommand("MEAS?");
                AppendReceive("Requested: ADC? and MEAS?");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendCommand(string cmd)
        {
            serialPort1.Write(cmd.Trim() + "\n");
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadExisting();
                if (string.IsNullOrEmpty(data)) return;

                lock (rxBuffer)
                {
                    rxBuffer.Append(data);

                    while (true)
                    {
                        int idx = rxBuffer.ToString().IndexOf('\n');
                        if (idx < 0) break;

                        string line = rxBuffer.ToString(0, idx);
                        rxBuffer.Remove(0, idx + 1);

                        line = line.Trim();
                        if (line.Length == 0) continue;

                        ProcessLine(line);
                    }
                }
            }
            catch
            {
            }
        }

        private void ProcessLine(string line)
        {
            AppendReceive("RX: " + line);

            if (line.StartsWith("ADC ", StringComparison.OrdinalIgnoreCase))
            {
                string valStr = line.Substring(4).Trim();
                if (int.TryParse(valStr, out int adc))
                {
                    SafeUI(() => analog_textbox.Text = adc.ToString());
                }
                return;
            }

            if (line.StartsWith("PER ", StringComparison.OrdinalIgnoreCase))
            {
                string valStr = line.Substring(4).Trim();
                if (double.TryParse(valStr, out double perUs) && perUs > 0.0)
                {
                    double freqHz = 1_000_000.0 / perUs;
                    SafeUI(() => pwmFreqTextBox.Text = freqHz.ToString("F2"));
                }
                else
                {
                    SafeUI(() => pwmFreqTextBox.Text = "0");
                }
                return;
            }

            // [LAB8 ADD] Handle Lab8 tick line: "TICK <ms>"
            if (line.StartsWith("TICK ", StringComparison.OrdinalIgnoreCase))
            {
                string valStr = line.Substring(5).Trim();
                if (long.TryParse(valStr, out long tickMs) && tickMs >= 0)
                {
                    SafeUI(() => tickTextBox.Text = tickMs.ToString());

                    if (tickLogCheckBox.Checked)
                    {
                        AppendTickToFile(tickMs);
                    }
                }
                return;
            }
        }

        // [LAB8 ADD] Append tick value to txt file (one per line)
        private void AppendTickToFile(long tickMs)
        {
            try
            {
                lock (tickFileLock)
                {
                    File.AppendAllText(tickLogPath, tickMs.ToString() + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                SafeUI(() => AppendReceive("Tick log error: " + ex.Message));
            }
        }

        // [LAB8 ADD] Choose tick log file path
        private void tickChooseFileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Title = "Choose tick log file";
                    sfd.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FileName = "ticks.txt";

                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        tickLogPath = sfd.FileName;
                        AppendReceive("Tick file set to: " + tickLogPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error choosing file: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendReceive(string text)
        {
            SafeUI(() => txtReceive.AppendText(text + Environment.NewLine));
        }

        private void SafeUI(Action a)
        {
            if (this.IsHandleCreated && this.InvokeRequired)
                this.BeginInvoke(a);
            else
                a();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();
            }
            catch
            {
            }
        }
    }
}
