namespace QrReaderGuiSimulator
{
    partial class QrReaderGuiSimulator
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected void Dispose(bool disposing)
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
            btnStatusChanged = new Button();
            chkConnected = new CheckBox();
            label1 = new Label();
            txtFirmware = new TextBox();
            chkScanning = new CheckBox();
            groupBox1 = new GroupBox();
            chkStartAnswer = new CheckBox();
            chkStartDetectAnswer = new CheckBox();
            btnSynchronize = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStatusChanged
            // 
            btnStatusChanged.Location = new Point(60, 31);
            btnStatusChanged.Name = "btnStatusChanged";
            btnStatusChanged.Size = new Size(120, 66);
            btnStatusChanged.TabIndex = 0;
            btnStatusChanged.Text = "Status Changed";
            btnStatusChanged.UseVisualStyleBackColor = true;
            btnStatusChanged.Click += btnStatusChanged_Click;
            // 
            // chkConnected
            // 
            chkConnected.AutoSize = true;
            chkConnected.Location = new Point(30, 33);
            chkConnected.Name = "chkConnected";
            chkConnected.Size = new Size(89, 19);
            chkConnected.TabIndex = 1;
            chkConnected.Text = "Connected?";
            chkConnected.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 95);
            label1.Name = "label1";
            label1.Size = new Size(97, 15);
            label1.TabIndex = 2;
            label1.Text = "Firmware Version";
            // 
            // txtFirmware
            // 
            txtFirmware.Location = new Point(30, 69);
            txtFirmware.Name = "txtFirmware";
            txtFirmware.Size = new Size(100, 23);
            txtFirmware.TabIndex = 3;
            txtFirmware.Text = "3.2";
            // 
            // chkScanning
            // 
            chkScanning.AutoSize = true;
            chkScanning.Location = new Point(30, 134);
            chkScanning.Name = "chkScanning";
            chkScanning.Size = new Size(80, 19);
            chkScanning.TabIndex = 4;
            chkScanning.Text = "Scanning?";
            chkScanning.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkConnected);
            groupBox1.Controls.Add(chkScanning);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtFirmware);
            groupBox1.Location = new Point(50, 103);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(144, 179);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Status";
            // 
            // chkStartAnswer
            // 
            chkStartAnswer.AutoSize = true;
            chkStartAnswer.Checked = true;
            chkStartAnswer.CheckState = CheckState.Checked;
            chkStartAnswer.Location = new Point(265, 56);
            chkStartAnswer.Name = "chkStartAnswer";
            chkStartAnswer.Size = new Size(92, 19);
            chkStartAnswer.TabIndex = 6;
            chkStartAnswer.Text = "Start Answer";
            chkStartAnswer.UseVisualStyleBackColor = true;
            chkStartAnswer.CheckedChanged += chkStartAnswer_CheckedChanged;
            // 
            // chkStartDetectAnswer
            // 
            chkStartDetectAnswer.AutoSize = true;
            chkStartDetectAnswer.Checked = true;
            chkStartDetectAnswer.CheckState = CheckState.Checked;
            chkStartDetectAnswer.Location = new Point(265, 81);
            chkStartDetectAnswer.Name = "chkStartDetectAnswer";
            chkStartDetectAnswer.Size = new Size(129, 19);
            chkStartDetectAnswer.TabIndex = 7;
            chkStartDetectAnswer.Text = "Start Detect Answer";
            chkStartDetectAnswer.UseVisualStyleBackColor = true;
            chkStartDetectAnswer.CheckedChanged += chkStartDetectAnswer_CheckedChanged;
            // 
            // btnSynchronize
            // 
            btnSynchronize.Location = new Point(340, 192);
            btnSynchronize.Name = "btnSynchronize";
            btnSynchronize.Size = new Size(120, 66);
            btnSynchronize.TabIndex = 8;
            btnSynchronize.Text = "Synchronize";
            btnSynchronize.UseVisualStyleBackColor = true;
            btnSynchronize.Click += btnSynchronize_Click;
            // 
            // QrReaderGuiSimulator
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSynchronize);
            Controls.Add(chkStartDetectAnswer);
            Controls.Add(chkStartAnswer);
            Controls.Add(groupBox1);
            Controls.Add(btnStatusChanged);
            Name = "QrReaderGuiSimulator";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStatusChanged;
        private CheckBox chkConnected;
        private Label label1;
        private TextBox txtFirmware;
        private CheckBox chkScanning;
        private GroupBox groupBox1;
        private CheckBox chkStartAnswer;
        private CheckBox chkStartDetectAnswer;
        private Button btnSynchronize;
    }
}
