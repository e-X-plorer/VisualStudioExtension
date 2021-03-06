﻿namespace Extension
{
    partial class RefactorDialog
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
            this.Text1 = new System.Windows.Forms.Label();
            this.ComboBox = new System.Windows.Forms.ComboBox();
            this.Text3 = new System.Windows.Forms.Label();
            this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.FileNameBox = new System.Windows.Forms.TextBox();
            this.Text2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Text1
            // 
            this.Text1.AutoSize = true;
            this.Text1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Text1.Location = new System.Drawing.Point(12, 9);
            this.Text1.Name = "Text1";
            this.Text1.Size = new System.Drawing.Size(231, 15);
            this.Text1.TabIndex = 1;
            this.Text1.Text = "Select class from the current file to divide:";
            // 
            // ComboBox
            // 
            this.ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.ComboBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ComboBox.FormattingEnabled = true;
            this.ComboBox.Location = new System.Drawing.Point(12, 27);
            this.ComboBox.Name = "ComboBox";
            this.ComboBox.Size = new System.Drawing.Size(460, 22);
            this.ComboBox.TabIndex = 2;
            this.ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // Text3
            // 
            this.Text3.AutoSize = true;
            this.Text3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Text3.Location = new System.Drawing.Point(9, 103);
            this.Text3.Name = "Text3";
            this.Text3.Size = new System.Drawing.Size(239, 15);
            this.Text3.TabIndex = 3;
            this.Text3.Text = "Select members to move to a separate file:";
            // 
            // CheckedListBox
            // 
            this.CheckedListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CheckedListBox.FormattingEnabled = true;
            this.CheckedListBox.Location = new System.Drawing.Point(12, 121);
            this.CheckedListBox.Name = "CheckedListBox";
            this.CheckedListBox.Size = new System.Drawing.Size(460, 191);
            this.CheckedListBox.TabIndex = 4;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ApplyButton.Location = new System.Drawing.Point(15, 326);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(457, 23);
            this.ApplyButton.TabIndex = 5;
            this.ApplyButton.Text = "Move members";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // FileNameBox
            // 
            this.FileNameBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FileNameBox.Location = new System.Drawing.Point(12, 70);
            this.FileNameBox.Name = "FileNameBox";
            this.FileNameBox.Size = new System.Drawing.Size(460, 22);
            this.FileNameBox.TabIndex = 6;
            // 
            // Text2
            // 
            this.Text2.AutoSize = true;
            this.Text2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Text2.Location = new System.Drawing.Point(12, 52);
            this.Text2.Name = "Text2";
            this.Text2.Size = new System.Drawing.Size(169, 15);
            this.Text2.TabIndex = 7;
            this.Text2.Text = "New file to move members to:";
            // 
            // RefactorDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.Text2);
            this.Controls.Add(this.FileNameBox);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.CheckedListBox);
            this.Controls.Add(this.Text3);
            this.Controls.Add(this.ComboBox);
            this.Controls.Add(this.Text1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RefactorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Refactoring";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Text1;
        private System.Windows.Forms.ComboBox ComboBox;
        private System.Windows.Forms.Label Text3;
        private System.Windows.Forms.CheckedListBox CheckedListBox;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.TextBox FileNameBox;
        private System.Windows.Forms.Label Text2;
    }
}