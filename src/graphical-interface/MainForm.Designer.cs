using System.Windows.Forms;

namespace graphical_interface {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.radioAll = new System.Windows.Forms.RadioButton();
            this.radioMaxWords = new System.Windows.Forms.RadioButton();
            this.radioDiffHeadMaxWords = new System.Windows.Forms.RadioButton();
            this.radioMaxAlpha = new System.Windows.Forms.RadioButton();
            this.checkHead = new System.Windows.Forms.CheckBox();
            this.checkTail = new System.Windows.Forms.CheckBox();
            this.checkLoop = new System.Windows.Forms.CheckBox();
            this.textHead = new System.Windows.Forms.TextBox();
            this.textTail = new System.Windows.Forms.TextBox();
            this.textWords = new System.Windows.Forms.TextBox();
            this.textChains = new System.Windows.Forms.TextBox();
            this.buttonRead = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioAll
            // 
            this.radioAll.AutoSize = true;
            this.radioAll.Location = new System.Drawing.Point(18, 11);
            this.radioAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioAll.Name = "radioAll";
            this.radioAll.Size = new System.Drawing.Size(103, 19);
            this.radioAll.TabIndex = 0;
            this.radioAll.Text = "所有单词链";
            this.radioAll.UseVisualStyleBackColor = true;
            this.radioAll.CheckedChanged += new System.EventHandler(this.radioAll_CheckedChanged);
            // 
            // radioMaxWords
            // 
            this.radioMaxWords.AutoSize = true;
            this.radioMaxWords.Checked = true;
            this.radioMaxWords.Location = new System.Drawing.Point(149, 11);
            this.radioMaxWords.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioMaxWords.Name = "radioMaxWords";
            this.radioMaxWords.Size = new System.Drawing.Size(103, 19);
            this.radioMaxWords.TabIndex = 1;
            this.radioMaxWords.TabStop = true;
            this.radioMaxWords.Text = "最多单词数";
            this.radioMaxWords.UseVisualStyleBackColor = true;
            // 
            // radioDiffHeadMaxWords
            // 
            this.radioDiffHeadMaxWords.AutoSize = true;
            this.radioDiffHeadMaxWords.Location = new System.Drawing.Point(273, 11);
            this.radioDiffHeadMaxWords.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioDiffHeadMaxWords.Name = "radioDiffHeadMaxWords";
            this.radioDiffHeadMaxWords.Size = new System.Drawing.Size(193, 19);
            this.radioDiffHeadMaxWords.TabIndex = 2;
            this.radioDiffHeadMaxWords.Text = "首字母不同，最多单词数";
            this.radioDiffHeadMaxWords.UseVisualStyleBackColor = true;
            this.radioDiffHeadMaxWords.CheckedChanged += new System.EventHandler(this.radioDiffHeadMaxWords_CheckedChanged);
            // 
            // radioMaxAlpha
            // 
            this.radioMaxAlpha.AutoSize = true;
            this.radioMaxAlpha.Location = new System.Drawing.Point(486, 11);
            this.radioMaxAlpha.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioMaxAlpha.Name = "radioMaxAlpha";
            this.radioMaxAlpha.Size = new System.Drawing.Size(103, 19);
            this.radioMaxAlpha.TabIndex = 3;
            this.radioMaxAlpha.Text = "最多字母数";
            this.radioMaxAlpha.UseVisualStyleBackColor = true;
            // 
            // checkHead
            // 
            this.checkHead.AutoSize = true;
            this.checkHead.Location = new System.Drawing.Point(18, 43);
            this.checkHead.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkHead.Name = "checkHead";
            this.checkHead.Size = new System.Drawing.Size(104, 19);
            this.checkHead.TabIndex = 4;
            this.checkHead.Text = "指定首字母";
            this.checkHead.UseVisualStyleBackColor = true;
            this.checkHead.CheckedChanged += new System.EventHandler(this.checkHead_CheckedChanged);
            this.checkHead.EnabledChanged += new System.EventHandler(this.checkHead_EnabledChanged);
            // 
            // checkTail
            // 
            this.checkTail.AutoSize = true;
            this.checkTail.Location = new System.Drawing.Point(287, 43);
            this.checkTail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkTail.Name = "checkTail";
            this.checkTail.Size = new System.Drawing.Size(104, 19);
            this.checkTail.TabIndex = 5;
            this.checkTail.Text = "指定尾字母";
            this.checkTail.UseVisualStyleBackColor = true;
            this.checkTail.CheckedChanged += new System.EventHandler(this.checkTail_CheckedChanged);
            this.checkTail.EnabledChanged += new System.EventHandler(this.checkTail_EnabledChanged);
            // 
            // checkLoop
            // 
            this.checkLoop.AutoSize = true;
            this.checkLoop.Location = new System.Drawing.Point(840, 11);
            this.checkLoop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkLoop.Name = "checkLoop";
            this.checkLoop.Size = new System.Drawing.Size(134, 19);
            this.checkLoop.TabIndex = 6;
            this.checkLoop.Text = "允许隐含单词环";
            this.checkLoop.UseVisualStyleBackColor = true;
            // 
            // textHead
            // 
            this.textHead.Enabled = false;
            this.textHead.Location = new System.Drawing.Point(138, 39);
            this.textHead.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textHead.MaxLength = 1;
            this.textHead.Name = "textHead";
            this.textHead.Size = new System.Drawing.Size(57, 25);
            this.textHead.TabIndex = 7;
            this.textHead.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textHead_KeyPress);
            // 
            // textTail
            // 
            this.textTail.Enabled = false;
            this.textTail.Location = new System.Drawing.Point(397, 39);
            this.textTail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textTail.MaxLength = 1;
            this.textTail.Name = "textTail";
            this.textTail.Size = new System.Drawing.Size(57, 25);
            this.textTail.TabIndex = 8;
            this.textTail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textTail_KeyPress);
            // 
            // textWords
            // 
            this.textWords.Location = new System.Drawing.Point(18, 78);
            this.textWords.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textWords.Multiline = true;
            this.textWords.Name = "textWords";
            this.textWords.Size = new System.Drawing.Size(470, 330);
            this.textWords.TabIndex = 9;
            // 
            // textChains
            // 
            this.textChains.Location = new System.Drawing.Point(504, 78);
            this.textChains.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textChains.Multiline = true;
            this.textChains.Name = "textChains";
            this.textChains.ReadOnly = true;
            this.textChains.Size = new System.Drawing.Size(470, 330);
            this.textChains.TabIndex = 10;
            // 
            // buttonRead
            // 
            this.buttonRead.Location = new System.Drawing.Point(730, 37);
            this.buttonRead.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(114, 27);
            this.buttonRead.TabIndex = 11;
            this.buttonRead.Text = "从文件导入词库";
            this.buttonRead.UseVisualStyleBackColor = true;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(860, 37);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(114, 27);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.Text = "保存单词链";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(624, 37);
            this.buttonRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(87, 27);
            this.buttonRun.TabIndex = 13;
            this.buttonRun.Text = "求单词链";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(986, 419);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonRead);
            this.Controls.Add(this.textChains);
            this.Controls.Add(this.textWords);
            this.Controls.Add(this.textTail);
            this.Controls.Add(this.textHead);
            this.Controls.Add(this.checkLoop);
            this.Controls.Add(this.checkTail);
            this.Controls.Add(this.checkHead);
            this.Controls.Add(this.radioMaxAlpha);
            this.Controls.Add(this.radioDiffHeadMaxWords);
            this.Controls.Add(this.radioMaxWords);
            this.Controls.Add(this.radioAll);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "最长英语单词链";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton radioAll;
        private RadioButton radioMaxWords;
        private RadioButton radioDiffHeadMaxWords;
        private RadioButton radioMaxAlpha;
        private CheckBox checkHead;
        private CheckBox checkTail;
        private CheckBox checkLoop;
        private TextBox textHead;
        private TextBox textTail;
        private TextBox textWords;
        private TextBox textChains;
        private Button buttonRead;
        private Button buttonSave;
        private Button buttonRun;
    }
}
