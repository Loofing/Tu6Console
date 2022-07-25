
namespace loofer.Forms
{
    partial class PresetLoader
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
            this.presetBox = new System.Windows.Forms.ListBox();
            this.weaponBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.jsonBox = new System.Windows.Forms.RichTextBox();
            this.setButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // presetBox
            // 
            this.presetBox.FormattingEnabled = true;
            this.presetBox.Location = new System.Drawing.Point(13, 13);
            this.presetBox.Name = "presetBox";
            this.presetBox.ScrollAlwaysVisible = true;
            this.presetBox.Size = new System.Drawing.Size(136, 212);
            this.presetBox.TabIndex = 0;
            this.presetBox.SelectedIndexChanged += new System.EventHandler(this.presetBox_SelectedIndexChanged);
            // 
            // weaponBox
            // 
            this.weaponBox.Enabled = false;
            this.weaponBox.Location = new System.Drawing.Point(232, 13);
            this.weaponBox.Name = "weaponBox";
            this.weaponBox.Size = new System.Drawing.Size(100, 20);
            this.weaponBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(176, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Weapon";
            // 
            // jsonBox
            // 
            this.jsonBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jsonBox.Font = new System.Drawing.Font("Cascadia Mono", 7.25F);
            this.jsonBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.jsonBox.Location = new System.Drawing.Point(179, 42);
            this.jsonBox.Name = "jsonBox";
            this.jsonBox.ReadOnly = true;
            this.jsonBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.jsonBox.ShortcutsEnabled = false;
            this.jsonBox.Size = new System.Drawing.Size(368, 211);
            this.jsonBox.TabIndex = 4;
            this.jsonBox.Text = "";
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(13, 232);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(136, 23);
            this.setButton.TabIndex = 5;
            this.setButton.Text = "Set Preset";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // PresetLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 265);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.jsonBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.weaponBox);
            this.Controls.Add(this.presetBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PresetLoader";
            this.Text = "Preset Loader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox presetBox;
        private System.Windows.Forms.TextBox weaponBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox jsonBox;
        private System.Windows.Forms.Button setButton;
    }
}