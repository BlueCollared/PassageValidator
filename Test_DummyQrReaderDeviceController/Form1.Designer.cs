namespace Test_DummyQrReaderDeviceController
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
            btnStartDetection = new Button();
            txtLog = new TextBox();
            btnStopDetect = new Button();
            SuspendLayout();
            // 
            // btnStartDetection
            // 
            btnStartDetection.Location = new Point(114, 105);
            btnStartDetection.Name = "btnStartDetection";
            btnStartDetection.Size = new Size(104, 23);
            btnStartDetection.TabIndex = 0;
            btnStartDetection.Text = "Start Detect";
            btnStartDetection.UseVisualStyleBackColor = true;
            btnStartDetection.Click += btnStartDetection_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(335, 72);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(216, 202);
            txtLog.TabIndex = 1;
            // 
            // btnStopDetect
            // 
            btnStopDetect.Location = new Point(114, 170);
            btnStopDetect.Name = "btnStopDetect";
            btnStopDetect.Size = new Size(104, 23);
            btnStopDetect.TabIndex = 2;
            btnStopDetect.Text = "Stop Detect";
            btnStopDetect.UseVisualStyleBackColor = true;
            btnStopDetect.Click += btnStopDetect_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStopDetect);
            Controls.Add(txtLog);
            Controls.Add(btnStartDetection);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStartDetection;
        private TextBox txtLog;
        private Button btnStopDetect;
    }
}
