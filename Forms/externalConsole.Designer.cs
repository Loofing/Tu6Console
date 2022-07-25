namespace loofer.Forms
{
    partial class externalConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(externalConsole));
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.consoleWindow = new System.Windows.Forms.RichTextBox();
            this.consoleLine = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox
            // 
            this.imageBox.Image = ((System.Drawing.Image)(resources.GetObject("imageBox.Image")));
            this.imageBox.Location = new System.Drawing.Point(12, 13);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(611, 61);
            this.imageBox.TabIndex = 0;
            this.imageBox.TabStop = false;
            // 
            // consoleWindow
            // 
            this.consoleWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.consoleWindow.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleWindow.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.consoleWindow.Location = new System.Drawing.Point(13, 81);
            this.consoleWindow.Name = "consoleWindow";
            this.consoleWindow.ReadOnly = true;
            this.consoleWindow.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.consoleWindow.Size = new System.Drawing.Size(610, 285);
            this.consoleWindow.TabIndex = 1;
            this.consoleWindow.Text = "";
            this.consoleWindow.TextChanged += new System.EventHandler(this.consoleWindow_TextChanged);
            // 
            // consoleLine
            // 
            this.consoleLine.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.consoleLine.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.consoleLine.Location = new System.Drawing.Point(12, 372);
            this.consoleLine.Name = "consoleLine";
            this.consoleLine.Size = new System.Drawing.Size(611, 20);
            this.consoleLine.TabIndex = 4;
            this.consoleLine.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.consoleLine_PreviewKeyDown);
            // 
            // externalConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 404);
            this.Controls.Add(this.consoleLine);
            this.Controls.Add(this.consoleWindow);
            this.Controls.Add(this.imageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "externalConsole";
            this.Text = "External Console";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.RichTextBox consoleWindow;
        private System.Windows.Forms.TextBox consoleLine;
    }
}