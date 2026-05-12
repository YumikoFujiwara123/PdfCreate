namespace PdfCreate
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtCsvPath = new System.Windows.Forms.TextBox();
            this.cmdCreatePdf = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmdマージテスト = new System.Windows.Forms.Button();
            this.optKojinbetu = new System.Windows.Forms.RadioButton();
            this.optJunihyou = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cmdItiranTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCsvPath
            // 
            this.txtCsvPath.AllowDrop = true;
            this.txtCsvPath.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCsvPath.Location = new System.Drawing.Point(107, 124);
            this.txtCsvPath.Name = "txtCsvPath";
            this.txtCsvPath.Size = new System.Drawing.Size(432, 33);
            this.txtCsvPath.TabIndex = 1;
            this.txtCsvPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtCsvPath_DragDrop);
            this.txtCsvPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtCsvPath_DragEnter);
            // 
            // cmdCreatePdf
            // 
            this.cmdCreatePdf.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmdCreatePdf.Location = new System.Drawing.Point(222, 266);
            this.cmdCreatePdf.Name = "cmdCreatePdf";
            this.cmdCreatePdf.Size = new System.Drawing.Size(223, 58);
            this.cmdCreatePdf.TabIndex = 2;
            this.cmdCreatePdf.Text = "PDF作成";
            this.cmdCreatePdf.UseVisualStyleBackColor = true;
            this.cmdCreatePdf.Click += new System.EventHandler(this.cmdCreatePdf_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(39, 405);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 44);
            this.button1.TabIndex = 3;
            this.button1.Text = "テスト";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(110, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(526, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "↑公開模試データ（ｃｓｖ）を、このテキストボックスへドラグ＆ドロップしてください";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(107, 356);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(529, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.Location = new System.Drawing.Point(222, 212);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(223, 30);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "＿＿＿＿＿＿＿＿＿＿";
            // 
            // cmdマージテスト
            // 
            this.cmdマージテスト.Location = new System.Drawing.Point(172, 405);
            this.cmdマージテスト.Name = "cmdマージテスト";
            this.cmdマージテスト.Size = new System.Drawing.Size(112, 44);
            this.cmdマージテスト.TabIndex = 7;
            this.cmdマージテスト.Text = "マージテスト";
            this.cmdマージテスト.UseVisualStyleBackColor = true;
            this.cmdマージテスト.Visible = false;
            this.cmdマージテスト.Click += new System.EventHandler(this.button2_Click);
            // 
            // optKojinbetu
            // 
            this.optKojinbetu.AutoSize = true;
            this.optKojinbetu.Checked = true;
            this.optKojinbetu.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.optKojinbetu.Location = new System.Drawing.Point(160, 54);
            this.optKojinbetu.Name = "optKojinbetu";
            this.optKojinbetu.Size = new System.Drawing.Size(124, 25);
            this.optKojinbetu.TabIndex = 8;
            this.optKojinbetu.TabStop = true;
            this.optKojinbetu.Text = "個人別成績表";
            this.optKojinbetu.UseVisualStyleBackColor = true;
            // 
            // optJunihyou
            // 
            this.optJunihyou.AutoSize = true;
            this.optJunihyou.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.optJunihyou.Location = new System.Drawing.Point(352, 54);
            this.optJunihyou.Name = "optJunihyou";
            this.optJunihyou.Size = new System.Drawing.Size(108, 25);
            this.optJunihyou.TabIndex = 9;
            this.optJunihyou.Text = "成績順位表";
            this.optJunihyou.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 25);
            this.label2.TabIndex = 11;
            this.label2.Text = "【成績表作成専用ツール】";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(538, 397);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(12, 15);
            this.lblVersion.TabIndex = 12;
            this.lblVersion.Text = "_";
            // 
            // cmdItiranTest
            // 
            this.cmdItiranTest.Location = new System.Drawing.Point(493, 261);
            this.cmdItiranTest.Name = "cmdItiranTest";
            this.cmdItiranTest.Size = new System.Drawing.Size(95, 44);
            this.cmdItiranTest.TabIndex = 13;
            this.cmdItiranTest.Text = "一覧表テスト";
            this.cmdItiranTest.UseVisualStyleBackColor = true;
            this.cmdItiranTest.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 450);
            this.Controls.Add(this.cmdItiranTest);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.optJunihyou);
            this.Controls.Add(this.optKojinbetu);
            this.Controls.Add(this.cmdマージテスト);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdCreatePdf);
            this.Controls.Add(this.txtCsvPath);
            this.Name = "FormMain";
            this.Text = "FormMain_成績表作成専用ツール";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox txtCsvPath;
        private Button cmdCreatePdf;
        private Button button1;
        private Label label1;
        private ProgressBar progressBar1;
        private Label lblStatus;
        private Button cmdマージテスト;
        private RadioButton optKojinbetu;
        private RadioButton optJunihyou;
        private Label label2;
        private Label lblVersion;
        private Button cmdItiranTest;
    }
}